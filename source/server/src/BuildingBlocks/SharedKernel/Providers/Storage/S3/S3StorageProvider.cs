using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using SharedKernel.Core;
using SharedKernel.Log;

namespace SharedKernel.Providers;

public class S3StorageProvider : IS3StorageProvider
{
    private readonly AmazonS3Client _amazonS3Client;
    private const int expiryTime = 365; // days;
    
    public S3StorageProvider()
    {
        _amazonS3Client = new AmazonS3Client(DefaultS3Config.AccessKey, DefaultS3Config.SecretKey, new AmazonS3Config
        {
            RegionEndpoint = RegionEndpoint.APSoutheast1,
            RetryMode = RequestRetryMode.Standard,
            MaxErrorRetry = 3
        });
    }
    
    #region [IMPLEMENTS]
    
    public async Task<UploadResponse> UploadAsync(UploadRequest model, CancellationToken cancellationToken = default)
    {
        try
        {
            var currentFileName = $"{Guid.NewGuid()}{model.FileExtension}";
            var request = new PutObjectRequest()
            {
                BucketName = DefaultS3Config.Bucket,
                Key = string.IsNullOrEmpty(model.Prefix) ? $"{DefaultS3Config.Root}/{currentFileName}" : $"{DefaultS3Config.Root}/{model.Prefix}/{currentFileName}",
                InputStream = model.Stream,
            };

            await _amazonS3Client.PutObjectAsync(request, cancellationToken);
            return new UploadResponse()
            {
                OriginalFileName = model.FileName,
                CurrentFileName = currentFileName,
                FileExtension = model.FileExtension,
                Size = model.Size,
                Prefix = model.Prefix,
            };
        }
        catch (Exception ex)
        {
            Logging.Error(ex, "s3 upload failed");
            return new UploadResponse()
            {
                Success = false,
                OriginalFileName = model.FileName,
                ErrorMessage = ex.Message,
            };
        }
    }

    public async Task<List<UploadResponse>> UploadAsync(List<UploadRequest> models, CancellationToken cancellationToken = default)
    {
        var responses = await Task.WhenAll(models.Select(model => this.UploadAsync(model, cancellationToken)));
        return responses.ToList();
    }

    public async Task<DownloadResponse> DownloadAsync(string fileName, string version = "", CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _amazonS3Client.GetObjectAsync(new GetObjectRequest()
            {
                BucketName = DefaultS3Config.Bucket,
                Key = $"{DefaultS3Config.Root}/{fileName}",
            }, cancellationToken);

            var publicUrl = _amazonS3Client.GetPreSignedURL(new GetPreSignedUrlRequest()
            {
                BucketName = DefaultS3Config.Bucket,
                Key = $"{DefaultS3Config.Root}/{fileName}",
                Expires = DateTime.UtcNow.AddDays(expiryTime)
            });

            //var ms = new MemoryStream();
            //await response.ResponseStream.CopyToAsync(ms);

            Logging.Warning("Download file from S3 with key = " + fileName);

            return new DownloadResponse()
            {
                PresignedUrl = publicUrl,
                ExpiryTime = expiryTime,
                //MemoryStream = ms,
                FileName = fileName,
            };
        }
        catch (Exception ex)
        {
            Logging.Error(ex, $"s3 download failed with specified key = {fileName}");
            throw;
        }
    }
    public async Task<List<DownloadResponse>> DownloadAsync(List<string> fileNames, string version = "", CancellationToken cancellationToken = default)
        {
            var tasks = fileNames.Select(f => this.DownloadAsync(f, cancellationToken: cancellationToken));
            return (await Task.WhenAll(tasks)).ToList();
        }

        public async Task<List<DownloadResponse>> DownloadDirectoryAsync(string directory, string version = "", CancellationToken cancellationToken = default)
        {
            var request = new ListObjectsV2Request()
            {
                BucketName = DefaultS3Config.Bucket,
                Prefix = $"{DefaultS3Config.Root}/{directory}"
            };

            var listObjects = await _amazonS3Client.ListObjectsV2Async(request, cancellationToken);
            var objects = listObjects.S3Objects.Where(o => !o.Key.Equals($"{DefaultS3Config.Root}/{directory}/")).ToList();

            var tasks = objects.Select(s3 => Task.Run(() =>
                {
                    var publicUrl = _amazonS3Client.GetPreSignedURL(new GetPreSignedUrlRequest()
                    {
                        BucketName = DefaultS3Config.Bucket,
                        Key = s3.Key,
                        Expires = DateTime.UtcNow.AddDays(expiryTime)
                    });

                    return new DownloadResponse()
                    {
                        PresignedUrl = publicUrl,
                        ExpiryTime = expiryTime,
                        FileName = s3.Key
                    };
                }));
            var response = await Task.WhenAll(tasks);

            return response.ToList();
        }

        public async Task<List<DownloadResponse>> DownloadPagingAsync(string directory, int pageIndex, int pageSize, string version = "", CancellationToken cancellationToken = default)
        {
            var request = new ListObjectsV2Request()
            {
                BucketName = DefaultS3Config.Bucket,
                Prefix = $"{DefaultS3Config.Root}/{directory}",
                MaxKeys = pageSize
            };

            var paginatorResponse = _amazonS3Client.Paginators.ListObjectsV2(request);
            await foreach (var paginator in paginatorResponse.Responses)
            {

                var objects = paginator.S3Objects.Where(o => !o.Key.Equals($"{DefaultS3Config.Root}/{directory}/")).ToList();
                var tasks = objects.Select(s3 => Task.Run(() =>
                {
                    var publicUrl = _amazonS3Client.GetPreSignedURL(new GetPreSignedUrlRequest()
                    {
                        BucketName = DefaultS3Config.Bucket,
                        Key = s3.Key,
                        Expires = DateTime.UtcNow.AddDays(expiryTime)
                    });

                    return new DownloadResponse()
                    {
                        PresignedUrl = publicUrl,
                        ExpiryTime = expiryTime,
                    };
                }));

                var response = await Task.WhenAll(tasks);
                return response.ToList();
            }
            return new List<DownloadResponse>();
        }

        public async Task DeleteAsync(string fileName, string version = "", CancellationToken cancellationToken = default)
        {
            var request = new DeleteObjectRequest()
            {
                BucketName = DefaultS3Config.Bucket,
                Key = $"{DefaultS3Config.Root}/{fileName}",
            };
            await _amazonS3Client.DeleteObjectAsync(request, cancellationToken);
        }

        public void Dispose()
        {
            _amazonS3Client.Dispose();
        }
    
    #endregion
}