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
        PaintEpics(issues, sb);
        PaintEpiclessIssues(issues, sb);
        sb.AppendLine("}");

        _graphWriter.WriteGraph(sb.ToString());
    }

    private void PaintEpics(IReadOnlyList<JiraIssue> issues, StringBuilder sb)
    {
        var subGraphClusterIndex = 0;
        var epics =
            issues.Where(x => x.IssueType.Equals("Epic"))
                .OrderBy(x => x.Summary);

        foreach (var epic in epics)
        {
            sb.AppendLine($"\tsubgraph cluster_{subGraphClusterIndex} {{");

            PaintEpicSubIssues(issues, sb, epic);

            sb.AppendLine($"\t\tlabel = \"{epic.Key}\";");
            sb.AppendLine("\t}");

            subGraphClusterIndex++;
        }
    }

    private void PaintEpicSubIssues(IReadOnlyList<JiraIssue> issues, StringBuilder sb, JiraIssue epic)
    {
        var Issues = issues.Where(x => x.ParentEpicKey.Equals(epic.Key)).OrderBy(x => x.Key);
        foreach (var issue in Issues)
        {
            var nodeKey = GetNodeKey(issue);
            sb.AppendLine($"\t\t{nodeKey} [shape=\"rectangle\" style=\"filled\" fillcolor=\"{GetFillColorForStatus(issue.Status)}\" label=\"{issue.IssueType}: {issue.Summary}\"];");
        }
    }

    private void PaintEpiclessIssues(IReadOnlyList<JiraIssue> issues, StringBuilder sb)
    {
        var Issues =
            issues.Where(x => string.IsNullOrWhiteSpace(x.ParentEpicKey) && x.IssueType != "Epic")
            .OrderBy(x => x.Key);

        foreach (var issue in Issues)
        {
            var nodeKey = GetNodeKey(issue);
            sb.AppendLine($"\t{nodeKey} [shape=\"rectangle\" style=\"filled\" fillcolor=\"{GetFillColorForStatus(issue.Status)}\" label=\"{issue.IssueType}: {issue.Summary}\"];");
        }
    }

    private string GetNodeKey(JiraIssue issue)
    {
        return issue.Key.Replace('-', '_');
    }

    private string GetFillColorForStatus(string status)
    {
        return status switch
        {
            "To Do" => "red",
            "In Progress" => "yellow",
            "Done" => "green",
            _ => "white"
        };
    }
}
