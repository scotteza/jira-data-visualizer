using System.Text.Json.Serialization;

namespace JiraDataFetcher.DTO;

public class JiraIssue
{
    [JsonPropertyName("key")]
    public string Key { get; }

    // TODO: it would be nice if this property wasn't public
    [JsonPropertyName("fields")]
    public JiraIssueFields? Fields { get; }

    public string Summary => Fields != null
                                ? Fields.Summary
                                : string.Empty;

    public string ParentEpicKey => Fields?.ParentEpic != null
                                    ? Fields.ParentEpic.Key
                                    : string.Empty;

    public string IssueType => Fields?.IssueType != null
                                    ? Fields.IssueType.Name
                                    : string.Empty;

    public string Status => Fields?.Status != null
                                    ? Fields.Status.Name
                                    : string.Empty;

    public IReadOnlyList<string> GetIssuesThatIBlock()
    {
        if (Fields?.IssueLinks == null)
        {
            return new List<string>().AsReadOnly();
        }

        return Fields.IssueLinks.Where(
                x => x.Type.Outward.Equals("blocks")
                && x.OutwardIssue != null
                )
            .Select(x => x.OutwardIssueKey)
            .ToList()
            .AsReadOnly();
    }

    public IReadOnlyList<string> GetIssuesThatBlockMe()
    {
        if (Fields?.IssueLinks == null)
        {
            return new List<string>().AsReadOnly();
        }

        return Fields.IssueLinks.Where(
                x => x.Type.Inward.Equals("is blocked by")
                && x.InwardIssue != null
                )
            .Select(x => x.InwardIssueKey)
            .ToList()
            .AsReadOnly();
    }

    public JiraIssue(string key, JiraIssueFields fields)
    {
        Key = key;
        Fields = fields;
    }
}
