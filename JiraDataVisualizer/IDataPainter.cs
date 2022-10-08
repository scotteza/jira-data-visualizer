using JiraDataFetcher.DTO;

namespace JiraDataVisualizer;

public interface IDataPainter
{
    void PaintData(IReadOnlyList<JiraIssue> getIssues);
}
