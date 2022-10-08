using HttpWrapper;
using Moq;
using NUnit.Framework;

namespace JiraDataFetcher.Test;

internal class DataFetcherShould
{
    [Test]
    public async Task Fetch_A_Jira_Issue_Without_A_Parent_Epic()
    {
        var httpGetter = new Mock<IHttpGetter>(MockBehavior.Strict);

        var httpResult = GetRealisticJiraSingleIssueWithoutEpicHttpResponse();
        httpGetter.Setup(x => x.GetWithBasicAuthentication("https://my-jira-domain.atlassian.net", "/rest/api/3/issue/PROJ-1?fields=key,summary,issuetype,parent", "username", "password")).Returns(Task.FromResult(httpResult));


        var dataFetcher = new DataFetcher(httpGetter.Object, "my-jira-domain", "username", "password");


        var result = await dataFetcher.FetchIssue("PROJ-1");


        httpGetter.Verify(x => x.GetWithBasicAuthentication(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);

        Assert.That(result.Key, Is.EqualTo("PROJ-1"));
        Assert.That(result.Summary, Is.EqualTo("Testing 123 Task Without Epic"));
        Assert.That(result.ParentEpicKey, Is.Empty);
        Assert.That(result.IssueType, Is.EqualTo("Task"));
    }

    private string GetRealisticJiraSingleIssueWithoutEpicHttpResponse()
    {
        return @"{
    ""expand"": ""renderedFields,names,schema,operations,editmeta,changelog,versionedRepresentations"",
    ""id"": ""10058"",
    ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/issue/10058"",
    ""key"": ""PROJ-1"",
    ""fields"": {
        ""summary"": ""Testing 123 Task Without Epic"",
        ""issuetype"": {
            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/issuetype/10009"",
            ""id"": ""10009"",
            ""description"": ""Tasks track small, distinct pieces of work."",
            ""iconUrl"": ""https://my-jira-domain.atlassian.net/rest/api/2/universal_avatar/view/type/issuetype/avatar/10318?size=medium"",
            ""name"": ""Task"",
            ""subtask"": false,
            ""avatarId"": 10318,
            ""entityId"": ""8ba76d8e-86a6-4e11-8df6-74e165872028"",
            ""hierarchyLevel"": 0
        }
    }
}";
    }



    [Test]
    public async Task Fetch_A_Jira_Issue_With_A_Parent_Epic()
    {
        var httpGetter = new Mock<IHttpGetter>(MockBehavior.Strict);

        var httpResult = GetRealisticJiraSingleIssueWithEpicHttpResponse();
        httpGetter.Setup(x => x.GetWithBasicAuthentication("https://my-jira-domain.atlassian.net", "/rest/api/3/issue/PROJ-1?fields=key,summary,issuetype,parent", "username", "password")).Returns(Task.FromResult(httpResult));


        var dataFetcher = new DataFetcher(httpGetter.Object, "my-jira-domain", "username", "password");


        var result = await dataFetcher.FetchIssue("PROJ-1");


        httpGetter.Verify(x => x.GetWithBasicAuthentication(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);

        Assert.That(result.Key, Is.EqualTo("PROJ-5"));
        Assert.That(result.Summary, Is.EqualTo("Task in epic"));
        Assert.That(result.ParentEpicKey, Is.EqualTo("PROJ-4"));
        Assert.That(result.IssueType, Is.EqualTo("Task"));
    }

    private string GetRealisticJiraSingleIssueWithEpicHttpResponse()
    {
        return @"{
    ""expand"": ""renderedFields,names,schema,operations,editmeta,changelog,versionedRepresentations"",
    ""id"": ""10062"",
    ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/issue/10062"",
    ""key"": ""PROJ-5"",
    ""fields"": {
        ""summary"": ""Task in epic"",
        ""issuetype"": {
            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/issuetype/10009"",
            ""id"": ""10009"",
            ""description"": ""Tasks track small, distinct pieces of work."",
            ""iconUrl"": ""https://my-jira-domain.atlassian.net/rest/api/2/universal_avatar/view/type/issuetype/avatar/10318?size=medium"",
            ""name"": ""Task"",
            ""subtask"": false,
            ""avatarId"": 10318,
            ""entityId"": ""8ba76d8e-86a6-4e11-8df6-74e165872028"",
            ""hierarchyLevel"": 0
        },
        ""parent"": {
            ""id"": ""10061"",
            ""key"": ""PROJ-4"",
            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/issue/10061"",
            ""fields"": {
                ""summary"": ""Big important epic"",
                ""status"": {
                    ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/status/10006"",
                    ""description"": """",
                    ""iconUrl"": ""https://my-jira-domain.atlassian.net/"",
                    ""name"": ""To Do"",
                    ""id"": ""10006"",
                    ""statusCategory"": {
                        ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/statuscategory/2"",
                        ""id"": 2,
                        ""key"": ""new"",
                        ""colorName"": ""blue-gray"",
                        ""name"": ""To Do""
                    }
                },
                ""priority"": {
                    ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/priority/3"",
                    ""iconUrl"": ""https://my-jira-domain.atlassian.net/images/icons/priorities/medium.svg"",
                    ""name"": ""Medium"",
                    ""id"": ""3""
                },
                ""issuetype"": {
                    ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/issuetype/10011"",
                    ""id"": ""10011"",
                    ""description"": ""Epics track collections of related bugs, stories, and tasks."",
                    ""iconUrl"": ""https://my-jira-domain.atlassian.net/rest/api/2/universal_avatar/view/type/issuetype/avatar/10307?size=medium"",
                    ""name"": ""Epic"",
                    ""subtask"": false,
                    ""avatarId"": 10307,
                    ""entityId"": ""6999ad6f-7d10-41c8-bc8a-967c377bac91"",
                    ""hierarchyLevel"": 1
                }
            }
        }
    }
}";
    }
    
    
    
    [Test]
    public async Task Search_For_Jira_Issues()
    {
        var httpGetter = new Mock<IHttpGetter>(MockBehavior.Strict);

        httpGetter.Setup(x => x.GetWithBasicAuthentication("https://my-jira-domain.atlassian.net", "/rest/api/3/search?jql=project=PROJ+order+by+key+ASC&fields=key,summary,issuetype,parent&maxResults=2&startAt=0", "username", "password")).Returns(Task.FromResult(GetRealisticJiraSearchHttpResponse_Page1()));
        httpGetter.Setup(x => x.GetWithBasicAuthentication("https://my-jira-domain.atlassian.net", "/rest/api/3/search?jql=project=PROJ+order+by+key+ASC&fields=key,summary,issuetype,parent&maxResults=2&startAt=2", "username", "password")).Returns(Task.FromResult(GetRealisticJiraSearchHttpResponse_Page2()));
        httpGetter.Setup(x => x.GetWithBasicAuthentication("https://my-jira-domain.atlassian.net", "/rest/api/3/search?jql=project=PROJ+order+by+key+ASC&fields=key,summary,issuetype,parent&maxResults=2&startAt=4", "username", "password")).Returns(Task.FromResult(GetRealisticJiraSearchHttpResponse_Page3()));
        httpGetter.Setup(x => x.GetWithBasicAuthentication("https://my-jira-domain.atlassian.net", "/rest/api/3/search?jql=project=PROJ+order+by+key+ASC&fields=key,summary,issuetype,parent&maxResults=2&startAt=6", "username", "password")).Returns(Task.FromResult(GetRealisticJiraSearchHttpResponse_Page4()));


        var dataFetcher = new DataFetcher(httpGetter.Object, "my-jira-domain", "username", "password");


        var result = await dataFetcher.SearchIssues("PROJ", 2);


        httpGetter.Verify(x => x.GetWithBasicAuthentication(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(4));

        var issues = result.GetIssues();
        Assert.That(issues!.Count, Is.EqualTo(7));

        Assert.That(issues[0].Key,Is.EqualTo("PROJ-1"));
        Assert.That(issues[0].Summary,Is.EqualTo("Testing 123 Task Without Epic"));
        Assert.That(issues[0].ParentEpicKey,Is.Empty);
        Assert.That(issues[0].IssueType,Is.EqualTo("Task"));

        Assert.That(issues[1].Key,Is.EqualTo("PROJ-2"));
        Assert.That(issues[1].Summary,Is.EqualTo("Testing 123 Bug Without Epic"));
        Assert.That(issues[1].ParentEpicKey,Is.Empty);
        Assert.That(issues[1].IssueType,Is.EqualTo("Bug"));

        Assert.That(issues[2].Key,Is.EqualTo("PROJ-3"));
        Assert.That(issues[2].Summary,Is.EqualTo("Testing 123 Story Without Epic"));
        Assert.That(issues[2].ParentEpicKey,Is.Empty);
        Assert.That(issues[2].IssueType,Is.EqualTo("Story"));

        Assert.That(issues[3].Key,Is.EqualTo("PROJ-4"));
        Assert.That(issues[3].Summary,Is.EqualTo("Big important epic"));
        Assert.That(issues[3].ParentEpicKey,Is.Empty);
        Assert.That(issues[3].IssueType,Is.EqualTo("Epic"));

        Assert.That(issues[4].Key,Is.EqualTo("PROJ-5"));
        Assert.That(issues[4].Summary,Is.EqualTo("Task in epic"));
        Assert.That(issues[4].ParentEpicKey,Is.EqualTo("PROJ-4"));
        Assert.That(issues[4].IssueType,Is.EqualTo("Task"));

        Assert.That(issues[5].Key,Is.EqualTo("PROJ-6"));
        Assert.That(issues[5].Summary,Is.EqualTo("Bug in epic"));
        Assert.That(issues[5].ParentEpicKey,Is.EqualTo("PROJ-4"));
        Assert.That(issues[5].IssueType,Is.EqualTo("Bug"));

        Assert.That(issues[6].Key,Is.EqualTo("PROJ-7"));
        Assert.That(issues[6].Summary,Is.EqualTo("Story in epic"));
        Assert.That(issues[6].ParentEpicKey, Is.EqualTo("PROJ-4"));
        Assert.That(issues[6].IssueType,Is.EqualTo("Story"));

    }

    private string GetRealisticJiraSearchHttpResponse_Page1()
    {
        return @"{
    ""expand"": ""schema,names"",
    ""startAt"": 0,
    ""maxResults"": 2,
    ""total"": 7,
    ""issues"": [
        {
            ""expand"": ""operations,versionedRepresentations,editmeta,changelog,renderedFields"",
            ""id"": ""10058"",
            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issue/10058"",
            ""key"": ""PROJ-1"",
            ""fields"": {
                ""summary"": ""Testing 123 Task Without Epic"",
                ""issuetype"": {
                    ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issuetype/10009"",
                    ""id"": ""10009"",
                    ""description"": ""Tasks track small, distinct pieces of work."",
                    ""iconUrl"": ""https://my-jira-domain.atlassian.net/rest/api/2/universal_avatar/view/type/issuetype/avatar/10318?size=medium"",
                    ""name"": ""Task"",
                    ""subtask"": false,
                    ""avatarId"": 10318,
                    ""entityId"": ""8ba76d8e-86a6-4e11-8df6-74e165872028"",
                    ""hierarchyLevel"": 0
                }
            }
        },
        {
            ""expand"": ""operations,versionedRepresentations,editmeta,changelog,renderedFields"",
            ""id"": ""10059"",
            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issue/10059"",
            ""key"": ""PROJ-2"",
            ""fields"": {
                ""summary"": ""Testing 123 Bug Without Epic"",
                ""issuetype"": {
                    ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issuetype/10013"",
                    ""id"": ""10013"",
                    ""description"": ""Bugs track problems or errors."",
                    ""iconUrl"": ""https://my-jira-domain.atlassian.net/rest/api/2/universal_avatar/view/type/issuetype/avatar/10303?size=medium"",
                    ""name"": ""Bug"",
                    ""subtask"": false,
                    ""avatarId"": 10303,
                    ""entityId"": ""0ab48f98-440d-4234-b55a-7ae3109a7180"",
                    ""hierarchyLevel"": 0
                }
            }
        }
    ]
}";
    }

    private string GetRealisticJiraSearchHttpResponse_Page2()
    {
        return @"{
    ""expand"": ""schema,names"",
    ""startAt"": 2,
    ""maxResults"": 2,
    ""total"": 7,
    ""issues"": [
        {
            ""expand"": ""operations,versionedRepresentations,editmeta,changelog,renderedFields"",
            ""id"": ""10060"",
            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issue/10060"",
            ""key"": ""PROJ-3"",
            ""fields"": {
                ""summary"": ""Testing 123 Story Without Epic"",
                ""issuetype"": {
                    ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issuetype/10014"",
                    ""id"": ""10014"",
                    ""description"": ""Stories track functionality or features expressed as user goals."",
                    ""iconUrl"": ""https://my-jira-domain.atlassian.net/rest/api/2/universal_avatar/view/type/issuetype/avatar/10315?size=medium"",
                    ""name"": ""Story"",
                    ""subtask"": false,
                    ""avatarId"": 10315,
                    ""entityId"": ""1556ac4d-515e-448d-bdf9-7ba528fc7bb3"",
                    ""hierarchyLevel"": 0
                }
            }
        },
        {
            ""expand"": ""operations,versionedRepresentations,editmeta,changelog,renderedFields"",
            ""id"": ""10061"",
            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issue/10061"",
            ""key"": ""PROJ-4"",
            ""fields"": {
                ""summary"": ""Big important epic"",
                ""issuetype"": {
                    ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issuetype/10011"",
                    ""id"": ""10011"",
                    ""description"": ""Epics track collections of related bugs, stories, and tasks."",
                    ""iconUrl"": ""https://my-jira-domain.atlassian.net/rest/api/2/universal_avatar/view/type/issuetype/avatar/10307?size=medium"",
                    ""name"": ""Epic"",
                    ""subtask"": false,
                    ""avatarId"": 10307,
                    ""entityId"": ""6999ad6f-7d10-41c8-bc8a-967c377bac91"",
                    ""hierarchyLevel"": 1
                }
            }
        }
    ]
}";
    }

    private string GetRealisticJiraSearchHttpResponse_Page3()
    {
        return @"{
    ""expand"": ""schema,names"",
    ""startAt"": 4,
    ""maxResults"": 2,
    ""total"": 7,
    ""issues"": [
        {
            ""expand"": ""operations,versionedRepresentations,editmeta,changelog,renderedFields"",
            ""id"": ""10062"",
            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issue/10062"",
            ""key"": ""PROJ-5"",
            ""fields"": {
                ""summary"": ""Task in epic"",
                ""issuetype"": {
                    ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issuetype/10009"",
                    ""id"": ""10009"",
                    ""description"": ""Tasks track small, distinct pieces of work."",
                    ""iconUrl"": ""https://my-jira-domain.atlassian.net/rest/api/2/universal_avatar/view/type/issuetype/avatar/10318?size=medium"",
                    ""name"": ""Task"",
                    ""subtask"": false,
                    ""avatarId"": 10318,
                    ""entityId"": ""8ba76d8e-86a6-4e11-8df6-74e165872028"",
                    ""hierarchyLevel"": 0
                },
                ""parent"": {
                    ""id"": ""10061"",
                    ""key"": ""PROJ-4"",
                    ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issue/10061"",
                    ""fields"": {
                        ""summary"": ""Big important epic"",
                        ""status"": {
                            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/status/10006"",
                            ""description"": """",
                            ""iconUrl"": ""https://my-jira-domain.atlassian.net/"",
                            ""name"": ""To Do"",
                            ""id"": ""10006"",
                            ""statusCategory"": {
                                ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/statuscategory/2"",
                                ""id"": 2,
                                ""key"": ""new"",
                                ""colorName"": ""blue-gray"",
                                ""name"": ""To Do""
                            }
                        },
                        ""priority"": {
                            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/priority/3"",
                            ""iconUrl"": ""https://my-jira-domain.atlassian.net/images/icons/priorities/medium.svg"",
                            ""name"": ""Medium"",
                            ""id"": ""3""
                        },
                        ""issuetype"": {
                            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issuetype/10011"",
                            ""id"": ""10011"",
                            ""description"": ""Epics track collections of related bugs, stories, and tasks."",
                            ""iconUrl"": ""https://my-jira-domain.atlassian.net/rest/api/2/universal_avatar/view/type/issuetype/avatar/10307?size=medium"",
                            ""name"": ""Epic"",
                            ""subtask"": false,
                            ""avatarId"": 10307,
                            ""entityId"": ""6999ad6f-7d10-41c8-bc8a-967c377bac91"",
                            ""hierarchyLevel"": 1
                        }
                    }
                }
            }
        },
        {
            ""expand"": ""operations,versionedRepresentations,editmeta,changelog,renderedFields"",
            ""id"": ""10063"",
            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issue/10063"",
            ""key"": ""PROJ-6"",
            ""fields"": {
                ""summary"": ""Bug in epic"",
                ""issuetype"": {
                    ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issuetype/10013"",
                    ""id"": ""10013"",
                    ""description"": ""Bugs track problems or errors."",
                    ""iconUrl"": ""https://my-jira-domain.atlassian.net/rest/api/2/universal_avatar/view/type/issuetype/avatar/10303?size=medium"",
                    ""name"": ""Bug"",
                    ""subtask"": false,
                    ""avatarId"": 10303,
                    ""entityId"": ""0ab48f98-440d-4234-b55a-7ae3109a7180"",
                    ""hierarchyLevel"": 0
                },
                ""parent"": {
                    ""id"": ""10061"",
                    ""key"": ""PROJ-4"",
                    ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issue/10061"",
                    ""fields"": {
                        ""summary"": ""Big important epic"",
                        ""status"": {
                            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/status/10006"",
                            ""description"": """",
                            ""iconUrl"": ""https://my-jira-domain.atlassian.net/"",
                            ""name"": ""To Do"",
                            ""id"": ""10006"",
                            ""statusCategory"": {
                                ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/statuscategory/2"",
                                ""id"": 2,
                                ""key"": ""new"",
                                ""colorName"": ""blue-gray"",
                                ""name"": ""To Do""
                            }
                        },
                        ""priority"": {
                            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/priority/3"",
                            ""iconUrl"": ""https://my-jira-domain.atlassian.net/images/icons/priorities/medium.svg"",
                            ""name"": ""Medium"",
                            ""id"": ""3""
                        },
                        ""issuetype"": {
                            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issuetype/10011"",
                            ""id"": ""10011"",
                            ""description"": ""Epics track collections of related bugs, stories, and tasks."",
                            ""iconUrl"": ""https://my-jira-domain.atlassian.net/rest/api/2/universal_avatar/view/type/issuetype/avatar/10307?size=medium"",
                            ""name"": ""Epic"",
                            ""subtask"": false,
                            ""avatarId"": 10307,
                            ""entityId"": ""6999ad6f-7d10-41c8-bc8a-967c377bac91"",
                            ""hierarchyLevel"": 1
                        }
                    }
                }
            }
        }
    ]
}";
    }

    private string GetRealisticJiraSearchHttpResponse_Page4()
    {
        return @"{
    ""expand"": ""names,schema"",
    ""startAt"": 6,
    ""maxResults"": 2,
    ""total"": 7,
    ""issues"": [
        {
            ""expand"": ""operations,versionedRepresentations,editmeta,changelog,renderedFields"",
            ""id"": ""10064"",
            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issue/10064"",
            ""key"": ""PROJ-7"",
            ""fields"": {
                ""summary"": ""Story in epic"",
                ""issuetype"": {
                    ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issuetype/10014"",
                    ""id"": ""10014"",
                    ""description"": ""Stories track functionality or features expressed as user goals."",
                    ""iconUrl"": ""https://my-jira-domain.atlassian.net/rest/api/2/universal_avatar/view/type/issuetype/avatar/10315?size=medium"",
                    ""name"": ""Story"",
                    ""subtask"": false,
                    ""avatarId"": 10315,
                    ""entityId"": ""1556ac4d-515e-448d-bdf9-7ba528fc7bb3"",
                    ""hierarchyLevel"": 0
                },
                ""parent"": {
                    ""id"": ""10061"",
                    ""key"": ""PROJ-4"",
                    ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issue/10061"",
                    ""fields"": {
                        ""summary"": ""Big important epic"",
                        ""status"": {
                            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/status/10006"",
                            ""description"": """",
                            ""iconUrl"": ""https://my-jira-domain.atlassian.net/"",
                            ""name"": ""To Do"",
                            ""id"": ""10006"",
                            ""statusCategory"": {
                                ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/statuscategory/2"",
                                ""id"": 2,
                                ""key"": ""new"",
                                ""colorName"": ""blue-gray"",
                                ""name"": ""To Do""
                            }
                        },
                        ""priority"": {
                            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/priority/3"",
                            ""iconUrl"": ""https://my-jira-domain.atlassian.net/images/icons/priorities/medium.svg"",
                            ""name"": ""Medium"",
                            ""id"": ""3""
                        },
                        ""issuetype"": {
                            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issuetype/10011"",
                            ""id"": ""10011"",
                            ""description"": ""Epics track collections of related bugs, stories, and tasks."",
                            ""iconUrl"": ""https://my-jira-domain.atlassian.net/rest/api/2/universal_avatar/view/type/issuetype/avatar/10307?size=medium"",
                            ""name"": ""Epic"",
                            ""subtask"": false,
                            ""avatarId"": 10307,
                            ""entityId"": ""6999ad6f-7d10-41c8-bc8a-967c377bac91"",
                            ""hierarchyLevel"": 1
                        }
                    }
                }
            }
        }
    ]
}";
    }
}
