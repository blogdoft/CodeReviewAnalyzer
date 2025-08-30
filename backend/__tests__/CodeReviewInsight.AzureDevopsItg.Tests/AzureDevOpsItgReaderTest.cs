using CodeReviewInsight.Application.Features.AzureDevOpsIntegration;
using CodeReviewInsight.Domain.Features.Configurations.Entities;
using CodeReviewInsight.Domain.Features.GitRepositories;

namespace CodeReviewInsight.AzureDevopsItg.Tests;

public class AzureDevOpsItgReaderTest
{
    public AzureDevOpsItgReaderTest()
    {
        RepositoryResolver = Substitute.For<IRepositoryResolver>();
        GitRepository = Substitute.For<IGitRepository>();
    }

    public IRepositoryResolver RepositoryResolver { get; }

    public IGitRepository GitRepository { get; }

    [Fact]
    public async Task Should_RetrieveAndSalveRepositories_When_ProjectHasRepositoriesAsync()
    {
        // Given
        var azureDevOps = new AzureDevOps(
            name: "My Inc. DevOps",
            devOpsUrl: new Uri("https://dev.azure.com/blog-do-ft"),
            pat: "313e12a7-52a4-4598-b9e5-47ca6dbbe957",
            projects: ["Project1", "Project2"],
            areas: ["Areas1", "Areas2"],
            active: true);
        var devopsReader = BuildAzureDevOpsItgReader();

        // When
        await devopsReader.ProcessAsync(azureDevOps);

        // Then
        await RepositoryResolver.Received(2).GetRepositoriesFromAsync(Arg.Any<string>());
        await GitRepository.Received(2).UpsertRepositoriesAsync(Arg.Any<IEnumerable<GitRepository>>());
    }

    private AzureDevOpsItgReader BuildAzureDevOpsItgReader() =>
        new(RepositoryResolver, GitRepository);
}
