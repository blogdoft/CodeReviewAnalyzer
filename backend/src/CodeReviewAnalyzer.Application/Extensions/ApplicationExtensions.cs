using CodeReviewAnalyzer.Application.Services;
using CodeReviewAnalyzer.Application.Services.Teams;
using CodeReviewAnalyzer.Application.Services.Teams.Impl;
using Microsoft.Extensions.DependencyInjection;

namespace CodeReviewAnalyzer.Application.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services) =>
        services
            .AddScoped<PullRequestMetadataProcessor>()
            .AddScoped<ICreateTeam, CreateTeam>();
}
