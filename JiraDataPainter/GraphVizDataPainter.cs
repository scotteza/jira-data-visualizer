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
        var subGraphClusterIndex = PaintEpics(issues, sb);
        PaintEpiclessIssues(issues, sb, subGraphClusterIndex);
        sb.AppendLine();
        PaintIssueLinks(issues, sb);
        sb.AppendLine("}");

        _graphWriter.WriteGraph(sb.ToString());
    }

    private int PaintEpics(IReadOnlyList<JiraIssue> issues, StringBuilder sb)
    {
        var subGraphClusterIndex = 0;
        var epics =
            issues.Where(x => x.IssueType.Equals("Epic"))
                .OrderBy(x => x.Summary);

        foreach (var epic in epics)
        {
            sb.AppendLine($"\tsubgraph cluster_{subGraphClusterIndex} {{");

            PaintEpicSubIssues(issues, sb, epic);

            sb.AppendLine($"\t\tlabel = \"{epic.Key}\\n{epic.Summary.Replace("\"", "\\\"")}\";");
            sb.AppendLine($"\t\tURL=\"{epic.GetFrontendUrl()}\";");
            sb.AppendLine($"\t\ttarget=\"_blank\";");
            sb.AppendLine("\t\tbgcolor=\"azure\";");
            sb.AppendLine("\t}");

            subGraphClusterIndex++;
        }

        return subGraphClusterIndex;
    }

    private void PaintEpicSubIssues(IReadOnlyList<JiraIssue> issues, StringBuilder sb, JiraIssue epic)
    {
        var issuesForEpic = issues.Where(x => x.ParentEpicKey.Equals(epic.Key)).OrderBy(x => x.Key);
        foreach (var issue in issuesForEpic)
        {
            var graphVizStringForIssue = GetGraphVizStringForIssue(issue);
            sb.AppendLine($"\t\t{graphVizStringForIssue}");
        }
    }

    private void PaintEpiclessIssues(IReadOnlyList<JiraIssue> issues, StringBuilder sb, int subGraphClusterIndex)
    {
        var issuesToPaint = new List<JiraIssue>();

        var epiclessIssues = issues.Where(x => string.IsNullOrWhiteSpace(x.ParentEpicKey) && x.IssueType != "Epic").ToList();
        issuesToPaint.AddRange(epiclessIssues);

        var issuesWithEpics = issues.Where(x => !string.IsNullOrWhiteSpace(x.ParentEpicKey) && x.IssueType != "Epic");
        foreach (var issue in issuesWithEpics)
        {
            if (!issues.Any(x => x.Key.Equals(issue.ParentEpicKey)))
            {
                issuesToPaint.Add(issue);
            }
        }

        sb.AppendLine($"\tsubgraph cluster_{subGraphClusterIndex} {{");

        foreach (var issue in issuesToPaint)
        {
            var graphVizStringForIssue = GetGraphVizStringForIssue(issue);
            sb.AppendLine($"\t\t{graphVizStringForIssue}");
        }

        sb.AppendLine($"\t\tlabel = \"No epic or epic not returned in search results\";");
        sb.AppendLine("\t\tbgcolor=\"lavender\";");
        sb.AppendLine("\t}");
    }

    private string GetNodeKey(JiraIssue issue)
    {
        var key = issue.Key;
        return GetNodeKey(key);
    }

    private string GetNodeKey(string key)
    {
        return key.Replace('-', '_');
    }

    private string GetGraphVizStringForIssue(JiraIssue issue)
    {
        var nodeKey = GetNodeKey(issue);
        return $"{nodeKey} [shape=\"rectangle\" style=\"filled\" fillcolor=\"{GetFillColorForStatus(issue.Status)}\" label=\"{issue.Key}\\n{issue.Summary.Replace("\"", "\\\"")}\" URL=\"{issue.GetFrontendUrl()}\" target=\"_blank\"];";
    }

    private string GetFillColorForStatus(string status)
    {
        return status switch
        {
            "To Do" => "pink",
            "Selected for Development" => "pink",
            "Important" => "pink",
            "Urgent" => "pink",
            "Important + Urgent" => "red",
            "Low Priority" => "floralwhite",
            "In Progress" => "yellow",
            "Done" => "lightgreen",
            "Closed" => "teal",
            "Deprecated" => "gray",
            "Blocked" => "lavender",
            _ => "white"
        };
    }

    private void PaintIssueLinks(IReadOnlyList<JiraIssue> issues, StringBuilder sb)
    {
        var links = new List<string>();
        foreach (var issue in issues)
        {
            var issuesThatBlockMe = issue.GetIssuesThatBlockMe();
            var issuesThatIBlock = issue.GetIssuesThatIBlock();
            links.AddRange(GetIssuesThatBlockMeText(issue.Key, issuesThatBlockMe));
            links.AddRange(GetIssuesThatIBlockText(issue.Key, issuesThatIBlock));
        }

        foreach (var link in links.Distinct().OrderBy(x => x))
        {
            sb.AppendLine($"\t{link}");
        }
    }

    private List<string> GetIssuesThatBlockMeText(string key, IReadOnlyList<string> issuesThatBlockMe)
    {
        var result = new List<string>();
        foreach (var issue in issuesThatBlockMe)
        {
            result.Add($"{GetNodeKey(issue)}->{GetNodeKey(key)}");
        }

        return result;
    }

    private List<string> GetIssuesThatIBlockText(string key, IReadOnlyList<string> issuesThatIBlock)
    {
        var result = new List<string>();
        foreach (var issue in issuesThatIBlock)
        {
            result.Add($"{GetNodeKey(key)}->{GetNodeKey(issue)}");
        }

        return result;
    }
}
