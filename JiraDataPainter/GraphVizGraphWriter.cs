namespace JiraDataPainter;

public class GraphVizGraphWriter : IGraphVizGraphWriter
{
    public void WriteGraph(string graphData)
    {
        var graphVizPath = new FileInfo("./input.dot").FullName;
        File.WriteAllText(graphVizPath, graphData);

        var fullBatchPath = new FileInfo("./generate-output.bat").FullName;
        File.WriteAllText(fullBatchPath, @"dot -Tpng input.dot > output.png
code output.png");

        System.Diagnostics.Process.Start("explorer.exe", $"/select, \"{fullBatchPath}\"");
    }
}
