using System.Text.Json.Serialization;

namespace JiraDataFetcher.DTO;

public class JiraIssue
{
    [JsonPropertyName("key")]
    public string Key { get; }

    // TODO: it would be nice if this property wasn't public
    [JsonPropertyName("fields")]
    public JiraIssueFields? Fields { get; }
    public string Summary => Fields != null ? Fields.Summary : "";

    public string ParentEpicKey => Fields != null && Fields.ParentEpic != null ? Fields.ParentEpic.Key : "";

    public JiraIssue(string key, JiraIssueFields fields)
    {
        Key = key;
        Fields = fields;
    }
}
