﻿using JiraDataFetcher.DTO;
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
        sb.AppendLine();
        PaintIssueLinks(issues, sb);
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

            sb.AppendLine($"\t\tlabel = \"{epic.Key}\\n{epic.Summary}\";");
            sb.AppendLine("\t\tbgcolor=\"azure\"");
            sb.AppendLine("\t}");

            subGraphClusterIndex++;
        }
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

    private void PaintEpiclessIssues(IReadOnlyList<JiraIssue> issues, StringBuilder sb)
    {
        var epiclessIssues =
            issues.Where(x => string.IsNullOrWhiteSpace(x.ParentEpicKey) && x.IssueType != "Epic")
            .OrderBy(x => x.Key);

        foreach (var issue in epiclessIssues)
        {
            var graphVizStringForIssue = GetGraphVizStringForIssue(issue);
            sb.AppendLine($"\t{graphVizStringForIssue}");
        }
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
        return $"{nodeKey} [shape=\"rectangle\" style=\"filled\" fillcolor=\"{GetFillColorForStatus(issue.Status)}\" label=\"{issue.Key}\\n{issue.IssueType}\\n{issue.Summary}\"];";
    }

    private string GetFillColorForStatus(string status)
    {
        return status switch
        {
            "To Do" => "pink",
            "In Progress" => "yellow",
            "Done" => "green",
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
