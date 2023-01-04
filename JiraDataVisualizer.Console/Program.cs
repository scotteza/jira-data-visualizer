using JiraDataFetcher;
using JiraDataPainter;

namespace JiraDataVisualizer.Console;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var pause = args.Length > 0;

        var fileLines = await File.ReadAllLinesAsync(@"../../../../Token.txt");
        var jiraDomain = fileLines[0];
        var username = fileLines[1];
        var password = fileLines[2];
        var issueKey = fileLines[3];

        var dataFetcher = new DataFetcher(new HttpWrapper.HttpWrapper(), jiraDomain, username, password);
        var dataPainter = new GraphVizDataPainter(new GraphVizGraphWriter());

        var issue = await dataFetcher.FetchIssue(issueKey);

        var visualizer = new Visualizer(dataFetcher, dataPainter);

        var previousLine = "";
        
        for (var i = 4; i < fileLines.Length; i++)
        {
            var jqlSearchString = fileLines[i];
            if (jqlSearchString.StartsWith("#"))
            {
                previousLine = jqlSearchString;
                continue;
            }
            System.Console.WriteLine();
            System.Console.WriteLine(previousLine);
            System.Console.WriteLine($"Executing JQL search and print for string JQL query: {jqlSearchString}");
            await visualizer.Visualize(jqlSearchString);
            System.Console.WriteLine("Visualisation complete");
            if (pause)
            {
                Thread.Sleep(2000);
            }
            previousLine = jqlSearchString;
        }
        
        System.Console.WriteLine();
    }
}