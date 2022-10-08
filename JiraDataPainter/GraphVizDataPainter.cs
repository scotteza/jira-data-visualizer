using JiraDataFetcher.DTO;
using System.Text;

namespace JiraDataPainter;

public class GraphVizDataPainter : IDataPainter
{
    private readonly IGraphVizGraphWriter _graphWriter;

    public GraphVizDataPainter(IGraphVizGraphWriter graphWriter)
    {
        _graphWriter = graphWriter;
    }

    public void PaintData(IReadOnlyList<JiraIssue> issues)
    {
        var sb = new StringBuilder();
        sb.AppendLine("digraph G {");

        var subGraphClusterIndex = 0;
        var epics = issues.Where(x => x.IssueType.Equals("Epic")).OrderBy(x => x.Summary);
        foreach (var epic in epics)
        {
            sb.AppendLine($"\tsubgraph cluster_{subGraphClusterIndex} {{");

            var epicIssues = issues.Where(x => x.ParentEpicKey.Equals(epic.Key)).OrderBy(x => x.Key);
            foreach (var epicIssue in epicIssues)
            {
                var nodeKey = epicIssue.Key.Replace('-', '_');
                sb.AppendLine($"\t\t{nodeKey} [shape=\"rectangle\" label=\"{epicIssue.IssueType}: {epicIssue.Summary}\"];");
            }

            sb.AppendLine($"\t\tlabel = \"{epic.Key}\";");

            sb.AppendLine("\t}");

            subGraphClusterIndex++;
        }

        var epiclessIssues = issues.Where(x => string.IsNullOrWhiteSpace(x.ParentEpicKey) && x.IssueType != "Epic").OrderBy(x => x.Key);
        foreach (var epicIssue in epiclessIssues)
        {
            var nodeKey = epicIssue.Key.Replace('-', '_');
            sb.AppendLine($"\t{nodeKey} [shape=\"rectangle\" label=\"{epicIssue.IssueType}: {epicIssue.Summary}\"];");
        }

        sb.AppendLine("}");

        _graphWriter.WriteGraph(sb.ToString());
    }
}
