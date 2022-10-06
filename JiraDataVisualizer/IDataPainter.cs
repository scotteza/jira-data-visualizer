using JiraDataFetcher;

namespace JiraDataVisualizer;

public interface IDataPainter
{
    void PaintData(IReadOnlyList<JiraIssue> getIssues);
}
