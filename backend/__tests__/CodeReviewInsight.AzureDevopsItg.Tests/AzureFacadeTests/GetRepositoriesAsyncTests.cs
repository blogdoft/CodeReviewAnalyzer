using CodeReviewInsight.Domain.Features.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.Core.WebApi;
using NSubstitute.ExceptionExtensions;
using System.Data;

namespace CodeReviewInsight.AzureDevopsItg.Tests.AzureFacadeTests;

public class GetRepositoriesAsyncTests : BaseAzureFacadeTests
{
    [Fact]
    public async Task Should_ReturnRepositories_When_FacadeIsConfiguredAsync()
    {
        // Given
        TenantId tenantId = Guid.NewGuid();
        var facade = Build();

        // When
        var repositories = await facade
            .SetContext(tenantId, _validAzureDevOps)
            .FromProject(_project)
            .GetRepositoriesAsync();

        // Then
        repositories.ShouldNotBeNull();
        repositories.Count().ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task Should_ThrowNoNullAllowed_When_DoNotSetTenantIdAsync()
    {
        // Given
        var facade = Build();

        // When
        Func<Task> act = async () =>
        {
            var repo = await facade
                .FromProject(_project)
                .GetRepositoriesAsync();
            _ = repo.ToList();
        };

        // Then
        await act.ShouldThrowAsync<NoNullAllowedException>();
    }

    [Fact]
    public async Task Should_LogError_When_ProjectDoesNotExistAsync()
    {
        // Given
        ProjectHttpClient
            .GetProject(_project)
            .ThrowsAsync<ProjectDoesNotExistWithNameException>();
        var facade = Build();

        // When
        await facade
            .SetContext(Guid.NewGuid(), _validAzureDevOps)
            .FromProject(_project)
            .GetRepositoriesAsync();

        // Then
        Logger.ReceivedWithAnyArgs().Log(
            LogLevel.Error,
            0,
            Arg.Any<object>(),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }
}
