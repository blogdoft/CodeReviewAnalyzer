using CodeReviewInsight.Application.Features.AzureDevOpsIntegration;
using CodeReviewInsight.Domain.Features.Configurations.Entities;

namespace CodeReviewInsight.AzureDevopsItg;

public class AzureDevOpsItgReader
{
    private readonly IRepositoryResolver _repositoryResolver;
    private readonly IGitRepository _gitRepository;

    public AzureDevOpsItgReader(
        IRepositoryResolver repositoryResolver,
        IGitRepository gitRepository)
    {
        _repositoryResolver = repositoryResolver;
        _gitRepository = gitRepository;
    }

    internal async Task ProcessAsync(AzureDevOps azureDevOps)
    {
        foreach (var project in azureDevOps.Projects)
        {
            var repositories = await _repositoryResolver.GetRepositoriesFromAsync(project);
            await _gitRepository.UpsertRepositoriesAsync(repositories);
        }
    }
}
