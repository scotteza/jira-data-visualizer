using JiraDataFetcher.DTO;
using Moq;
using NUnit.Framework;

namespace JiraDataPainter.Test;

internal class GraphVizDataPainterShould
{
    private GraphVizDataPainter _painter = null!;
    private Mock<IGraphVizGraphWriter> _graphWriter = null!;

    [SetUp]
    public void SetUp()
    {
        _graphWriter = new Mock<IGraphVizGraphWriter>(MockBehavior.Loose);
        _painter = new GraphVizDataPainter(_graphWriter.Object);
    }

    [Test]
    public void Paint_A_Jira_Issue_List()
    {
        const string expectedOutput = @"digraph G {
	subgraph cluster_0 {
		PROJ_5 [shape=""rectangle"" style=""filled"" fillcolor=""pink"" label=""PROJ-5\nTask\nTask in epic""];
		PROJ_6 [shape=""rectangle"" style=""filled"" fillcolor=""pink"" label=""PROJ-6\nBug\nBug in epic""];
		PROJ_7 [shape=""rectangle"" style=""filled"" fillcolor=""yellow"" label=""PROJ-7\nStory\nStory in epic""];
		label = ""PROJ-4\nBig important epic"";
		bgcolor=""azure""
	}
	subgraph cluster_1 {
		PROJ_9 [shape=""rectangle"" style=""filled"" fillcolor=""green"" label=""PROJ-9\nTask\nTask in epic 2""];
		label = ""PROJ-8\nBig important epic 2"";
		bgcolor=""azure""
	}
	PROJ_1 [shape=""rectangle"" style=""filled"" fillcolor=""white"" label=""PROJ-1\nTask\nTesting 123 Task Without Epic""];
	PROJ_2 [shape=""rectangle"" style=""filled"" fillcolor=""pink"" label=""PROJ-2\nBug\nTesting 123 Bug Without Epic""];
	PROJ_3 [shape=""rectangle"" style=""filled"" fillcolor=""green"" label=""PROJ-3\nStory\nTesting 123 Story Without Epic""];

	PROJ_8->PROJ_9
}
";
        _graphWriter.Setup(x => x.WriteGraph(expectedOutput));

        // ReSharper disable once UseObjectOrCollectionInitializer
        var issues = new List<JiraIssue>();

        issues.Add(new JiraIssue("PROJ-1",
            new JiraIssueFields("Testing 123 Task Without Epic",
                new JiraIssueParentEpic(string.Empty),
                new JiraIssueType("Task"),
                new JiraIssueStatus("Unknown Status"),
                new List<JiraIssueLinks>())));

        issues.Add(new JiraIssue("PROJ-2",
            new JiraIssueFields("Testing 123 Bug Without Epic",
                new JiraIssueParentEpic(string.Empty),
                new JiraIssueType("Bug"),
                new JiraIssueStatus("To Do"),
                new List<JiraIssueLinks>())));

        issues.Add(new JiraIssue("PROJ-3",
            new JiraIssueFields("Testing 123 Story Without Epic",
                new JiraIssueParentEpic(string.Empty),
                new JiraIssueType("Story"),
                new JiraIssueStatus("Done"),
                new List<JiraIssueLinks>())));

        issues.Add(new JiraIssue("PROJ-4",
            new JiraIssueFields("Big important epic",
                new JiraIssueParentEpic(string.Empty),
                new JiraIssueType("Epic"),
                new JiraIssueStatus("To Do"),
                new List<JiraIssueLinks>())));

        issues.Add(new JiraIssue("PROJ-5",
            new JiraIssueFields("Task in epic",
                new JiraIssueParentEpic("PROJ-4"),
                new JiraIssueType("Task"),
                new JiraIssueStatus("To Do"),
                new List<JiraIssueLinks>())));

        issues.Add(new JiraIssue("PROJ-6",
            new JiraIssueFields("Bug in epic",
                new JiraIssueParentEpic("PROJ-4"),
                new JiraIssueType("Bug"),
                new JiraIssueStatus("To Do"),
                new List<JiraIssueLinks>())));

        issues.Add(new JiraIssue("PROJ-7",
            new JiraIssueFields("Story in epic",
                new JiraIssueParentEpic("PROJ-4"),
                new JiraIssueType("Story"),
                new JiraIssueStatus("In Progress"),
                new List<JiraIssueLinks>())));

        // 8 blocks 9
        var issue8Links = new List<JiraIssueLinks>
        {
            new(new IssueLinkType("is blocked by", "blocks"),
                null,
                new JiraIssue("PROJ-9", null))
        };
        var issue8 = new JiraIssue("PROJ-8",
            new JiraIssueFields("Big important epic 2",
                new JiraIssueParentEpic(string.Empty),
                new JiraIssueType("Epic"),
                new JiraIssueStatus("Done"),
                issue8Links));
        issues.Add(issue8);

        // 9 is blocked by 8
        var issue9Links = new List<JiraIssueLinks>
        {
            new(new IssueLinkType("is blocked by", "blocks"),
                new JiraIssue("PROJ-8", null),
                null)
        };
        var issue9 = new JiraIssue("PROJ-9",
            new JiraIssueFields("Task in epic 2",
                new JiraIssueParentEpic("PROJ-8"),
                new JiraIssueType("Task"),
                new JiraIssueStatus("Done"),
                issue9Links));
        issues.Add(issue9);


        _painter.PaintData(issues.AsReadOnly());


        _graphWriter.Verify(x => x.WriteGraph(expectedOutput), Times.Once);
    }
}