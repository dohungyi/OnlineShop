namespace SharedKernel.Providers.HttpClients;

public interface IHttpClientProvider
{
    HttpClient HttpClient { get; }

    #region [DELETE]
    
    Task<HttpResponseMessage> DeleteAsync(string requestUri);

    Task<HttpResponseMessage> DeleteAsync(string requestUri, CancellationToken cancellationToken = default);

    Task<HttpResponseMessage> DeleteAsync(Uri requestUri);
    
    Task<HttpResponseMessage> DeleteAsync(Uri requestUri, CancellationToken cancellationToken = default);
    
    #endregion [DELETE]

    #region [GET]
    
    Task<HttpResponseMessage> GetAsync(string requestUri);
    
    Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken = default);
    
    Task<HttpResponseMessage> GetAsync(Uri requestUri);
    
    Task<HttpResponseMessage> GetAsync(Uri requestUri, CancellationToken cancellationToken = default);
    
    #endregion

    #region [POST]

    Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content);
    
    Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content, CancellationToken cancellationToken = default);
    
    Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content);
    
    Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken = default);

    #endregion [POST]

    #region [PUT]
    
    Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content);
    Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content, CancellationToken cancellationToken = default);
    Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content);
    Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken = default);
    
    #endregion [PUT]

    #region [SEND]
    
    Task<HttpResponseMessage> SendAsync(string requestUri, object content, HttpMethod method, string token = "");
    
    Task<HttpResponseMessage> SendAsync(string requestUri, object content, HttpMethod method, CancellationToken cancellationToken = default, string token = "");
    
    #endregion [SEND]
    
}