using JiraDataFetcher;
using JiraDataPainter;

namespace JiraDataVisualizer;

public class Visualizer
{
    private readonly IDataFetcher _dataFetcher;
    private readonly IDataPainter _dataPainter;

    public Visualizer(IDataFetcher dataFetcher, IDataPainter dataPainter)
    {
        _dataFetcher = dataFetcher;
        _dataPainter = dataPainter;
    }

    public async Task Visualize(string projectName)
    {
        var issues = await _dataFetcher.SearchIssues(projectName, 50);
        _dataPainter.PaintData(issues.GetIssues());
    }
}
