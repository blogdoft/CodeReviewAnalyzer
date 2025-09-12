using CodeReviewInsight.Application.Models;

using CodeReviewInsight.Domain.Features.Configurations;
using Microsoft.TeamFoundation.SourceControl.WebApi;

namespace CodeReviewInsight.AzureDevopsItg.Tests.AzureFacadeTests;

public class GetPullRequestsAsyncTests : BaseAzureFacadeTests
{
    [Fact]
    public async Task Should_ReturnPullRequests_When_FacadeIsConfiguredAsync()
    {
        // Given
        TenantId tenantId = Guid.NewGuid();
        var facade = Build();

        // When
        var pullRequests = await facade
            .SetContext(tenantId, _validAzureDevOps)
            .FromProject(_project)
            .GetPullRequestsAsync(
                startPeriod: DateTime.UtcNow.AddMonths(-3),
                endPeriod: DateTime.UtcNow);

        // Then
        pullRequests.ShouldNotBeNull();
    }

    [Fact]
    public async Task Should_NoLoadPrOnRepository_When_ItIsDisabledAsync()
    {
        // Given
        TenantId tenantId = Guid.NewGuid();
        GitHttpClient
            .GetRepositoriesAsync(
                Arg.Any<Guid>(),
                includeHidden: false,
                cancellationToken: TestContext.Current.CancellationToken)
            .ReturnsForAnyArgs(new AutoFaker<GitRepository>()
                .RuleFor(gr => gr.RemoteUrl, (fk, _) => fk.Internet.Url())
                .RuleFor(gr => gr.IsDisabled, true)
                .Generate(2));
        var facade = Build();

        // When
        var pullRequests = await facade
            .SetContext(tenantId, _validAzureDevOps)
            .FromProject(_project)
            .GetPullRequestsAsync(
                startPeriod: DateTime.UtcNow.AddMonths(-3),
                endPeriod: DateTime.UtcNow);

        // Then
        await GitHttpClient.DidNotReceiveWithAnyArgs()
            .GetPullRequestsAsync(
                Arg.Any<Guid>(),
                Arg.Any<GitPullRequestSearchCriteria>(),
                cancellationToken: TestContext.Current.CancellationToken);
    }
}
