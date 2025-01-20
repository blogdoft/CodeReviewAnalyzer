using CodeReviewAnalyzer.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CodeReviewAnalyzer.Application.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services) =>
        services
            .AddScoped<PullRequestMetadataProcessor>();
}
