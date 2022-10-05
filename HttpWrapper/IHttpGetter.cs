namespace HttpWrapper;

public interface IHttpGetter
{
    public Task<string> GetWithBasicAuthentication(string baseAddress, string requestUri, string username, string password);
}
