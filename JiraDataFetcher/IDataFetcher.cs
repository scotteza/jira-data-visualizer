namespace JiraDataFetcher;

public interface IDataFetcher
{
    Task<JiraIssue?> FetchIssue(string key);
}
