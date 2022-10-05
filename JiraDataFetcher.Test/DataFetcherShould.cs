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
        var httpResult = GetRealisticJiraHttpResponse();
        httpGetter.Setup(x => x.GetWithBasicAuthentication(
            "https://my-jira-domain.atlassian.net",
            "/rest/api/3/issue/PROJ-1234",
            "username",
            "password"))
            .Returns(Task.FromResult(httpResult));


        var dataFetcher = new DataFetcher(httpGetter.Object, "my-jira-domain", "username", "password");


        var result = await dataFetcher.FetchIssue("PROJ-1234");


        httpGetter.Verify(x => x.GetWithBasicAuthentication(
            "https://my-jira-domain.atlassian.net",
            "/rest/api/3/issue/PROJ-1234",
            "username",
            "password"), Times.Once);

        Assert.NotNull(result);
        Assert.That(result?.Key, Is.EqualTo("PROJ-1234"));
    }

    private string GetRealisticJiraHttpResponse()
    {
        return @"{
    ""expand"": ""renderedFields,names,schema,operations,editmeta,changelog,versionedRepresentations,customfield_10010.requestTypePractice"",
    ""id"": ""10000"",
    ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/issue/10000"",
    ""key"": ""PROJ-1234"",
    ""fields"": {
        ""statuscategorychangedate"": ""2022-10-05T20:44:12.760+0100"",
        ""issuetype"": {
            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/issuetype/10004"",
            ""id"": ""10004"",
            ""description"": ""Tasks track small, distinct pieces of work."",
            ""iconUrl"": ""https://my-jira-domain.atlassian.net/rest/api/2/universal_avatar/view/type/issuetype/avatar/9923?size=medium"",
            ""name"": ""Task"",
            ""subtask"": false,
            ""avatarId"": 9923,
            ""entityId"": ""2c755785-f1ff-4f42-8887-f294a38f9483"",
            ""hierarchyLevel"": 0
        },
        ""timespent"": null,
        ""project"": {
            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/project/10001"",
            ""id"": ""10001"",
            ""key"": ""PROJ"",
            ""name"": ""My Project"",
            ""projectTypeKey"": ""software"",
            ""simplified"": true,
            ""avatarUrls"": {
                ""48x48"": ""https://my-jira-domain.atlassian.net/rest/api/3/universal_avatar/view/type/project/avatar/9924"",
                ""24x24"": ""https://my-jira-domain.atlassian.net/rest/api/3/universal_avatar/view/type/project/avatar/9924?size=small"",
                ""16x16"": ""https://my-jira-domain.atlassian.net/rest/api/3/universal_avatar/view/type/project/avatar/9924?size=xsmall"",
                ""32x32"": ""https://my-jira-domain.atlassian.net/rest/api/3/universal_avatar/view/type/project/avatar/9924?size=medium""
            }
        },
        ""fixVersions"": [],
        ""aggregatetimespent"": null,
        ""resolution"": null,
        ""customfield_10027"": null,
        ""customfield_10028"": null,
        ""customfield_10029"": null,
        ""resolutiondate"": null,
        ""workratio"": -1,
        ""issuerestriction"": {
            ""issuerestrictions"": {},
            ""shouldDisplay"": true
        },
        ""lastViewed"": null,
        ""watches"": {
            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/issue/PROJ-1234/watchers"",
            ""watchCount"": 1,
            ""isWatching"": true
        },
        ""created"": ""2022-10-05T20:44:12.446+0100"",
        ""customfield_10020"": null,
        ""customfield_10021"": null,
        ""customfield_10022"": null,
        ""priority"": {
            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/priority/3"",
            ""iconUrl"": ""https://my-jira-domain.atlassian.net/images/icons/priorities/medium.svg"",
            ""name"": ""Medium"",
            ""id"": ""3""
        },
        ""customfield_10023"": null,
        ""customfield_10024"": null,
        ""customfield_10025"": null,
        ""customfield_10026"": null,
        ""labels"": [],
        ""customfield_10016"": null,
        ""customfield_10017"": null,
        ""customfield_10018"": {
            ""hasEpicLinkFieldDependency"": false,
            ""showField"": false,
            ""nonEditableReason"": {
                ""reason"": ""PLUGIN_LICENSE_ERROR"",
                ""message"": ""The Parent Link is only available to Jira Premium users.""
            }
        },
        ""customfield_10019"": ""0|hzzzzz:"",
        ""timeestimate"": null,
        ""aggregatetimeoriginalestimate"": null,
        ""versions"": [],
        ""issuelinks"": [],
        ""assignee"": null,
        ""updated"": ""2022-10-05T20:44:12.446+0100"",
        ""status"": {
            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/status/10003"",
            ""description"": """",
            ""iconUrl"": ""https://my-jira-domain.atlassian.net/"",
            ""name"": ""To Do"",
            ""id"": ""10003"",
            ""statusCategory"": {
                ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/statuscategory/2"",
                ""id"": 2,
                ""key"": ""new"",
                ""colorName"": ""blue-gray"",
                ""name"": ""To Do""
            }
        },
        ""components"": [],
        ""timeoriginalestimate"": null,
        ""description"": null,
        ""customfield_10010"": null,
        ""customfield_10014"": null,
        ""timetracking"": {},
        ""customfield_10015"": null,
        ""customfield_10005"": null,
        ""customfield_10006"": null,
        ""security"": null,
        ""customfield_10007"": null,
        ""customfield_10008"": null,
        ""customfield_10009"": null,
        ""attachment"": [],
        ""aggregatetimeestimate"": null,
        ""summary"": ""Testing 123"",
        ""creator"": {
            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/user?accountId=92de3dbb-55e7-40e9-b970-8801b07a83a3"",
            ""accountId"": ""fa1632eb-f6f0-4142-b425-311c5d5cbd16"",
            ""emailAddress"": ""my-jira-domain@gmail.com"",
            ""avatarUrls"": {
                ""48x48"": ""https://avatar-management--avatars.us-west-2.prod.public.atl-paas.net/fa1632eb-f6f0-4142-b425-311c5d5cbd16/06c21b40-cbff-4a11-9ab0-e8c9d91d6557/48"",
                ""24x24"": ""https://avatar-management--avatars.us-west-2.prod.public.atl-paas.net/fa1632eb-f6f0-4142-b425-311c5d5cbd16/06c21b40-cbff-4a11-9ab0-e8c9d91d6557/24"",
                ""16x16"": ""https://avatar-management--avatars.us-west-2.prod.public.atl-paas.net/fa1632eb-f6f0-4142-b425-311c5d5cbd16/06c21b40-cbff-4a11-9ab0-e8c9d91d6557/16"",
                ""32x32"": ""https://avatar-management--avatars.us-west-2.prod.public.atl-paas.net/fa1632eb-f6f0-4142-b425-311c5d5cbd16/06c21b40-cbff-4a11-9ab0-e8c9d91d6557/32""
            },
            ""displayName"": ""Random User"",
            ""active"": true,
            ""timeZone"": ""Europe/Zurich"",
            ""accountType"": ""atlassian""
        },
        ""subtasks"": [],
        ""reporter"": {
            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/user?accountId=92de3dbb-55e7-40e9-b970-8801b07a83a3"",
            ""accountId"": ""fa1632eb-f6f0-4142-b425-311c5d5cbd16"",
            ""emailAddress"": ""my-jira-domain@gmail.com"",
            ""avatarUrls"": {
                ""48x48"": ""https://avatar-management--avatars.us-west-2.prod.public.atl-paas.net/fa1632eb-f6f0-4142-b425-311c5d5cbd16/06c21b40-cbff-4a11-9ab0-e8c9d91d6557/48"",
                ""24x24"": ""https://avatar-management--avatars.us-west-2.prod.public.atl-paas.net/fa1632eb-f6f0-4142-b425-311c5d5cbd16/06c21b40-cbff-4a11-9ab0-e8c9d91d6557/24"",
                ""16x16"": ""https://avatar-management--avatars.us-west-2.prod.public.atl-paas.net/fa1632eb-f6f0-4142-b425-311c5d5cbd16/06c21b40-cbff-4a11-9ab0-e8c9d91d6557/16"",
                ""32x32"": ""https://avatar-management--avatars.us-west-2.prod.public.atl-paas.net/fa1632eb-f6f0-4142-b425-311c5d5cbd16/06c21b40-cbff-4a11-9ab0-e8c9d91d6557/32""
            },
            ""displayName"": ""Random User"",
            ""active"": true,
            ""timeZone"": ""Europe/Zurich"",
            ""accountType"": ""atlassian""
        },
        ""aggregateprogress"": {
            ""progress"": 0,
            ""total"": 0
        },
        ""customfield_10001"": null,
        ""customfield_10002"": null,
        ""customfield_10003"": null,
        ""customfield_10004"": null,
        ""environment"": null,
        ""duedate"": null,
        ""progress"": {
            ""progress"": 0,
            ""total"": 0
        },
        ""votes"": {
            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/issue/PROJ-1234/votes"",
            ""votes"": 0,
            ""hasVoted"": false
        },
        ""comment"": {
            ""comments"": [],
            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/issue/10000/comment"",
            ""maxResults"": 0,
            ""total"": 0,
            ""startAt"": 0
        },
        ""worklog"": {
            ""startAt"": 0,
            ""maxResults"": 20,
            ""total"": 0,
            ""worklogs"": []
        }
    }
}";
    }
}
