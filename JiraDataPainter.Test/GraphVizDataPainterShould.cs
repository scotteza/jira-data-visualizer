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
		PROJ_5 [shape=""rectangle"" style=""filled"" fillcolor=""red"" label=""Task: Task in epic""];
		PROJ_6 [shape=""rectangle"" style=""filled"" fillcolor=""red"" label=""Bug: Bug in epic""];
		PROJ_7 [shape=""rectangle"" style=""filled"" fillcolor=""yellow"" label=""Story: Story in epic""];
		label = ""PROJ-4"";
	}
	subgraph cluster_1 {
		PROJ_9 [shape=""rectangle"" style=""filled"" fillcolor=""green"" label=""Task: Task in epic 2""];
		label = ""PROJ-8"";
	}
	PROJ_1 [shape=""rectangle"" style=""filled"" fillcolor=""white"" label=""Task: Testing 123 Task Without Epic""];
	PROJ_2 [shape=""rectangle"" style=""filled"" fillcolor=""red"" label=""Bug: Testing 123 Bug Without Epic""];
	PROJ_3 [shape=""rectangle"" style=""filled"" fillcolor=""green"" label=""Story: Testing 123 Story Without Epic""];
}
";
        _graphWriter.Setup(x => x.WriteGraph(expectedOutput));

        // ReSharper disable once UseObjectOrCollectionInitializer
        var issues = new List<JiraIssue>();

        issues.Add(new JiraIssue("PROJ-1",
            new JiraIssueFields("Testing 123 Task Without Epic",
                new JiraIssueParentEpic(string.Empty),
                new JiraIssueType("Task"),
                new JiraIssueStatus("Unknown Status"))));

        issues.Add(new JiraIssue("PROJ-2",
            new JiraIssueFields("Testing 123 Bug Without Epic",
                new JiraIssueParentEpic(string.Empty),
                new JiraIssueType("Bug"),
                new JiraIssueStatus("To Do"))));

        issues.Add(new JiraIssue("PROJ-3",
            new JiraIssueFields("Testing 123 Story Without Epic",
                new JiraIssueParentEpic(string.Empty),
                new JiraIssueType("Story"),
                new JiraIssueStatus("Done"))));

        issues.Add(new JiraIssue("PROJ-4",
            new JiraIssueFields("Big important epic",
                new JiraIssueParentEpic(string.Empty),
                new JiraIssueType("Epic"),
                new JiraIssueStatus("To Do"))));

        issues.Add(new JiraIssue("PROJ-5",
            new JiraIssueFields("Task in epic",
                new JiraIssueParentEpic("PROJ-4"),
                new JiraIssueType("Task"),
                new JiraIssueStatus("To Do"))));

        issues.Add(new JiraIssue("PROJ-6",
            new JiraIssueFields("Bug in epic",
                new JiraIssueParentEpic("PROJ-4"),
                new JiraIssueType("Bug"),
                new JiraIssueStatus("To Do"))));

        issues.Add(new JiraIssue("PROJ-7",
            new JiraIssueFields("Story in epic",
                new JiraIssueParentEpic("PROJ-4"),
                new JiraIssueType("Story"),
                new JiraIssueStatus("In Progress"))));

        issues.Add(new JiraIssue("PROJ-8",
            new JiraIssueFields("Big important epic 2",
                new JiraIssueParentEpic(string.Empty),
                new JiraIssueType("Epic"),
                new JiraIssueStatus("Done"))));

        issues.Add(new JiraIssue("PROJ-9",
            new JiraIssueFields("Task in epic 2",
                new JiraIssueParentEpic("PROJ-8"),
                new JiraIssueType("Task"),
                new JiraIssueStatus("Done"))));


        _painter.PaintData(issues.AsReadOnly());


        _graphWriter.Verify(x => x.WriteGraph(expectedOutput), Times.Once);
    }
}