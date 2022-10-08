using System.Text.Json.Serialization;

namespace JiraDataFetcher.DTO.SearchResults;

public class JiraSearchResultPage
{
    [JsonPropertyName("startAt")]
    public int StartAt { get; }
    
    [JsonPropertyName("maxResults")]
    public int MaxResults { get; }
    
    [JsonPropertyName("total")]
    public int Total { get; }
    
    [JsonPropertyName("issues")]
    public List<JiraIssue> Issues { get; }

    public JiraSearchResultPage(int startAt, int maxResults, int total, List<JiraIssue> issues)
    {
        StartAt = startAt;
        MaxResults = maxResults;
        Total = total;
        Issues = issues;
    }
}
