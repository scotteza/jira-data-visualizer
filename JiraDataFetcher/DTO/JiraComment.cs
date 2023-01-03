using System.Text.Json.Serialization;

namespace JiraDataFetcher.DTO;

public class JiraComment
{
    [JsonPropertyName("author")]
    public JiraAuthor Author { get; set; }

    [JsonPropertyName("body")]
    public JiraCommentBody Body { get; set; }

    [JsonPropertyName("created")]
    public string CreatedDate { get; set; }

    [JsonPropertyName("updated")]
    public string UpdatedDate { get; set; }
}
