using CodeReviewAnalyzer.Application.Integrations;
using CodeReviewAnalyzer.AzureDevopsItg.Clients;
using CodeReviewAnalyzer.AzureDevopsItg.Clients.Impl;
using CodeReviewAnalyzer.AzureDevopsItg.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CodeReviewAnalyzer.AzureDevopsItg.Extensions;

public static class AzureDevopsItgExtension
{
    public static IServiceCollection AddAzureDevopsItg(this IServiceCollection services) =>
        services
            .AddTransient<IConnectionFactory, ConnectionFactory>()
            .AddScoped<IPullRequestsClient, PullRequestClient>()
            .AddScoped<IWorkItemsIntegration, WorkItemIntegration>();
}
