namespace SharedKernel.Providers;

public class DownloadResponse
{
    // chuỗi chứa Presigned URL, có thể dùng chuỗi này để tải tệp xuống từ S3 mà không cần xác thực
    public string PresignedUrl { get; set; }

    // Là thời điểm hết hạn của Presigned URL, được biểu diễn dưới dạng một giá trị số nguyên. Sau thời điểm này, URL sẽ không còn hiệu lực.
    public int ExpiryTime { get; set; }

    // Là tên của tệp cần tải xuống. Điều này có thể hữu ích khi người dùng muốn lưu trữ tệp với tên cụ thể.
    public string FileName { get; set; }

    // Là một MemoryStream, một bộ đệm trong bộ nhớ RAM, có thể chứa dữ liệu của tệp tải xuống.
    // Điều này cho phép bạn đọc dữ liệu từ S3 và giữ nó trong bộ nhớ cho việc xử lý tiếp theo mà không cần lưu vào đĩa cứng trước.
    public MemoryStream MemoryStream { get; set; }
}