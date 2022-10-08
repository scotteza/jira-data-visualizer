using System.Text.Json.Serialization;

namespace JiraDataFetcher.DTO;

public class JiraIssueType
{
    [JsonPropertyName("name")]
    public string Name { get; }

    public JiraIssueType(string name)
    {
        Name = name;
    }
}
