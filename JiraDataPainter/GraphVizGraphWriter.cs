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
            var graphVizPath = new FileInfo($"./input_{dateString}.dot").FullName;
            var fullBatchPath = new FileInfo($"./generate-output_{dateString}.sh").FullName;
            var fullOutputImagePath = new FileInfo($"./output_{dateString}.svg").FullName;

            File.WriteAllText(graphVizPath, graphData);
            File.WriteAllText(fullBatchPath, @$"dot -Tsvg {graphVizPath} > {fullOutputImagePath}{Environment.NewLine}");

            const string command = "sh";

            var processInfo1 = new ProcessStartInfo
            {
                FileName = command,
                Arguments = fullBatchPath,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            var process1 = Process.Start(processInfo1);
            while (!process1.StandardOutput.EndOfStream)
            {
                var result = process1.StandardOutput.ReadLine();
                Console.WriteLine(result);
            }
            
            process1.WaitForExit();

            var processInfo2 = new ProcessStartInfo
            {
                FileName = "open",
                Arguments = $"{fullOutputImagePath}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            var process2 = Process.Start(processInfo2);
            while (!process2.StandardOutput.EndOfStream)
            {
                var result = process2.StandardOutput.ReadLine();
                Console.WriteLine(result);
            }

            process2.WaitForExit();
        }
    }
}