using JiraDataFetcher.DTO;

namespace JiraDataPainter;

public interface IDataPainter
{
    void PaintData(IReadOnlyList<JiraIssue> issues);
}
