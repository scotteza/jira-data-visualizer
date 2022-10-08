namespace JiraDataFetcher;

public interface IDataFetcher
{
    Task<JiraIssue> FetchIssue(string key);
    Task<JiraIssueSearchResult> SearchIssues(string projectName, int resultsPerPage);
}
