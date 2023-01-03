using System.Text.Json.Serialization;

namespace JiraDataFetcher.DTO;

public class JiraCommentContentTextContainer
{
    [JsonPropertyName("text")]
    public string Text { get; set; }
}
