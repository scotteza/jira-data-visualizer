using System.Text.Json.Serialization;

namespace JiraDataFetcher;

public class JiraIssue
{
    [JsonPropertyName("key")]
    public string Key { get; }

    [JsonPropertyName("fields")]
    public JiraIssueFields Fields { get; }

    public JiraIssue(string key, JiraIssueFields fields)
    {
        Key = key;
        Fields = fields;
    }
}
