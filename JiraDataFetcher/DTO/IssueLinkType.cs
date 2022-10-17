using System.Text.Json.Serialization;

namespace JiraDataFetcher.DTO;

public class IssueLinkType
{
    [JsonPropertyName("inward")]
    public string Inward { get; set; }

    [JsonPropertyName("outward")]
    public string Outward { get; set; }

    public IssueLinkType(string inward, string outward)
    {
        Inward = inward;
        Outward = outward;
    }
}
