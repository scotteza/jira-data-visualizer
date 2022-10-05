using NUnit.Framework;

namespace JiraDataVisualizer.Test
{
    internal class VisualizerShould
    {
        [Test]
        public void Visualize_A_Thing()
        {
            var result = new Visualizer().Visualize();
            Assert.True(result);
        }
    }

}
