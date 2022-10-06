namespace JiraDataFetcher;

public class JiraIssueSearchResult
{
    private readonly IReadOnlyList<JiraIssue> _issues;

    public JiraIssueSearchResult(IReadOnlyList<JiraIssue> issues)
    {
        _issues = issues;
    }

    public IReadOnlyList<JiraIssue> GetIssues()
    {
        return _issues;
    }
}
