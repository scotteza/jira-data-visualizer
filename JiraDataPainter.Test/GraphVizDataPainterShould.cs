using JiraDataFetcher.DTO;
using NUnit.Framework;

namespace JiraDataPainter.Test
{
    internal class GraphVizDataPainterShould
    {
        private GraphVizDataPainter _painter = null!;

        [SetUp]
        public void SetUp()
        {
            _painter = new GraphVizDataPainter();
        }

        [Test]
        public void Paint_A_Jira_Issue_List()
        {
            // ReSharper disable once UseObjectOrCollectionInitializer
            var issues = new List<JiraIssue>();

            issues.Add(new JiraIssue("PROJ-1",
                            new JiraIssueFields("Testing 123 Task Without Epic",
                            new JiraIssueParentEpic(string.Empty),
                            new JiraIssueType("Task"))));

            issues.Add(new JiraIssue("PROJ-2",
                            new JiraIssueFields("Testing 123 Bug Without Epic",
                            new JiraIssueParentEpic(string.Empty),
                            new JiraIssueType("Bug"))));

            issues.Add(new JiraIssue("PROJ-3",
                            new JiraIssueFields("Testing 123 Story Without Epic",
                            new JiraIssueParentEpic(string.Empty),
                            new JiraIssueType("Story"))));

            issues.Add(new JiraIssue("PROJ-4",
                            new JiraIssueFields("Big important epic",
                            new JiraIssueParentEpic(string.Empty),
                            new JiraIssueType("Epic"))));

            issues.Add(new JiraIssue("PROJ-5",
                            new JiraIssueFields("Task in epic",
                            new JiraIssueParentEpic("PROJ-4"),
                            new JiraIssueType("Task"))));

            issues.Add(new JiraIssue("PROJ-6",
                            new JiraIssueFields("Bug in epic",
                            new JiraIssueParentEpic("PROJ-4"),
                            new JiraIssueType("Bug"))));

            issues.Add(new JiraIssue("PROJ-6",
                            new JiraIssueFields("Story in epic",
                            new JiraIssueParentEpic("PROJ-4"),
                            new JiraIssueType("Story"))));


            _painter.PaintData(issues.AsReadOnly());
        }
    }
}
