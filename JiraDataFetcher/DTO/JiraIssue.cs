using System.Globalization;
using System.Text.Json.Serialization;

namespace JiraDataFetcher.DTO;

public class JiraIssue
{
    [JsonPropertyName("key")]
    public string Key { get; }

    [JsonPropertyName("self")]
    public string Self { get; }

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

    public JiraCommentCollection Comments => Fields?.Comments != null
                                    ? Fields.Comments
                                    : new JiraCommentCollection();

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

    public JiraIssue(string key, JiraIssueFields fields, string self)
    {
        Key = key;
        Fields = fields;
        Self = self;
    }

    public string GetFrontendUrl()
    {
        var prefix = Self.Split(@"/rest/api/3/issue").First();
        return $"{prefix}/browse/{Key}";
    }

    public List<DeNormalizedJiraComment> GetComments()
    {
        var result = new List<DeNormalizedJiraComment>();

        foreach (var comment in this.Comments.Comments)
        {
            var createdDate = DateTime.ParseExact(comment.CreatedDate, "yyyy-MM-dd'T'HH:mm:ss.fffK", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
            var updatedDate = DateTime.ParseExact(comment.UpdatedDate, "yyyy-MM-dd'T'HH:mm:ss.fffK", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);

            var deNormalizedJiraComment = new DeNormalizedJiraComment(
                comment.Author.DisplayName,
                comment.Author.EmailAddress,
                createdDate,
                updatedDate,
                // TODO: this is a textbook Law of Demeter violation
                comment.Body.Content.First().Content.First().Text);

            result.Add(deNormalizedJiraComment);
        }

        return result;
    }
}
