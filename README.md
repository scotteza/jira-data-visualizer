# Jira Data Visualizer

Seeing if I can visualize Jira project dependencies using [Graphviz](https://graphviz.org/).

## How to run it

- Install the [dotnet framework](https://dotnet.microsoft.com/en-us/download/dotnet-framework) on your local machine.
- Install [GraphViz](https://graphviz.org/) on your local machine.
- Create file named `Token.txt` at the root of the repo with this format:
``` 
your-jira-domain
your-jira-username
a-jira-token-generated-here: https://id.atlassian.com/manage-profile/security/api-tokens
a-jira-ticket-key-that-you-have-access-to - used to test the connection
jql-query-1
jql-query-2
jql-query-3
jql-query-n
```
- Compile.
- Run the JiraDataVisualizer.Console project.

This will generate a GraphViz diagram for each JQL query in your `Token.txt` file.

## Disclaimer

I build this project for my own personal use, it is far from perfect.

If you need extra features, please PR changes you are interested in or drop an issue on GitHub's issue tracker.