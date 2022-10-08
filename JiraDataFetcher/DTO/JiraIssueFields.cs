using System.Text.Json.Serialization;

namespace JiraDataFetcher.DTO;

public class JiraIssueFields
{
    [JsonPropertyName("summary")]
    public string Summary { get; }

    // TODO: it would be nice if this property wasn't public
    [JsonPropertyName("parent")]
    public JiraIssueParentEpic? ParentEpic { get; }

    public JiraIssueFields(string summary, JiraIssueParentEpic parentEpic)
    {
        Summary = summary;
        ParentEpic = parentEpic;
    }
}
