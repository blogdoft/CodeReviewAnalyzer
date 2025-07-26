using CodeReviewAnalyzer.Application.Services;
using CodeReviewAnalyzer.Application.Services.Crawlers;
using CodeReviewAnalyzer.Application.Services.Crawlers.Impl;
using CodeReviewAnalyzer.Application.Services.Teams;
using CodeReviewAnalyzer.Application.Services.Teams.Impl;
using CodeReviewAnalyzer.Application.TenantFeature.Services;
using CodeReviewAnalyzer.Application.TenantFeature.Services.Impl;
using Microsoft.Extensions.DependencyInjection;

namespace CodeReviewAnalyzer.Application.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services) =>
        services
            .AddScoped<PullRequestMetadataProcessor>()
            .AddScoped<ICreateTeam, CreateTeam>()
            .AddScoped<IWorkItemsCrawler, WorkItemsCrawler>()
            .AddScoped<ITenantAdd, TenantAdd>()
            .AddScoped<ITenantUpdate, TenantUpdate>();
}
