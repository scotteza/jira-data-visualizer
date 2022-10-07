using System.Text.Json.Serialization;

namespace JiraDataFetcher;

public class JiraIssueFields
{
    [JsonPropertyName("summary")]
    public string Summary { get; }

    public JiraIssueFields(string summary)
    {
        Summary = summary;
    }
}
