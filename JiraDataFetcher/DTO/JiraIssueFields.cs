using System.Text.Json.Serialization;

namespace JiraDataFetcher.DTO;

public class JiraIssueFields
{
    [JsonPropertyName("summary")]
    public string Summary { get; }

    // TODO: it would be nice if this property wasn't public
    [JsonPropertyName("parent")]
    public JiraIssueParentEpic? ParentEpic { get; }

    [JsonPropertyName("issuetype")]
    public JiraIssueType IssueType { get; }

    [JsonPropertyName("status")]
    public JiraIssueStatus Status { get; }

    [JsonPropertyName("issuelinks")]
    public List<JiraIssueLinks> IssueLinks { get; }

    public JiraIssueFields(
        string summary,
        JiraIssueParentEpic parentEpic,
        JiraIssueType issueType,
        JiraIssueStatus status,
        List<JiraIssueLinks> issueLinks)
    {
        Summary = summary;
        ParentEpic = parentEpic;
        IssueType = issueType;
        Status = status;
        IssueLinks = issueLinks;
    }
}
