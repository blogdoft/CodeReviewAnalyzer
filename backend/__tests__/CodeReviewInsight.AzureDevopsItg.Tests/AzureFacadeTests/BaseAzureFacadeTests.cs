using CodeReviewInsight.AzureDevopsItg.Clients;
using CodeReviewInsight.Domain.Features.Configurations.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;

namespace CodeReviewInsight.AzureDevopsItg.Tests.AzureFacadeTests;

public class BaseAzureFacadeTests
{
    protected readonly Faker _faker = BogusFixture.Get();
    protected readonly string _project;
    protected readonly AzureDevOps _validAzureDevOps;

    public BaseAzureFacadeTests()
    {
        _project = "code-review-insight";
        _validAzureDevOps = new AzureDevOps(
            name: "Test",
            devOpsUrl: new Uri("https://dev.azure.com/blog-do-ft"),
            pat: _faker.Internet.Password(),
            projects: [_project],
            areas: []);
        Logger = Substitute.For<ILogger<AzureFacade>>();
        ConnectionFactory = Substitute.For<IConnectionFactory>();
        Connection = Substitute.For<IVssConnection>();
        ProjectHttpClient = Substitute.For<ProjectHttpClient>(
            _validAzureDevOps.DevOpsUrl,
            new VssCredentials());
        GitHttpClient = Substitute.For<GitHttpClient>(
            _validAzureDevOps.DevOpsUrl,
            new VssCredentials());

        ConnectionFactory
            .CreateConnection(Arg.Any<Uri>(), Arg.Any<string>())
            .Returns(Connection);
        Connection
            .GetClientAsync<ProjectHttpClient>()
            .Returns(ProjectHttpClient);
        Connection
            .GetClientAsync<GitHttpClient>()
            .Returns(GitHttpClient);
        GitHttpClient
            .GetRepositoriesAsync(Arg.Any<Guid>(), includeHidden: false)
            .Returns(new AutoFaker<GitRepository>()
                .RuleFor(gr => gr.RemoteUrl, (fk, _) => fk.Internet.Url())
                .Generate(2));
        ProjectHttpClient
            .GetProject(_project)
            .Returns(new AutoFaker<TeamProject>());
    }

    protected IConnectionFactory ConnectionFactory { get; }
    protected IVssConnection Connection { get; }
    protected ProjectHttpClient ProjectHttpClient { get; }
    protected GitHttpClient GitHttpClient { get; }
    protected ILogger<AzureFacade> Logger { get; }

    protected AzureFacade Build() => new(Logger, ConnectionFactory);
}
