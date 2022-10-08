﻿using HttpWrapper;
using JiraDataFetcher.DTO;
using JiraDataFetcher.DTO.SearchResults;
using System.Text.Json;

namespace JiraDataFetcher;

public class DataFetcher : IDataFetcher
{
    private readonly IHttpGetter _httpGetter;
    private readonly string _jiraDomain;
    private readonly string _username;
    private readonly string _password;

    public DataFetcher(IHttpGetter httpGetter, string jiraDomain, string username, string password)
    {
        _httpGetter = httpGetter;
        _jiraDomain = jiraDomain;
        _username = username;
        _password = password;
    }

    public async Task<JiraIssue> FetchIssue(string key)
    {
        var response = await _httpGetter.GetWithBasicAuthentication(
            $"https://{_jiraDomain}.atlassian.net",
            $"/rest/api/3/issue/{key}?fields=key,summary,issuetype,parent",
            _username,
            _password);

        var jiraIssue = JsonSerializer.Deserialize<JiraIssue>(response);

        return jiraIssue ?? throw new InvalidOperationException("Deserialized Jira issue was null");
    }

    public async Task<JiraIssueSearchResult> SearchIssues(string projectName, int resultsPerPage)
    {
        var startAt = 0;
        var completedSearching = false;
        var issues = new List<JiraIssue>();

        while (!completedSearching)
        {
            var response = await _httpGetter.GetWithBasicAuthentication(
                $"https://{_jiraDomain}.atlassian.net",
                $"/rest/api/3/search?jql=project={projectName}+order+by+key+ASC&fields=key,summary,issuetype,parent&maxResults={resultsPerPage}&startAt={startAt}",
                _username,
                _password);

            var jiraSearchResultPage = JsonSerializer.Deserialize<JiraSearchResultPage>(response);

            if (jiraSearchResultPage == null)
            {
                throw new InvalidOperationException($"jiraSearchResultPage returned on startAt={startAt} was null");
            }
            if (jiraSearchResultPage.Issues == null)
            {
                throw new InvalidOperationException($"jiraSearchResultPage.Issues returned on startAt={startAt} was null");
            }
            if (jiraSearchResultPage.Issues.Count == 0)
            {
                throw new InvalidOperationException("jiraSearchResultPage.Issues was an empty list");
            }

            issues.AddRange(jiraSearchResultPage.Issues);

            startAt += resultsPerPage;
            completedSearching = issues.Count == jiraSearchResultPage.Total;

            if (issues.Count > jiraSearchResultPage.Total)
            {
                throw new InvalidOperationException($"issues.Count ({issues.Count}) was greater than jiraSearchResultPage.Total ({jiraSearchResultPage.Total})");
            }
        }

        return new JiraIssueSearchResult(issues.AsReadOnly());
    }
}
