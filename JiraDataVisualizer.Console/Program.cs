﻿using HttpWrapper;
using JiraDataFetcher;
using JiraDataPainter;

namespace JiraDataVisualizer.Console;

internal class Program
{
    static async Task Main(string[] args)
    {
        var fileLines = await File.ReadAllLinesAsync(@"../../../../Token.txt");
        var jiraDomain = fileLines[0];
        var username = fileLines[1];
        var password = fileLines[2];
        var issueKey = fileLines[3];
        var projectName = fileLines[4];

        var dataFetcher = new DataFetcher(new HttpWrapper.HttpWrapper(), jiraDomain, username, password);
        var dataPainter = new GraphVizDataPainter(new GraphVizGraphWriter());

        var issue = await dataFetcher.FetchIssue(issueKey);

        var visualizer = new Visualizer(dataFetcher, dataPainter);

        await visualizer.Visualize(projectName);
    }
}
