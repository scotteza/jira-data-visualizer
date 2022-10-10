using System.Text.Json.Serialization;

namespace JiraDataFetcher.DTO;

public class JiraIssueStatus
{
    [JsonPropertyName("name")]
    public string Name { get; }

    public JiraIssueStatus(string name)
    {
        Name = name;
    }
}
