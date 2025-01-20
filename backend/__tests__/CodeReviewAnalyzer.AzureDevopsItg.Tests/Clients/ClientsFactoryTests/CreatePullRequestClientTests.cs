using CodeReviewAnalyzer.AzureDevopsItg.Clients;

namespace CodeReviewAnalyzer.AzureDevopsItg.Tests.Clients.ClientsFactoryTests;

public class CreatePullRequestClientTests
{
    public CreatePullRequestClientTests()
    {
        ConnectionFactory = Substitute.For<IConnectionFactory>();
    }

    public IConnectionFactory ConnectionFactory { get; }
}
