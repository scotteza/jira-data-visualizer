using JiraDataFetcher;

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

    public async Task Visualize()
    {
        var issues = await _dataFetcher.SearchIssues();
        _dataPainter.PaintData(issues.GetIssues());
    }
}
