using System.Text.Json.Serialization;

namespace JiraDataFetcher.DTO;

public class JiraCommentContent
{
    [JsonPropertyName("content")]
    public JiraCommentContentTextContainer[] Content { get; set; }
}
