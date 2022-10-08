using System.Text.Json.Serialization;

namespace JiraDataFetcher;

public class JiraIssue
{
    [JsonPropertyName("key")]
    public string Key { get; }

    // TODO: it would be nice if this property wasn't public
    [JsonPropertyName("fields")]
    public JiraIssueFields? Fields { get; }
    public string Summary => Fields != null ? Fields.Summary : "";

    // TODO: it would be nice if this property wasn't public
    [JsonPropertyName("parent")]
    public JiraIssueParentEpic? ParentEpic { get; }
    public string ParentEpicKey => ParentEpic != null ? ParentEpic.Key : "";

    public JiraIssue(string key, JiraIssueFields fields, JiraIssueParentEpic parentEpic)
    {
        Key = key;
        Fields = fields;
        ParentEpic = parentEpic;
    }
}
