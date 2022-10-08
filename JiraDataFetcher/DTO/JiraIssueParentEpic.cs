using System.Text.Json.Serialization;

namespace JiraDataFetcher.DTO;

public class JiraIssueParentEpic
{
    [JsonPropertyName("key")]
    public string Key { get; }

    public JiraIssueParentEpic(string key)
    {
        Key = key;
    }
}
