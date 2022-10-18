namespace JiraDataPainter;

public class GraphVizGraphWriter : IGraphVizGraphWriter
{
    public void WriteGraph(string graphData)
    {
        var graphVizPath = new FileInfo("./input.dot").FullName;
        File.WriteAllText(graphVizPath, graphData);

        var fullBatchPath = new FileInfo("./generate-output.bat").FullName;
        var fullOutputImagePath = new FileInfo("./output.svg").FullName;
        File.WriteAllText(fullBatchPath, @$"dot -Tsvg input.dot > {fullOutputImagePath}
""C:\Program Files\Mozilla Firefox\firefox.exe"" {fullOutputImagePath}");

        //System.Diagnostics.Process.Start("explorer.exe", $"/select, \"{fullBatchPath}\"");
        System.Diagnostics.Process.Start("cmd.exe", $"/c {fullBatchPath}");
    }
}
