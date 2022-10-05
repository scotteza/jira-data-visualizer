using System.Text.Json.Serialization;

namespace JiraDataFetcher;

public class JiraIssue
{
    [JsonPropertyName("key")]
    public string Key { get; }

    public JiraIssue(string key)
    {
        Key = key;
    }
}
