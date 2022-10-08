using JiraDataFetcher;
using Moq;
using NUnit.Framework;

namespace JiraDataVisualizer.Test;

internal class VisualizerShould
{
    private Mock<IDataFetcher> _dataFetcher = null!;
    private Mock<IDataPainter> _dataPainter = null!;
    private Visualizer _visualizer = null!;

    [SetUp]
    public void SetUp()
    {
        _dataFetcher = new Mock<IDataFetcher>(MockBehavior.Strict);
        _dataPainter = new Mock<IDataPainter>(MockBehavior.Strict);

        _visualizer = new Visualizer(_dataFetcher.Object, _dataPainter.Object);
    }

    [Test]
    public async Task Visualize_A_Group_Of_Jira_Tickets()
    {
        var issues = new List<JiraIssue>().AsReadOnly();
        var searchResult = new JiraIssueSearchResult(issues);
        _dataFetcher.Setup(x => x.SearchIssues("PROJ", 50)).Returns(Task.FromResult(searchResult));

        _dataPainter.Setup(x => x.PaintData(issues));


        await _visualizer.Visualize("PROJ");


        _dataPainter.Verify(x => x.PaintData(issues));
    }
}
