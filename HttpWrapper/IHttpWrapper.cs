namespace HttpWrapper;

public interface IHttpWrapper
{
    Task<string> GetWithBasicAuthentication(string baseAddress, string requestUri, string username, string password);
    Task<string> Post(string baseAddress, string requestUri, string body);
}
