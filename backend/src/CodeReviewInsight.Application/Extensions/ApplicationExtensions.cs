using CodeReviewInsight.Application.Services;
using CodeReviewInsight.Application.Services.Crawlers;
using CodeReviewInsight.Application.Services.Crawlers.Impl;
using CodeReviewInsight.Application.Services.Processors;
using CodeReviewInsight.Application.Services.Processors.Impl;
using CodeReviewInsight.Application.Services.Teams;
using CodeReviewInsight.Application.Services.Teams.Impl;
using CodeReviewInsight.Application.TenantFeature.Services;
using CodeReviewInsight.Application.TenantFeature.Services.Impl;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace CodeReviewInsight.Application.Extensions;

[ExcludeFromCodeCoverage]
public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services) =>
        services
            .AddScoped<PullRequestMetadataProcessor>()
            .AddScoped<ICreateTeam, CreateTeam>()
            .AddScoped<IWorkItemsCrawler, WorkItemsCrawler>()
            .AddScoped<ITenantAdd, TenantAdd>()
            .AddScoped<ITenantUpdate, TenantUpdate>()
            .AddScoped<TenantProcessor>()
            .AddScoped<IDataSourceProcessor, DataSourceProcessor>();
}
