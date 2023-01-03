namespace JiraDataFetcher.DTO;

public class DeNormalizedJiraComment
{
    public string AuthorDisplayName { get; }
    public string AuthorEmailAddress { get; }
    public DateTime CreatedDate { get; }
    public DateTime UpdatedDate { get; }
    public string Text { get; }

    public DeNormalizedJiraComment(
        string authorDisplayName,
        string authorEmailAddress,
        DateTime createdDate,
        DateTime updatedDate,
        string text)
    {
        AuthorDisplayName = authorDisplayName;
        AuthorEmailAddress = authorEmailAddress;
        CreatedDate = createdDate;
        UpdatedDate = updatedDate;
        Text = text;
    }
}
