using System.Text.Json.Serialization;

namespace JiraDataFetcher.DTO;

public class JiraIssueLinks
{
    [JsonPropertyName("type")]
    public IssueLinkType Type { get; }


    [JsonPropertyName("inwardIssue")]
    public JiraIssue? InwardIssue { get; }

    public string InwardIssueKey => InwardIssue?.Key ?? string.Empty;


    [JsonPropertyName("outwardIssue")]
    public JiraIssue? OutwardIssue { get; }

    public string OutwardIssueKey => OutwardIssue?.Key ?? string.Empty;


    public JiraIssueLinks(IssueLinkType type, JiraIssue inwardIssue, JiraIssue outwardIssue)
    {
        Type = type;
        InwardIssue = inwardIssue;
        OutwardIssue = outwardIssue;
    }
}
