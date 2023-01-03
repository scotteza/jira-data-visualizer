using HttpWrapper;
using Moq;
using NUnit.Framework;

namespace JiraDataFetcher.Test;

internal class DataFetcherShould
{
    [Test]
    public async Task Fetch_A_Jira_Issue_Without_A_Parent_Epic()
    {
        var httpWrapper = new Mock<IHttpWrapper>(MockBehavior.Strict);

        var httpResult = GetRealisticJiraSingleIssueWithoutEpicHttpResponse();
        httpWrapper.Setup(x => x.GetWithBasicAuthentication("https://my-jira-domain.atlassian.net", "/rest/api/3/issue/PROJ-1?fields=key,summary,issuetype,parent,issuelinks,status,comment", "username", "password")).Returns(Task.FromResult(httpResult));


        var dataFetcher = new DataFetcher(httpWrapper.Object, "my-jira-domain", "username", "password");


        var result = await dataFetcher.FetchIssue("PROJ-1");


        httpWrapper.Verify(x => x.GetWithBasicAuthentication(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);

        Assert.That(result.Key, Is.EqualTo("PROJ-1"));
        Assert.That(result.Summary, Is.EqualTo("Testing 123 Task Without Epic"));
        Assert.That(result.ParentEpicKey, Is.Empty);
        Assert.That(result.IssueType, Is.EqualTo("Task"));
        Assert.That(result.Status, Is.EqualTo("To Do"));
        Assert.That(result.GetFrontendUrl(), Is.EqualTo("https://my-jira-domain.atlassian.net/browse/PROJ-1"));
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
        },
        ""issuelinks"": [],
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
        }
    }
}";
    }



    [Test]
    public async Task Fetch_A_Jira_Issue_With_A_Parent_Epic()
    {
        var httpWrapper = new Mock<IHttpWrapper>(MockBehavior.Strict);

        var httpResult = GetRealisticJiraSingleIssueWithEpicHttpResponse();
        httpWrapper.Setup(x => x.GetWithBasicAuthentication("https://my-jira-domain.atlassian.net", "/rest/api/3/issue/PROJ-5?fields=key,summary,issuetype,parent,issuelinks,status,comment", "username", "password")).Returns(Task.FromResult(httpResult));


        var dataFetcher = new DataFetcher(httpWrapper.Object, "my-jira-domain", "username", "password");


        var result = await dataFetcher.FetchIssue("PROJ-5");


        httpWrapper.Verify(x => x.GetWithBasicAuthentication(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);

        Assert.That(result.Key, Is.EqualTo("PROJ-5"));
        Assert.That(result.Summary, Is.EqualTo("Task in epic"));
        Assert.That(result.ParentEpicKey, Is.EqualTo("PROJ-4"));
        Assert.That(result.IssueType, Is.EqualTo("Task"));
        Assert.That(result.Status, Is.EqualTo("In Progress"));
        Assert.That(result.GetFrontendUrl(), Is.EqualTo("https://my-jira-domain.atlassian.net/browse/PROJ-5"));
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
                    ""entityId"": ""0c640f1e-c9e5-4c84-acab-9824cbaad1c0"",
                    ""hierarchyLevel"": 1
                }
            }
        },
        ""issuelinks"": [],
        ""status"": {
            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/status/10007"",
            ""description"": """",
            ""iconUrl"": ""https://my-jira-domain.atlassian.net/"",
            ""name"": ""In Progress"",
            ""id"": ""10007"",
            ""statusCategory"": {
                ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/statuscategory/4"",
                ""id"": 4,
                ""key"": ""indeterminate"",
                ""colorName"": ""yellow"",
                ""name"": ""In Progress""
            }
        }
    }
}
";
    }



    [Test]
    public async Task Fetch_An_Issue_That_Blocks_Others()
    {
        var httpWrapper = new Mock<IHttpWrapper>(MockBehavior.Strict);

        var httpResult = GetRealisticJiraBlockerIssueHttpResponse();
        httpWrapper.Setup(x => x.GetWithBasicAuthentication("https://my-jira-domain.atlassian.net", "/rest/api/3/issue/PROJ-9?fields=key,summary,issuetype,parent,issuelinks,status,comment", "username", "password")).Returns(Task.FromResult(httpResult));


        var dataFetcher = new DataFetcher(httpWrapper.Object, "my-jira-domain", "username", "password");


        var result = await dataFetcher.FetchIssue("PROJ-9");


        httpWrapper.Verify(x => x.GetWithBasicAuthentication(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);

        Assert.That(result.Key, Is.EqualTo("PROJ-9"));
        var blockedIssues = result.GetIssuesThatIBlock();
        Assert.That(blockedIssues, Has.Count.EqualTo(2));
        Assert.That(blockedIssues[0], Is.EqualTo("PROJ-8"));
        Assert.That(blockedIssues[1], Is.EqualTo("PROJ-10"));
    }

    private string GetRealisticJiraBlockerIssueHttpResponse()
    {
        return @"{
    ""expand"": ""renderedFields,names,schema,operations,editmeta,changelog,versionedRepresentations"",
    ""id"": ""10066"",
    ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/issue/10066"",
    ""key"": ""PROJ-9"",
    ""fields"": {
        ""summary"": ""I am a blocker"",
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
        ""issuelinks"": [
            {
                ""id"": ""10005"",
                ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/issueLink/10005"",
                ""type"": {
                    ""id"": ""10000"",
                    ""name"": ""Blocks"",
                    ""inward"": ""is blocked by"",
                    ""outward"": ""blocks"",
                    ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/issueLinkType/10000""
                },
                ""outwardIssue"": {
                    ""id"": ""10065"",
                    ""key"": ""PROJ-8"",
                    ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/issue/10065"",
                    ""fields"": {
                        ""summary"": ""I am blocked"",
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
                }
            },
            {
                ""id"": ""10006"",
                ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/issueLink/10006"",
                ""type"": {
                    ""id"": ""10000"",
                    ""name"": ""Blocks"",
                    ""inward"": ""is blocked by"",
                    ""outward"": ""blocks"",
                    ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/issueLinkType/10000""
                },
                ""outwardIssue"": {
                    ""id"": ""10067"",
                    ""key"": ""PROJ-10"",
                    ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/issue/10067"",
                    ""fields"": {
                        ""summary"": ""A random story"",
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
                            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/issuetype/10014"",
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
                }
            },
            {
                ""id"": ""10009"",
                ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/issueLink/10009"",
                ""type"": {
                    ""id"": ""10000"",
                    ""name"": ""Blocks"",
                    ""inward"": ""is blocked by"",
                    ""outward"": ""blocks"",
                    ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/issueLinkType/10000""
                },
                ""inwardIssue"": {
                    ""id"": ""10069"",
                    ""key"": ""PROJ-12"",
                    ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/issue/10069"",
                    ""fields"": {
                        ""summary"": ""Misc issue ABC"",
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
                            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/issuetype/10014"",
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
                }
            }
        ],
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
        }
    }
}";
    }




    [Test]
    public async Task Fetch_A_Blocked_Issue()
    {
        var httpWrapper = new Mock<IHttpWrapper>(MockBehavior.Strict);

        var httpResult = GetRealisticJiraBlockedIssueHttpResponse();
        httpWrapper.Setup(x => x.GetWithBasicAuthentication("https://my-jira-domain.atlassian.net", "/rest/api/3/issue/PROJ-9?fields=key,summary,issuetype,parent,issuelinks,status,comment", "username", "password")).Returns(Task.FromResult(httpResult));


        var dataFetcher = new DataFetcher(httpWrapper.Object, "my-jira-domain", "username", "password");


        var result = await dataFetcher.FetchIssue("PROJ-9");


        httpWrapper.Verify(x => x.GetWithBasicAuthentication(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);

        Assert.That(result.Key, Is.EqualTo("PROJ-8"));
        var blockedIssues = result.GetIssuesThatBlockMe();
        Assert.That(blockedIssues, Has.Count.EqualTo(3));
        Assert.That(blockedIssues[0], Is.EqualTo("PROJ-9"));
        Assert.That(blockedIssues[1], Is.EqualTo("PROJ-10"));
        Assert.That(blockedIssues[2], Is.EqualTo("PROJ-11"));
    }

    private string GetRealisticJiraBlockedIssueHttpResponse()
    {
        return @"{
    ""expand"": ""renderedFields,names,schema,operations,editmeta,changelog,versionedRepresentations"",
    ""id"": ""10065"",
    ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/issue/10065"",
    ""key"": ""PROJ-8"",
    ""fields"": {
        ""summary"": ""I am blocked"",
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
        ""issuelinks"": [
            {
                ""id"": ""10010"",
                ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/issueLink/10010"",
                ""type"": {
                    ""id"": ""10000"",
                    ""name"": ""Blocks"",
                    ""inward"": ""is blocked by"",
                    ""outward"": ""blocks"",
                    ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/issueLinkType/10000""
                },
                ""outwardIssue"": {
                    ""id"": ""10069"",
                    ""key"": ""PROJ-12"",
                    ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/issue/10069"",
                    ""fields"": {
                        ""summary"": ""Misc issue ABC"",
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
                            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/issuetype/10014"",
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
                }
            },
            {
                ""id"": ""10005"",
                ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/issueLink/10005"",
                ""type"": {
                    ""id"": ""10000"",
                    ""name"": ""Blocks"",
                    ""inward"": ""is blocked by"",
                    ""outward"": ""blocks"",
                    ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/issueLinkType/10000""
                },
                ""inwardIssue"": {
                    ""id"": ""10066"",
                    ""key"": ""PROJ-9"",
                    ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/issue/10066"",
                    ""fields"": {
                        ""summary"": ""I am a blocker"",
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
                }
            },
            {
                ""id"": ""10007"",
                ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/issueLink/10007"",
                ""type"": {
                    ""id"": ""10000"",
                    ""name"": ""Blocks"",
                    ""inward"": ""is blocked by"",
                    ""outward"": ""blocks"",
                    ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/issueLinkType/10000""
                },
                ""inwardIssue"": {
                    ""id"": ""10067"",
                    ""key"": ""PROJ-10"",
                    ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/issue/10067"",
                    ""fields"": {
                        ""summary"": ""A random story"",
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
                            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/issuetype/10014"",
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
                }
            },
            {
                ""id"": ""10008"",
                ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/issueLink/10008"",
                ""type"": {
                    ""id"": ""10000"",
                    ""name"": ""Blocks"",
                    ""inward"": ""is blocked by"",
                    ""outward"": ""blocks"",
                    ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/issueLinkType/10000""
                },
                ""inwardIssue"": {
                    ""id"": ""10068"",
                    ""key"": ""PROJ-11"",
                    ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/issue/10068"",
                    ""fields"": {
                        ""summary"": ""Another random story"",
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
                            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/issuetype/10014"",
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
                }
            }
        ],
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
        }
    }
}";
    }




    // TODO: add links and status checks to tickets
    [Test]
    public async Task Search_For_Jira_Issues()
    {
        var httpWrapper = new Mock<IHttpWrapper>(MockBehavior.Strict);

        httpWrapper.Setup(x => x.PostWithBasicAuthentication("https://my-jira-domain.atlassian.net", "/rest/api/3/search", "{\"jql\":\"project = PROJ\",\"maxResults\":2,\"startAt\":0,\"fieldsByKeys\":false,\"fields\":[\"key\",\"summary\",\"issuetype\",\"parent\",\"issuelinks\",\"status\"]}", "username", "password")).Returns(Task.FromResult(GetRealisticJiraSearchHttpResponse_Page1()));
        httpWrapper.Setup(x => x.PostWithBasicAuthentication("https://my-jira-domain.atlassian.net", "/rest/api/3/search", "{\"jql\":\"project = PROJ\",\"maxResults\":2,\"startAt\":2,\"fieldsByKeys\":false,\"fields\":[\"key\",\"summary\",\"issuetype\",\"parent\",\"issuelinks\",\"status\"]}", "username", "password")).Returns(Task.FromResult(GetRealisticJiraSearchHttpResponse_Page2()));
        httpWrapper.Setup(x => x.PostWithBasicAuthentication("https://my-jira-domain.atlassian.net", "/rest/api/3/search", "{\"jql\":\"project = PROJ\",\"maxResults\":2,\"startAt\":4,\"fieldsByKeys\":false,\"fields\":[\"key\",\"summary\",\"issuetype\",\"parent\",\"issuelinks\",\"status\"]}", "username", "password")).Returns(Task.FromResult(GetRealisticJiraSearchHttpResponse_Page3()));
        httpWrapper.Setup(x => x.PostWithBasicAuthentication("https://my-jira-domain.atlassian.net", "/rest/api/3/search", "{\"jql\":\"project = PROJ\",\"maxResults\":2,\"startAt\":6,\"fieldsByKeys\":false,\"fields\":[\"key\",\"summary\",\"issuetype\",\"parent\",\"issuelinks\",\"status\"]}", "username", "password")).Returns(Task.FromResult(GetRealisticJiraSearchHttpResponse_Page4()));
        httpWrapper.Setup(x => x.PostWithBasicAuthentication("https://my-jira-domain.atlassian.net", "/rest/api/3/search", "{\"jql\":\"project = PROJ\",\"maxResults\":2,\"startAt\":8,\"fieldsByKeys\":false,\"fields\":[\"key\",\"summary\",\"issuetype\",\"parent\",\"issuelinks\",\"status\"]}", "username", "password")).Returns(Task.FromResult(GetRealisticJiraSearchHttpResponse_Page5()));
        httpWrapper.Setup(x => x.PostWithBasicAuthentication("https://my-jira-domain.atlassian.net", "/rest/api/3/search", "{\"jql\":\"project = PROJ\",\"maxResults\":2,\"startAt\":10,\"fieldsByKeys\":false,\"fields\":[\"key\",\"summary\",\"issuetype\",\"parent\",\"issuelinks\",\"status\"]}", "username", "password")).Returns(Task.FromResult(GetRealisticJiraSearchHttpResponse_Page6()));


        var dataFetcher = new DataFetcher(httpWrapper.Object, "my-jira-domain", "username", "password");


        var result = await dataFetcher.SearchIssues("project = PROJ", 2);


        httpWrapper.Verify(x => x.PostWithBasicAuthentication(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(6));

        var issues = result.GetIssues();
        Assert.That(issues.Count, Is.EqualTo(12));

        Assert.That(issues[0].Key, Is.EqualTo("PROJ-1"));
        Assert.That(issues[0].Summary, Is.EqualTo("Testing 123 Task Without Epic"));
        Assert.That(issues[0].ParentEpicKey, Is.Empty);
        Assert.That(issues[0].IssueType, Is.EqualTo("Task"));
        Assert.That(issues[0].Status, Is.EqualTo("To Do"));
        Assert.That(issues[0].GetIssuesThatIBlock(), Is.Empty);
        Assert.That(issues[0].GetIssuesThatBlockMe(), Is.Empty);

        Assert.That(issues[1].Key, Is.EqualTo("PROJ-2"));
        Assert.That(issues[1].Summary, Is.EqualTo("Testing 123 Bug Without Epic"));
        Assert.That(issues[1].ParentEpicKey, Is.Empty);
        Assert.That(issues[1].IssueType, Is.EqualTo("Bug"));
        Assert.That(issues[1].Status, Is.EqualTo("To Do"));
        Assert.That(issues[1].GetIssuesThatIBlock(), Is.Empty);
        Assert.That(issues[1].GetIssuesThatBlockMe(), Is.Empty);

        Assert.That(issues[2].Key, Is.EqualTo("PROJ-3"));
        Assert.That(issues[2].Summary, Is.EqualTo("Testing 123 Story Without Epic"));
        Assert.That(issues[2].ParentEpicKey, Is.Empty);
        Assert.That(issues[2].IssueType, Is.EqualTo("Story"));
        Assert.That(issues[2].Status, Is.EqualTo("To Do"));
        Assert.That(issues[2].GetIssuesThatIBlock(), Is.Empty);
        Assert.That(issues[2].GetIssuesThatBlockMe(), Is.Empty);

        Assert.That(issues[3].Key, Is.EqualTo("PROJ-4"));
        Assert.That(issues[3].Summary, Is.EqualTo("Big important epic"));
        Assert.That(issues[3].ParentEpicKey, Is.Empty);
        Assert.That(issues[3].IssueType, Is.EqualTo("Epic"));
        Assert.That(issues[3].Status, Is.EqualTo("To Do"));
        Assert.That(issues[3].GetIssuesThatIBlock(), Is.Empty);
        Assert.That(issues[3].GetIssuesThatBlockMe(), Is.Empty);

        Assert.That(issues[4].Key, Is.EqualTo("PROJ-5"));
        Assert.That(issues[4].Summary, Is.EqualTo("Task in epic"));
        Assert.That(issues[4].ParentEpicKey, Is.EqualTo("PROJ-4"));
        Assert.That(issues[4].IssueType, Is.EqualTo("Task"));
        Assert.That(issues[4].Status, Is.EqualTo("In Progress"));
        Assert.That(issues[4].GetIssuesThatIBlock(), Is.Empty);
        Assert.That(issues[4].GetIssuesThatBlockMe(), Is.Empty);

        Assert.That(issues[5].Key, Is.EqualTo("PROJ-6"));
        Assert.That(issues[5].Summary, Is.EqualTo("Bug in epic"));
        Assert.That(issues[5].ParentEpicKey, Is.EqualTo("PROJ-4"));
        Assert.That(issues[5].IssueType, Is.EqualTo("Bug"));
        Assert.That(issues[5].Status, Is.EqualTo("To Do"));
        Assert.That(issues[5].GetIssuesThatIBlock(), Is.Empty);
        Assert.That(issues[5].GetIssuesThatBlockMe(), Is.Empty);

        Assert.That(issues[6].Key, Is.EqualTo("PROJ-7"));
        Assert.That(issues[6].Summary, Is.EqualTo("Story in epic"));
        Assert.That(issues[6].ParentEpicKey, Is.EqualTo("PROJ-4"));
        Assert.That(issues[6].IssueType, Is.EqualTo("Story"));
        Assert.That(issues[6].Status, Is.EqualTo("To Do"));
        Assert.That(issues[6].GetIssuesThatIBlock(), Is.Empty);
        Assert.That(issues[6].GetIssuesThatBlockMe(), Is.Empty);

        Assert.That(issues[7].Key, Is.EqualTo("PROJ-8"));
        Assert.That(issues[7].Summary, Is.EqualTo("I am blocked"));
        Assert.That(issues[7].ParentEpicKey, Is.Empty);
        Assert.That(issues[7].IssueType, Is.EqualTo("Task"));
        Assert.That(issues[7].Status, Is.EqualTo("To Do"));
        Assert.That(issues[7].GetIssuesThatIBlock(), Has.Count.EqualTo(1));
        Assert.That(issues[7].GetIssuesThatIBlock()[0], Is.EqualTo("PROJ-12"));
        Assert.That(issues[7].GetIssuesThatBlockMe(), Has.Count.EqualTo(3));
        Assert.That(issues[7].GetIssuesThatBlockMe()[0], Is.EqualTo("PROJ-9"));
        Assert.That(issues[7].GetIssuesThatBlockMe()[1], Is.EqualTo("PROJ-10"));
        Assert.That(issues[7].GetIssuesThatBlockMe()[2], Is.EqualTo("PROJ-11"));

        Assert.That(issues[8].Key, Is.EqualTo("PROJ-9"));
        Assert.That(issues[8].Summary, Is.EqualTo("I am a blocker"));
        Assert.That(issues[8].ParentEpicKey, Is.Empty);
        Assert.That(issues[8].IssueType, Is.EqualTo("Task"));
        Assert.That(issues[8].Status, Is.EqualTo("To Do"));
        Assert.That(issues[8].GetIssuesThatIBlock(), Has.Count.EqualTo(2));
        Assert.That(issues[8].GetIssuesThatIBlock()[0], Is.EqualTo("PROJ-8"));
        Assert.That(issues[8].GetIssuesThatIBlock()[1], Is.EqualTo("PROJ-10"));
        Assert.That(issues[8].GetIssuesThatBlockMe(), Has.Count.EqualTo(1));
        Assert.That(issues[8].GetIssuesThatBlockMe()[0], Is.EqualTo("PROJ-12"));

        Assert.That(issues[9].Key, Is.EqualTo("PROJ-10"));
        Assert.That(issues[9].Summary, Is.EqualTo("A random story"));
        Assert.That(issues[9].ParentEpicKey, Is.Empty);
        Assert.That(issues[9].IssueType, Is.EqualTo("Story"));
        Assert.That(issues[9].Status, Is.EqualTo("To Do"));
        Assert.That(issues[9].GetIssuesThatIBlock(), Has.Count.EqualTo(1));
        Assert.That(issues[9].GetIssuesThatIBlock()[0], Is.EqualTo("PROJ-8"));
        Assert.That(issues[9].GetIssuesThatBlockMe(), Has.Count.EqualTo(1));
        Assert.That(issues[9].GetIssuesThatBlockMe()[0], Is.EqualTo("PROJ-9"));

        Assert.That(issues[10].Key, Is.EqualTo("PROJ-11"));
        Assert.That(issues[10].Summary, Is.EqualTo("Another random story"));
        Assert.That(issues[10].ParentEpicKey, Is.Empty);
        Assert.That(issues[10].IssueType, Is.EqualTo("Story"));
        Assert.That(issues[10].Status, Is.EqualTo("To Do"));
        Assert.That(issues[10].GetIssuesThatIBlock(), Has.Count.EqualTo(1));
        Assert.That(issues[10].GetIssuesThatIBlock()[0], Is.EqualTo("PROJ-8"));
        Assert.That(issues[10].GetIssuesThatBlockMe(), Is.Empty);

        Assert.That(issues[11].Key, Is.EqualTo("PROJ-12"));
        Assert.That(issues[11].Summary, Is.EqualTo("Misc issue ABC"));
        Assert.That(issues[11].ParentEpicKey, Is.Empty);
        Assert.That(issues[11].IssueType, Is.EqualTo("Story"));
        Assert.That(issues[11].Status, Is.EqualTo("Done"));
        Assert.That(issues[11].GetIssuesThatIBlock(), Has.Count.EqualTo(1));
        Assert.That(issues[11].GetIssuesThatIBlock()[0], Is.EqualTo("PROJ-9"));
        Assert.That(issues[11].GetIssuesThatBlockMe(), Has.Count.EqualTo(1));
        Assert.That(issues[11].GetIssuesThatBlockMe()[0], Is.EqualTo("PROJ-8"));

    }

    private string GetRealisticJiraSearchHttpResponse_Page1()
    {
        return @"{
    ""expand"": ""schema,names"",
    ""startAt"": 0,
    ""maxResults"": 2,
    ""total"": 12,
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
                },
                ""issuelinks"": [],
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
                    ""entityId"": ""e26d7d8b-4827-4461-b3ac-e0b37612dcb3"",
                    ""hierarchyLevel"": 0
                },
                ""issuelinks"": [],
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
                }
            }
        }
    ]
}
";
    }

    private string GetRealisticJiraSearchHttpResponse_Page2()
    {
        return @"{
    ""expand"": ""schema,names"",
    ""startAt"": 2,
    ""maxResults"": 2,
    ""total"": 12,
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
                },
                ""issuelinks"": [],
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
                    ""entityId"": ""0c640f1e-c9e5-4c84-acab-9824cbaad1c0"",
                    ""hierarchyLevel"": 1
                },
                ""issuelinks"": [],
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
                }
            }
        }
    ]
}
";
    }

    private string GetRealisticJiraSearchHttpResponse_Page3()
    {
        return @"{
    ""expand"": ""schema,names"",
    ""startAt"": 4,
    ""maxResults"": 2,
    ""total"": 12,
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
                            ""entityId"": ""0c640f1e-c9e5-4c84-acab-9824cbaad1c0"",
                            ""hierarchyLevel"": 1
                        }
                    }
                },
                ""issuelinks"": [],
                ""status"": {
                    ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/status/10007"",
                    ""description"": """",
                    ""iconUrl"": ""https://my-jira-domain.atlassian.net/"",
                    ""name"": ""In Progress"",
                    ""id"": ""10007"",
                    ""statusCategory"": {
                        ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/statuscategory/4"",
                        ""id"": 4,
                        ""key"": ""indeterminate"",
                        ""colorName"": ""yellow"",
                        ""name"": ""In Progress""
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
                    ""entityId"": ""e26d7d8b-4827-4461-b3ac-e0b37612dcb3"",
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
                            ""entityId"": ""0c640f1e-c9e5-4c84-acab-9824cbaad1c0"",
                            ""hierarchyLevel"": 1
                        }
                    }
                },
                ""issuelinks"": [],
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
                }
            }
        }
    ]
}
";
    }

    private string GetRealisticJiraSearchHttpResponse_Page4()
    {
        return @"{
    ""expand"": ""schema,names"",
    ""startAt"": 6,
    ""maxResults"": 2,
    ""total"": 12,
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
                            ""entityId"": ""0c640f1e-c9e5-4c84-acab-9824cbaad1c0"",
                            ""hierarchyLevel"": 1
                        }
                    }
                },
                ""issuelinks"": [],
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
                }
            }
        },
        {
            ""expand"": ""operations,versionedRepresentations,editmeta,changelog,renderedFields"",
            ""id"": ""10065"",
            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issue/10065"",
            ""key"": ""PROJ-8"",
            ""fields"": {
                ""summary"": ""I am blocked"",
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
                ""issuelinks"": [
                    {
                        ""id"": ""10010"",
                        ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issueLink/10010"",
                        ""type"": {
                            ""id"": ""10000"",
                            ""name"": ""Blocks"",
                            ""inward"": ""is blocked by"",
                            ""outward"": ""blocks"",
                            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issueLinkType/10000""
                        },
                        ""outwardIssue"": {
                            ""id"": ""10069"",
                            ""key"": ""PROJ-12"",
                            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issue/10069"",
                            ""fields"": {
                                ""summary"": ""Misc issue ABC"",
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
                        }
                    },
                    {
                        ""id"": ""10005"",
                        ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issueLink/10005"",
                        ""type"": {
                            ""id"": ""10000"",
                            ""name"": ""Blocks"",
                            ""inward"": ""is blocked by"",
                            ""outward"": ""blocks"",
                            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issueLinkType/10000""
                        },
                        ""inwardIssue"": {
                            ""id"": ""10066"",
                            ""key"": ""PROJ-9"",
                            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issue/10066"",
                            ""fields"": {
                                ""summary"": ""I am a blocker"",
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
                        }
                    },
                    {
                        ""id"": ""10007"",
                        ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issueLink/10007"",
                        ""type"": {
                            ""id"": ""10000"",
                            ""name"": ""Blocks"",
                            ""inward"": ""is blocked by"",
                            ""outward"": ""blocks"",
                            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issueLinkType/10000""
                        },
                        ""inwardIssue"": {
                            ""id"": ""10067"",
                            ""key"": ""PROJ-10"",
                            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issue/10067"",
                            ""fields"": {
                                ""summary"": ""A random story"",
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
                        }
                    },
                    {
                        ""id"": ""10008"",
                        ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issueLink/10008"",
                        ""type"": {
                            ""id"": ""10000"",
                            ""name"": ""Blocks"",
                            ""inward"": ""is blocked by"",
                            ""outward"": ""blocks"",
                            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issueLinkType/10000""
                        },
                        ""inwardIssue"": {
                            ""id"": ""10068"",
                            ""key"": ""PROJ-11"",
                            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issue/10068"",
                            ""fields"": {
                                ""summary"": ""Another random story"",
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
                        }
                    }
                ],
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
                }
            }
        }
    ]
}
";
    }

    private string GetRealisticJiraSearchHttpResponse_Page5()
    {
        return @"{
    ""expand"": ""schema,names"",
    ""startAt"": 8,
    ""maxResults"": 2,
    ""total"": 12,
    ""issues"": [
        {
            ""expand"": ""operations,versionedRepresentations,editmeta,changelog,renderedFields"",
            ""id"": ""10066"",
            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issue/10066"",
            ""key"": ""PROJ-9"",
            ""fields"": {
                ""summary"": ""I am a blocker"",
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
                ""issuelinks"": [
                    {
                        ""id"": ""10005"",
                        ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issueLink/10005"",
                        ""type"": {
                            ""id"": ""10000"",
                            ""name"": ""Blocks"",
                            ""inward"": ""is blocked by"",
                            ""outward"": ""blocks"",
                            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issueLinkType/10000""
                        },
                        ""outwardIssue"": {
                            ""id"": ""10065"",
                            ""key"": ""PROJ-8"",
                            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issue/10065"",
                            ""fields"": {
                                ""summary"": ""I am blocked"",
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
                        }
                    },
                    {
                        ""id"": ""10006"",
                        ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issueLink/10006"",
                        ""type"": {
                            ""id"": ""10000"",
                            ""name"": ""Blocks"",
                            ""inward"": ""is blocked by"",
                            ""outward"": ""blocks"",
                            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issueLinkType/10000""
                        },
                        ""outwardIssue"": {
                            ""id"": ""10067"",
                            ""key"": ""PROJ-10"",
                            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issue/10067"",
                            ""fields"": {
                                ""summary"": ""A random story"",
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
                        }
                    },
                    {
                        ""id"": ""10009"",
                        ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issueLink/10009"",
                        ""type"": {
                            ""id"": ""10000"",
                            ""name"": ""Blocks"",
                            ""inward"": ""is blocked by"",
                            ""outward"": ""blocks"",
                            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issueLinkType/10000""
                        },
                        ""inwardIssue"": {
                            ""id"": ""10069"",
                            ""key"": ""PROJ-12"",
                            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issue/10069"",
                            ""fields"": {
                                ""summary"": ""Misc issue ABC"",
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
                        }
                    }
                ],
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
                }
            }
        },
        {
            ""expand"": ""operations,versionedRepresentations,editmeta,changelog,renderedFields"",
            ""id"": ""10067"",
            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issue/10067"",
            ""key"": ""PROJ-10"",
            ""fields"": {
                ""summary"": ""A random story"",
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
                ""issuelinks"": [
                    {
                        ""id"": ""10007"",
                        ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issueLink/10007"",
                        ""type"": {
                            ""id"": ""10000"",
                            ""name"": ""Blocks"",
                            ""inward"": ""is blocked by"",
                            ""outward"": ""blocks"",
                            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issueLinkType/10000""
                        },
                        ""outwardIssue"": {
                            ""id"": ""10065"",
                            ""key"": ""PROJ-8"",
                            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issue/10065"",
                            ""fields"": {
                                ""summary"": ""I am blocked"",
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
                        }
                    },
                    {
                        ""id"": ""10006"",
                        ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issueLink/10006"",
                        ""type"": {
                            ""id"": ""10000"",
                            ""name"": ""Blocks"",
                            ""inward"": ""is blocked by"",
                            ""outward"": ""blocks"",
                            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issueLinkType/10000""
                        },
                        ""inwardIssue"": {
                            ""id"": ""10066"",
                            ""key"": ""PROJ-9"",
                            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issue/10066"",
                            ""fields"": {
                                ""summary"": ""I am a blocker"",
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
                        }
                    }
                ],
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
                }
            }
        }
    ]
}
";
    }

    private string GetRealisticJiraSearchHttpResponse_Page6()
    {
        return @"{
    ""expand"": ""schema,names"",
    ""startAt"": 10,
    ""maxResults"": 2,
    ""total"": 12,
    ""issues"": [
        {
            ""expand"": ""operations,versionedRepresentations,editmeta,changelog,renderedFields"",
            ""id"": ""10068"",
            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issue/10068"",
            ""key"": ""PROJ-11"",
            ""fields"": {
                ""summary"": ""Another random story"",
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
                ""issuelinks"": [
                    {
                        ""id"": ""10008"",
                        ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issueLink/10008"",
                        ""type"": {
                            ""id"": ""10000"",
                            ""name"": ""Blocks"",
                            ""inward"": ""is blocked by"",
                            ""outward"": ""blocks"",
                            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issueLinkType/10000""
                        },
                        ""outwardIssue"": {
                            ""id"": ""10065"",
                            ""key"": ""PROJ-8"",
                            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issue/10065"",
                            ""fields"": {
                                ""summary"": ""I am blocked"",
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
                        }
                    }
                ],
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
                }
            }
        },
        {
            ""expand"": ""operations,versionedRepresentations,editmeta,changelog,renderedFields"",
            ""id"": ""10069"",
            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issue/10069"",
            ""key"": ""PROJ-12"",
            ""fields"": {
                ""summary"": ""Misc issue ABC"",
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
                ""issuelinks"": [
                    {
                        ""id"": ""10009"",
                        ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issueLink/10009"",
                        ""type"": {
                            ""id"": ""10000"",
                            ""name"": ""Blocks"",
                            ""inward"": ""is blocked by"",
                            ""outward"": ""blocks"",
                            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issueLinkType/10000""
                        },
                        ""outwardIssue"": {
                            ""id"": ""10066"",
                            ""key"": ""PROJ-9"",
                            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issue/10066"",
                            ""fields"": {
                                ""summary"": ""I am a blocker"",
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
                        }
                    },
                    {
                        ""id"": ""10010"",
                        ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issueLink/10010"",
                        ""type"": {
                            ""id"": ""10000"",
                            ""name"": ""Blocks"",
                            ""inward"": ""is blocked by"",
                            ""outward"": ""blocks"",
                            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issueLinkType/10000""
                        },
                        ""inwardIssue"": {
                            ""id"": ""10065"",
                            ""key"": ""PROJ-8"",
                            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/issue/10065"",
                            ""fields"": {
                                ""summary"": ""I am blocked"",
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
                        }
                    }
                ],
                ""status"": {
                    ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/status/10008"",
                    ""description"": """",
                    ""iconUrl"": ""https://my-jira-domain.atlassian.net/"",
                    ""name"": ""Done"",
                    ""id"": ""10008"",
                    ""statusCategory"": {
                        ""self"": ""https://my-jira-domain.atlassian.net/rest/api/2/statuscategory/3"",
                        ""id"": 3,
                        ""key"": ""done"",
                        ""colorName"": ""green"",
                        ""name"": ""Done""
                    }
                }
            }
        }
    ]
}
";
    }




    [Test]
    public async Task Fetch_Comments()
    {
        var httpWrapper = new Mock<IHttpWrapper>(MockBehavior.Strict);

        var httpResult = GetRealisticJiraIssueWithCommentsHttpResponse();
        httpWrapper.Setup(x => x.GetWithBasicAuthentication("https://my-jira-domain.atlassian.net", "/rest/api/3/issue/PROJ-9?fields=key,summary,issuetype,parent,issuelinks,status,comment", "username", "password")).Returns(Task.FromResult(httpResult));


        var dataFetcher = new DataFetcher(httpWrapper.Object, "my-jira-domain", "username", "password");


        var result = await dataFetcher.FetchIssue("PROJ-9");


        httpWrapper.Verify(x => x.GetWithBasicAuthentication(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);

        Assert.That(result.Key, Is.EqualTo("PROJ-6"));

        var comments = result.GetComments();

        Assert.That(comments.Count, Is.EqualTo(3));

        Assert.That(comments[0].Text, Is.EqualTo("Comment 1"));
        Assert.That(comments[0].CreatedDate, Is.EqualTo(new DateTime(2022, 12, 30, 22, 15, 49, 070)));
        Assert.That(comments[0].UpdatedDate, Is.EqualTo(new DateTime(2022, 12, 30, 22, 15, 50, 070)));
        Assert.That(comments[0].AuthorDisplayName, Is.EqualTo("Bob Smith"));
        Assert.That(comments[0].AuthorEmailAddress, Is.EqualTo("email_address@gmail.com"));

        Assert.That(comments[1].Text, Is.EqualTo("Comment 2"));
        Assert.That(comments[1].CreatedDate, Is.EqualTo(new DateTime(2022, 12, 30, 22, 15, 51, 649)));
        Assert.That(comments[1].UpdatedDate, Is.EqualTo(new DateTime(2022, 12, 30, 22, 15, 51, 649)));
        Assert.That(comments[1].AuthorDisplayName, Is.EqualTo("Bob Smith 2"));
        Assert.That(comments[1].AuthorEmailAddress, Is.EqualTo("email_address_2@gmail.com"));

        Assert.That(comments[2].Text, Is.EqualTo("Comment 3"));
        Assert.That(comments[2].CreatedDate, Is.EqualTo(new DateTime(2022, 12, 30, 22, 15, 56, 744)));
        Assert.That(comments[2].UpdatedDate, Is.EqualTo(new DateTime(2022, 12, 30, 22, 15, 56, 744)));
        Assert.That(comments[2].AuthorDisplayName, Is.EqualTo("Bob Smith 3"));
        Assert.That(comments[2].AuthorEmailAddress, Is.EqualTo("email_address_3@gmail.com"));
    }

    private string GetRealisticJiraIssueWithCommentsHttpResponse()
    {
        return @"{
    ""expand"": ""renderedFields,names,schema,operations,editmeta,changelog,versionedRepresentations"",
    ""id"": ""10063"",
    ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/issue/10063"",
    ""key"": ""PROJ-6"",
    ""fields"": {
        ""summary"": ""Bug in epic"",
        ""issuetype"": {
            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/issuetype/10013"",
            ""id"": ""10013"",
            ""description"": ""Bugs track problems or errors."",
            ""iconUrl"": ""https://my-jira-domain.atlassian.net/rest/api/2/universal_avatar/view/type/issuetype/avatar/10303?size=medium"",
            ""name"": ""Bug"",
            ""subtask"": false,
            ""avatarId"": 10303,
            ""entityId"": ""e26d7d8b-4827-4461-b3ac-e0b37612dcb3"",
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
                    ""entityId"": ""0c640f1e-c9e5-4c84-acab-9824cbaad1c0"",
                    ""hierarchyLevel"": 1
                }
            }
        },
        ""comment"": {
            ""comments"": [
                {
                    ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/issue/10063/comment/10594"",
                    ""id"": ""10594"",
                    ""author"": {
                        ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/user?accountId=557058%3A27610eaf-dfb9-46a8-bf59-799b4d56c29c"",
                        ""accountId"": ""924aa932-bb6c-46e2-95fd-1f4a9633d779"",
                        ""emailAddress"": ""email_address@gmail.com"",
                        ""avatarUrls"": {
                            ""48x48"": ""https://avatar-management--avatars.us-west-2.prod.public.atl-paas.net/924aa932-bb6c-46e2-95fd-1f4a9633d779/7409a98c-95a9-4e5d-8dd9-e3bc1c3006aa/48"",
                            ""24x24"": ""https://avatar-management--avatars.us-west-2.prod.public.atl-paas.net/924aa932-bb6c-46e2-95fd-1f4a9633d779/7409a98c-95a9-4e5d-8dd9-e3bc1c3006aa/24"",
                            ""16x16"": ""https://avatar-management--avatars.us-west-2.prod.public.atl-paas.net/924aa932-bb6c-46e2-95fd-1f4a9633d779/7409a98c-95a9-4e5d-8dd9-e3bc1c3006aa/16"",
                            ""32x32"": ""https://avatar-management--avatars.us-west-2.prod.public.atl-paas.net/924aa932-bb6c-46e2-95fd-1f4a9633d779/7409a98c-95a9-4e5d-8dd9-e3bc1c3006aa/32""
                        },
                        ""displayName"": ""Bob Smith"",
                        ""active"": true,
                        ""timeZone"": ""Europe/London"",
                        ""accountType"": ""atlassian""
                    },
                    ""body"": {
                        ""version"": 1,
                        ""type"": ""doc"",
                        ""content"": [
                            {
                                ""type"": ""paragraph"",
                                ""content"": [
                                    {
                                        ""type"": ""text"",
                                        ""text"": ""Comment 1""
                                    }
                                ]
                            }
                        ]
                    },
                    ""updateAuthor"": {
                        ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/user?accountId=557058%3A27610eaf-dfb9-46a8-bf59-799b4d56c29c"",
                        ""accountId"": ""924aa932-bb6c-46e2-95fd-1f4a9633d779"",
                        ""emailAddress"": ""email_address@gmail.com"",
                        ""avatarUrls"": {
                            ""48x48"": ""https://avatar-management--avatars.us-west-2.prod.public.atl-paas.net/924aa932-bb6c-46e2-95fd-1f4a9633d779/7409a98c-95a9-4e5d-8dd9-e3bc1c3006aa/48"",
                            ""24x24"": ""https://avatar-management--avatars.us-west-2.prod.public.atl-paas.net/924aa932-bb6c-46e2-95fd-1f4a9633d779/7409a98c-95a9-4e5d-8dd9-e3bc1c3006aa/24"",
                            ""16x16"": ""https://avatar-management--avatars.us-west-2.prod.public.atl-paas.net/924aa932-bb6c-46e2-95fd-1f4a9633d779/7409a98c-95a9-4e5d-8dd9-e3bc1c3006aa/16"",
                            ""32x32"": ""https://avatar-management--avatars.us-west-2.prod.public.atl-paas.net/924aa932-bb6c-46e2-95fd-1f4a9633d779/7409a98c-95a9-4e5d-8dd9-e3bc1c3006aa/32""
                        },
                        ""displayName"": ""Bob Smith"",
                        ""active"": true,
                        ""timeZone"": ""Europe/London"",
                        ""accountType"": ""atlassian""
                    },
                    ""created"": ""2022-12-30T22:15:49.070+0000"",
                    ""updated"": ""2022-12-30T22:15:50.070+0000"",
                    ""jsdPublic"": true
                },
                {
                    ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/issue/10063/comment/10595"",
                    ""id"": ""10595"",
                    ""author"": {
                        ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/user?accountId=557058%3A27610eaf-dfb9-46a8-bf59-799b4d56c29c"",
                        ""accountId"": ""924aa932-bb6c-46e2-95fd-1f4a9633d779"",
                        ""emailAddress"": ""email_address_2@gmail.com"",
                        ""avatarUrls"": {
                            ""48x48"": ""https://avatar-management--avatars.us-west-2.prod.public.atl-paas.net/924aa932-bb6c-46e2-95fd-1f4a9633d779/7409a98c-95a9-4e5d-8dd9-e3bc1c3006aa/48"",
                            ""24x24"": ""https://avatar-management--avatars.us-west-2.prod.public.atl-paas.net/924aa932-bb6c-46e2-95fd-1f4a9633d779/7409a98c-95a9-4e5d-8dd9-e3bc1c3006aa/24"",
                            ""16x16"": ""https://avatar-management--avatars.us-west-2.prod.public.atl-paas.net/924aa932-bb6c-46e2-95fd-1f4a9633d779/7409a98c-95a9-4e5d-8dd9-e3bc1c3006aa/16"",
                            ""32x32"": ""https://avatar-management--avatars.us-west-2.prod.public.atl-paas.net/924aa932-bb6c-46e2-95fd-1f4a9633d779/7409a98c-95a9-4e5d-8dd9-e3bc1c3006aa/32""
                        },
                        ""displayName"": ""Bob Smith 2"",
                        ""active"": true,
                        ""timeZone"": ""Europe/London"",
                        ""accountType"": ""atlassian""
                    },
                    ""body"": {
                        ""version"": 1,
                        ""type"": ""doc"",
                        ""content"": [
                            {
                                ""type"": ""paragraph"",
                                ""content"": [
                                    {
                                        ""type"": ""text"",
                                        ""text"": ""Comment 2""
                                    }
                                ]
                            }
                        ]
                    },
                    ""updateAuthor"": {
                        ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/user?accountId=557058%3A27610eaf-dfb9-46a8-bf59-799b4d56c29c"",
                        ""accountId"": ""924aa932-bb6c-46e2-95fd-1f4a9633d779"",
                        ""emailAddress"": ""email_address@gmail.com"",
                        ""avatarUrls"": {
                            ""48x48"": ""https://avatar-management--avatars.us-west-2.prod.public.atl-paas.net/924aa932-bb6c-46e2-95fd-1f4a9633d779/7409a98c-95a9-4e5d-8dd9-e3bc1c3006aa/48"",
                            ""24x24"": ""https://avatar-management--avatars.us-west-2.prod.public.atl-paas.net/924aa932-bb6c-46e2-95fd-1f4a9633d779/7409a98c-95a9-4e5d-8dd9-e3bc1c3006aa/24"",
                            ""16x16"": ""https://avatar-management--avatars.us-west-2.prod.public.atl-paas.net/924aa932-bb6c-46e2-95fd-1f4a9633d779/7409a98c-95a9-4e5d-8dd9-e3bc1c3006aa/16"",
                            ""32x32"": ""https://avatar-management--avatars.us-west-2.prod.public.atl-paas.net/924aa932-bb6c-46e2-95fd-1f4a9633d779/7409a98c-95a9-4e5d-8dd9-e3bc1c3006aa/32""
                        },
                        ""displayName"": ""Bob Smith"",
                        ""active"": true,
                        ""timeZone"": ""Europe/London"",
                        ""accountType"": ""atlassian""
                    },
                    ""created"": ""2022-12-30T22:15:51.649+0000"",
                    ""updated"": ""2022-12-30T22:15:51.649+0000"",
                    ""jsdPublic"": true
                },
                {
                    ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/issue/10063/comment/10596"",
                    ""id"": ""10596"",
                    ""author"": {
                        ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/user?accountId=557058%3A27610eaf-dfb9-46a8-bf59-799b4d56c29c"",
                        ""accountId"": ""924aa932-bb6c-46e2-95fd-1f4a9633d779"",
                        ""emailAddress"": ""email_address_3@gmail.com"",
                        ""avatarUrls"": {
                            ""48x48"": ""https://avatar-management--avatars.us-west-2.prod.public.atl-paas.net/924aa932-bb6c-46e2-95fd-1f4a9633d779/7409a98c-95a9-4e5d-8dd9-e3bc1c3006aa/48"",
                            ""24x24"": ""https://avatar-management--avatars.us-west-2.prod.public.atl-paas.net/924aa932-bb6c-46e2-95fd-1f4a9633d779/7409a98c-95a9-4e5d-8dd9-e3bc1c3006aa/24"",
                            ""16x16"": ""https://avatar-management--avatars.us-west-2.prod.public.atl-paas.net/924aa932-bb6c-46e2-95fd-1f4a9633d779/7409a98c-95a9-4e5d-8dd9-e3bc1c3006aa/16"",
                            ""32x32"": ""https://avatar-management--avatars.us-west-2.prod.public.atl-paas.net/924aa932-bb6c-46e2-95fd-1f4a9633d779/7409a98c-95a9-4e5d-8dd9-e3bc1c3006aa/32""
                        },
                        ""displayName"": ""Bob Smith 3"",
                        ""active"": true,
                        ""timeZone"": ""Europe/London"",
                        ""accountType"": ""atlassian""
                    },
                    ""body"": {
                        ""version"": 1,
                        ""type"": ""doc"",
                        ""content"": [
                            {
                                ""type"": ""paragraph"",
                                ""content"": [
                                    {
                                        ""type"": ""text"",
                                        ""text"": ""Comment 3""
                                    }
                                ]
                            }
                        ]
                    },
                    ""updateAuthor"": {
                        ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/user?accountId=557058%3A27610eaf-dfb9-46a8-bf59-799b4d56c29c"",
                        ""accountId"": ""924aa932-bb6c-46e2-95fd-1f4a9633d779"",
                        ""emailAddress"": ""email_address@gmail.com"",
                        ""avatarUrls"": {
                            ""48x48"": ""https://avatar-management--avatars.us-west-2.prod.public.atl-paas.net/924aa932-bb6c-46e2-95fd-1f4a9633d779/7409a98c-95a9-4e5d-8dd9-e3bc1c3006aa/48"",
                            ""24x24"": ""https://avatar-management--avatars.us-west-2.prod.public.atl-paas.net/924aa932-bb6c-46e2-95fd-1f4a9633d779/7409a98c-95a9-4e5d-8dd9-e3bc1c3006aa/24"",
                            ""16x16"": ""https://avatar-management--avatars.us-west-2.prod.public.atl-paas.net/924aa932-bb6c-46e2-95fd-1f4a9633d779/7409a98c-95a9-4e5d-8dd9-e3bc1c3006aa/16"",
                            ""32x32"": ""https://avatar-management--avatars.us-west-2.prod.public.atl-paas.net/924aa932-bb6c-46e2-95fd-1f4a9633d779/7409a98c-95a9-4e5d-8dd9-e3bc1c3006aa/32""
                        },
                        ""displayName"": ""Bob Smith"",
                        ""active"": true,
                        ""timeZone"": ""Europe/London"",
                        ""accountType"": ""atlassian""
                    },
                    ""created"": ""2022-12-30T22:15:56.744+0000"",
                    ""updated"": ""2022-12-30T22:15:56.744+0000"",
                    ""jsdPublic"": true
                }
            ],
            ""self"": ""https://my-jira-domain.atlassian.net/rest/api/3/issue/10063/comment"",
            ""maxResults"": 3,
            ""total"": 3,
            ""startAt"": 0
        },
        ""issuelinks"": [],
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
        }
    }
}
";
    }
}
