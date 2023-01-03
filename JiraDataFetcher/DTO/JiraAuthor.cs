using System.Text.Json.Serialization;

namespace JiraDataFetcher.DTO;

public class JiraAuthor
{
    [JsonPropertyName("emailAddress")]
    public string EmailAddress { get; set; }

    [JsonPropertyName("displayName")]
    public string DisplayName { get; set; }
}
