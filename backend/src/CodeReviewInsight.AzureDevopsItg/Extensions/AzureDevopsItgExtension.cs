using CodeReviewInsight.Application.Integrations;
using CodeReviewInsight.Application.Services.Crawlers.AzureCrawlers;
using CodeReviewInsight.AzureDevopsItg.Clients;
using CodeReviewInsight.AzureDevopsItg.Clients.Impl;
using CodeReviewInsight.AzureDevopsItg.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CodeReviewInsight.AzureDevopsItg.Extensions;

public static class AzureDevopsItgExtension
{
    public static IServiceCollection AddAzureDevopsItg(this IServiceCollection services) =>
        services
            .AddTransient<IConnectionFactory, ConnectionFactory>()
            .AddScoped<IPullRequestsClient, PullRequestClient>()
            .AddScoped<IWorkItemsIntegration, WorkItemIntegration>()
            .AddScoped<IAzureFacade, AzureFacade>();
}
