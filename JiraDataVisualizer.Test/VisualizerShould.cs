using JiraDataFetcher;
using Moq;
using NUnit.Framework;

namespace JiraDataVisualizer.Test;

internal class VisualizerShould
{
    [Test]
    public void Visualize_A_Thing()
    {
        var dataFetcher = new Mock<IDataFetcher>(MockBehavior.Strict);
        var result = new Visualizer(dataFetcher.Object).Visualize();
        Assert.True(result);
    }
}
