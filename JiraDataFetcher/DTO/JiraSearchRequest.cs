using System.Text.Json.Serialization;

namespace JiraDataFetcher.DTO;

public class JiraSearchRequest
{
    [JsonPropertyName("jql")]
    public string Jql { get; }

    [JsonPropertyName("maxResults")]
    public int ResultsPerPage { get; }

    [JsonPropertyName("startAt")]
    public int StartAt { get; }

    [JsonPropertyName("fieldsByKeys")]
    public bool FieldsByKeys => false;

    [JsonPropertyName("fields")]
    public string[] Fields
    {
        get
        {
            return new[] { "key", "summary", "issuetype", "parent", "issuelinks", "status" };
        }
    }

    public JiraSearchRequest(string jql, int resultsPerPage, int startAt)
    {
        Jql = jql;
        ResultsPerPage = resultsPerPage;
        StartAt = startAt;
    }
}
