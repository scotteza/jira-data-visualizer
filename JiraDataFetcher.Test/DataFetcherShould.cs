﻿using HttpWrapper;
using Moq;
using NUnit.Framework;

namespace JiraDataFetcher.Test;

internal class DataFetcherShould
{
    // TODO: add links and status checks to tickets
    [Test]
    public async Task Fetch_A_Jira_Issue_Without_A_Parent_Epic()
    {
        var httpGetter = new Mock<IHttpGetter>(MockBehavior.Strict);

        var httpResult = GetRealisticJiraSingleIssueWithoutEpicHttpResponse();
        httpGetter.Setup(x => x.GetWithBasicAuthentication("https://my-jira-domain.atlassian.net", "/rest/api/3/issue/PROJ-1?fields=key,summary,issuetype,parent,issuelinks,status", "username", "password")).Returns(Task.FromResult(httpResult));


        var dataFetcher = new DataFetcher(httpGetter.Object, "my-jira-domain", "username", "password");


        var result = await dataFetcher.FetchIssue("PROJ-1");


        httpGetter.Verify(x => x.GetWithBasicAuthentication(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);

        Assert.That(result.Key, Is.EqualTo("PROJ-1"));
        Assert.That(result.Summary, Is.EqualTo("Testing 123 Task Without Epic"));
        Assert.That(result.ParentEpicKey, Is.Empty);
        Assert.That(result.IssueType, Is.EqualTo("Task"));
        Assert.That(result.Status, Is.EqualTo("To Do"));
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



    // TODO: add links and status checks to tickets
    [Test]
    public async Task Fetch_A_Jira_Issue_With_A_Parent_Epic()
    {
        var httpGetter = new Mock<IHttpGetter>(MockBehavior.Strict);

        var httpResult = GetRealisticJiraSingleIssueWithEpicHttpResponse();
        httpGetter.Setup(x => x.GetWithBasicAuthentication("https://my-jira-domain.atlassian.net", "/rest/api/3/issue/PROJ-1?fields=key,summary,issuetype,parent,issuelinks,status", "username", "password")).Returns(Task.FromResult(httpResult));


        var dataFetcher = new DataFetcher(httpGetter.Object, "my-jira-domain", "username", "password");


        var result = await dataFetcher.FetchIssue("PROJ-1");


        httpGetter.Verify(x => x.GetWithBasicAuthentication(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);

        Assert.That(result.Key, Is.EqualTo("PROJ-5"));
        Assert.That(result.Summary, Is.EqualTo("Task in epic"));
        Assert.That(result.ParentEpicKey, Is.EqualTo("PROJ-4"));
        Assert.That(result.IssueType, Is.EqualTo("Task"));
        Assert.That(result.Status, Is.EqualTo("In Progress"));
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
        var httpGetter = new Mock<IHttpGetter>(MockBehavior.Strict);

        var httpResult = GetRealisticJiraBlockerIssueHttpResponse();
        httpGetter.Setup(x => x.GetWithBasicAuthentication("https://my-jira-domain.atlassian.net", "/rest/api/3/issue/PROJ-9?fields=key,summary,issuetype,parent,issuelinks,status", "username", "password")).Returns(Task.FromResult(httpResult));


        var dataFetcher = new DataFetcher(httpGetter.Object, "my-jira-domain", "username", "password");


        var result = await dataFetcher.FetchIssue("PROJ-9");


        httpGetter.Verify(x => x.GetWithBasicAuthentication(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);

        Assert.That(result.Key, Is.EqualTo("PROJ-9"));
        var blockedIssues = result.GetBlockedIssues();
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
        var httpGetter = new Mock<IHttpGetter>(MockBehavior.Strict);

        var httpResult = GetRealisticJiraBlockedIssueHttpResponse();
        httpGetter.Setup(x => x.GetWithBasicAuthentication("https://my-jira-domain.atlassian.net", "/rest/api/3/issue/PROJ-9?fields=key,summary,issuetype,parent,issuelinks,status", "username", "password")).Returns(Task.FromResult(httpResult));


        var dataFetcher = new DataFetcher(httpGetter.Object, "my-jira-domain", "username", "password");


        var result = await dataFetcher.FetchIssue("PROJ-9");


        httpGetter.Verify(x => x.GetWithBasicAuthentication(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);

        Assert.That(result.Key, Is.EqualTo("PROJ-8"));
        var blockedIssues = result.GetBlockerIssues();
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
        var httpGetter = new Mock<IHttpGetter>(MockBehavior.Strict);

        httpGetter.Setup(x => x.GetWithBasicAuthentication("https://my-jira-domain.atlassian.net", "/rest/api/3/search?jql=project=PROJ+order+by+key+ASC&fields=key,summary,issuetype,parent,issuelinks,status&maxResults=2&startAt=0", "username", "password")).Returns(Task.FromResult(GetRealisticJiraSearchHttpResponse_Page1()));
        httpGetter.Setup(x => x.GetWithBasicAuthentication("https://my-jira-domain.atlassian.net", "/rest/api/3/search?jql=project=PROJ+order+by+key+ASC&fields=key,summary,issuetype,parent,issuelinks,status&maxResults=2&startAt=2", "username", "password")).Returns(Task.FromResult(GetRealisticJiraSearchHttpResponse_Page2()));
        httpGetter.Setup(x => x.GetWithBasicAuthentication("https://my-jira-domain.atlassian.net", "/rest/api/3/search?jql=project=PROJ+order+by+key+ASC&fields=key,summary,issuetype,parent,issuelinks,status&maxResults=2&startAt=4", "username", "password")).Returns(Task.FromResult(GetRealisticJiraSearchHttpResponse_Page3()));
        httpGetter.Setup(x => x.GetWithBasicAuthentication("https://my-jira-domain.atlassian.net", "/rest/api/3/search?jql=project=PROJ+order+by+key+ASC&fields=key,summary,issuetype,parent,issuelinks,status&maxResults=2&startAt=6", "username", "password")).Returns(Task.FromResult(GetRealisticJiraSearchHttpResponse_Page4()));
        httpGetter.Setup(x => x.GetWithBasicAuthentication("https://my-jira-domain.atlassian.net", "/rest/api/3/search?jql=project=PROJ+order+by+key+ASC&fields=key,summary,issuetype,parent,issuelinks,status&maxResults=2&startAt=8", "username", "password")).Returns(Task.FromResult(GetRealisticJiraSearchHttpResponse_Page5()));


        var dataFetcher = new DataFetcher(httpGetter.Object, "my-jira-domain", "username", "password");


        var result = await dataFetcher.SearchIssues("PROJ", 2);


        httpGetter.Verify(x => x.GetWithBasicAuthentication(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(5));

        var issues = result.GetIssues();
        Assert.That(issues.Count, Is.EqualTo(9));

        Assert.That(issues[0].Key, Is.EqualTo("PROJ-1"));
        Assert.That(issues[0].Summary, Is.EqualTo("Testing 123 Task Without Epic"));
        Assert.That(issues[0].ParentEpicKey, Is.Empty);
        Assert.That(issues[0].IssueType, Is.EqualTo("Task"));

        Assert.That(issues[1].Key, Is.EqualTo("PROJ-2"));
        Assert.That(issues[1].Summary, Is.EqualTo("Testing 123 Bug Without Epic"));
        Assert.That(issues[1].ParentEpicKey, Is.Empty);
        Assert.That(issues[1].IssueType, Is.EqualTo("Bug"));

        Assert.That(issues[2].Key, Is.EqualTo("PROJ-3"));
        Assert.That(issues[2].Summary, Is.EqualTo("Testing 123 Story Without Epic"));
        Assert.That(issues[2].ParentEpicKey, Is.Empty);
        Assert.That(issues[2].IssueType, Is.EqualTo("Story"));

        Assert.That(issues[3].Key, Is.EqualTo("PROJ-4"));
        Assert.That(issues[3].Summary, Is.EqualTo("Big important epic"));
        Assert.That(issues[3].ParentEpicKey, Is.Empty);
        Assert.That(issues[3].IssueType, Is.EqualTo("Epic"));

        Assert.That(issues[4].Key, Is.EqualTo("PROJ-5"));
        Assert.That(issues[4].Summary, Is.EqualTo("Task in epic"));
        Assert.That(issues[4].ParentEpicKey, Is.EqualTo("PROJ-4"));
        Assert.That(issues[4].IssueType, Is.EqualTo("Task"));

        Assert.That(issues[5].Key, Is.EqualTo("PROJ-6"));
        Assert.That(issues[5].Summary, Is.EqualTo("Bug in epic"));
        Assert.That(issues[5].ParentEpicKey, Is.EqualTo("PROJ-4"));
        Assert.That(issues[5].IssueType, Is.EqualTo("Bug"));

        Assert.That(issues[6].Key, Is.EqualTo("PROJ-7"));
        Assert.That(issues[6].Summary, Is.EqualTo("Story in epic"));
        Assert.That(issues[6].ParentEpicKey, Is.EqualTo("PROJ-4"));
        Assert.That(issues[6].IssueType, Is.EqualTo("Story"));

        Assert.That(issues[7].Key, Is.EqualTo("PROJ-8"));
        Assert.That(issues[7].Summary, Is.EqualTo("I am blocked"));
        Assert.That(issues[7].ParentEpicKey, Is.Empty);
        Assert.That(issues[7].IssueType, Is.EqualTo("Task"));

        Assert.That(issues[8].Key, Is.EqualTo("PROJ-9"));
        Assert.That(issues[8].Summary, Is.EqualTo("I am a blocker"));
        Assert.That(issues[8].ParentEpicKey, Is.Empty);
        Assert.That(issues[8].IssueType, Is.EqualTo("Task"));

    }

    private string GetRealisticJiraSearchHttpResponse_Page1()
    {
        return @"{
    ""expand"": ""schema,names"",
    ""startAt"": 0,
    ""maxResults"": 2,
    ""total"": 9,
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
                    ""entityId"": ""0ab48f98-440d-4234-b55a-7ae3109a7180"",
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
}";
    }

    private string GetRealisticJiraSearchHttpResponse_Page2()
    {
        return @"{
    ""expand"": ""schema,names"",
    ""startAt"": 2,
    ""maxResults"": 2,
    ""total"": 9,
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
                    ""entityId"": ""6999ad6f-7d10-41c8-bc8a-967c377bac91"",
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
}";
    }

    private string GetRealisticJiraSearchHttpResponse_Page3()
    {
        return @"{
    ""expand"": ""schema,names"",
    ""startAt"": 4,
    ""maxResults"": 2,
    ""total"": 9,
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
}";
    }

    private string GetRealisticJiraSearchHttpResponse_Page4()
    {
        return @"{
    ""expand"": ""schema,names"",
    ""startAt"": 6,
    ""maxResults"": 2,
    ""total"": 9,
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
}";
    }

    private string GetRealisticJiraSearchHttpResponse_Page5()
    {
        return @"{
    ""expand"": ""names,schema"",
    ""startAt"": 8,
    ""maxResults"": 2,
    ""total"": 9,
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
}";
    }
}
