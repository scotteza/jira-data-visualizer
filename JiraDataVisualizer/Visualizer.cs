using JiraDataFetcher;

namespace JiraDataVisualizer;

public class Visualizer
{
    private readonly IDataFetcher _dataFetcher;

    public Visualizer(IDataFetcher dataFetcher)
    {
        _dataFetcher = dataFetcher;
    }

    public bool Visualize()
    {
        return true;
    }
}
