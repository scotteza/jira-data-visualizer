using HttpWrapper;
using Moq;
using NUnit.Framework;

namespace JiraDataFetcher.Test;

internal class DataFetcherShould
{
    [Test]
    public async Task Fetch_A_Jira_Issue()
    {
        var httpGetter = new Mock<IHttpGetter>(MockBehavior.Strict);

        var httpResult = GetRealisticJiraSingleIssueHttpResponse();
        httpGetter.Setup(x => x.GetWithBasicAuthentication(
            "https://my-jira-domain.atlassian.net",
            "/rest/api/3/issue/PROJ-1234?fields=key,summary",
            "username",
            "password"))
            .Returns(Task.FromResult(httpResult));


        var dataFetcher = new DataFetcher(httpGetter.Object, "my-jira-domain", "username", "password");


        var result = await dataFetcher.FetchIssue("PROJ-1234");


        httpGetter.Verify(x => x.GetWithBasicAuthentication(
            "https://my-jira-domain.atlassian.net",
            "/rest/api/3/issue/PROJ-1234?fields=key,summary",
            "username",
            "password"), Times.Once);

        Assert.That(result.Key, Is.EqualTo("PROJ-1234"));
        Assert.That(result.Fields.Summary, Is.EqualTo("Testing 123 Task"));
    }

    [Test]
    public async Task Search_For_Jira_Issues()
    {
        var httpGetter = new Mock<IHttpGetter>(MockBehavior.Strict);
        var httpResult = GetRealisticJiraSearchHttpResponse();
        httpGetter.Setup(x => x.GetWithBasicAuthentication(
                "https://my-jira-domain.atlassian.net",
                "/rest/api/3/search",
                "username",
                "password"))
            .Returns(Task.FromResult(httpResult));


        var dataFetcher = new DataFetcher(httpGetter.Object, "my-jira-domain", "username", "password");


        var result = await dataFetcher.SearchIssues();


        httpGetter.Verify(x => x.GetWithBasicAuthentication(
            "https://my-jira-domain.atlassian.net",
            "/rest/api/3/search",
            "username",
            "password"), Times.Once);

        Assert.NotNull(result);
        var issues = result?.GetIssues();
        Assert.That(issues!.Count, Is.EqualTo(58));
    }

    private string GetRealisticJiraSingleIssueHttpResponse()
    {
        return "{\"expand\":\"renderedFields,names,schema,operations,editmeta,changelog,versionedRepresentations\",\"id\":\"10000\",\"self\":\"https://my-jira-domain.atlassian.net/rest/api/3/issue/10000\",\"key\":\"PROJ-1234\",\"fields\":{\"summary\":\"Testing 123 Task\"}}\r\n";
    }

    private string GetRealisticJiraSearchHttpResponse()
    {
        return "";
    }
}
