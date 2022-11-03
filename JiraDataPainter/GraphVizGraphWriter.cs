using System.Diagnostics;
using System.Runtime.InteropServices;

namespace JiraDataPainter;

public class GraphVizGraphWriter : IGraphVizGraphWriter
{
    public void WriteGraph(string graphData)
    {
        var dateString = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff");

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            var graphVizPath = new FileInfo("./input.dot").FullName;
            File.WriteAllText(graphVizPath, graphData);

            var fullBatchPath = new FileInfo("./generate-output.bat").FullName;
            var fullOutputImagePath = new FileInfo($"./output_{dateString}.svg").FullName;
            File.WriteAllText(fullBatchPath, @$"dot -Tsvg input.dot > {fullOutputImagePath}
""C:\Program Files\Mozilla Firefox\firefox.exe"" {fullOutputImagePath}");

            Process.Start("cmd.exe", $"/c {fullBatchPath}");
        }
        
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            var graphVizPath = new FileInfo($"./input.dot").FullName;
            var fullBatchPath = new FileInfo("./generate-output.sh").FullName;
            var fullOutputImagePath = new FileInfo($"./output.svg").FullName;
            
            File.WriteAllText(graphVizPath, graphData);
            File.WriteAllText(fullBatchPath, @$"dot -Tsvg {graphVizPath} > {fullOutputImagePath}");
        }
    }
}
