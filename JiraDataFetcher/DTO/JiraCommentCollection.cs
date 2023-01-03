using System.Text.Json.Serialization;

namespace JiraDataFetcher.DTO;

public class JiraCommentCollection
{
    [JsonPropertyName("comments")]
    public List<JiraComment> Comments { get; set; }

    public JiraCommentCollection()
    {
        Comments = new List<JiraComment>();
    }
}
