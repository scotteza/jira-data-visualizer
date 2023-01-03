using System.Text.Json.Serialization;

namespace JiraDataFetcher.DTO;

public class JiraCommentBody
{
    [JsonPropertyName("content")]
    public List<JiraCommentContent> Content { get; set; }

}
