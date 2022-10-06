using HttpWrapper;
using System.Text.Json;

namespace JiraDataFetcher;

public class DataFetcher : IDataFetcher
{
    private readonly IHttpGetter _httpGetter;
    private readonly string _jiraDomain;
    private readonly string _username;
    private readonly string _password;

    public DataFetcher(IHttpGetter httpGetter, string jiraDomain, string username, string password)
    {
        _httpGetter = httpGetter;
        _jiraDomain = jiraDomain;
        _username = username;
        _password = password;
    }

    public async Task<JiraIssue?> FetchIssue(string key)
    {
        var response = await _httpGetter.GetWithBasicAuthentication(
            $"https://{_jiraDomain}.atlassian.net",
            $"/rest/api/3/issue/{key}",
            _username,
            _password);

        var jiraIssue = JsonSerializer.Deserialize<JiraIssue>(response);

        return jiraIssue;
    }
}
