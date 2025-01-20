using CodeReviewAnalyzer.Application.Extensions;
using CodeReviewAnalyzer.AzureDevopsItg.Extensions;
using CodeReviewAnalyzer.Database.Extensions;

namespace CodeReviewAnalyzer.Api.Extensions;

public static class CodeReviewAnalyzerApiExtensions
{
    public static IServiceCollection ConfigureApplication(this IServiceCollection services)
    {
        return services
            .AddDatabase()
            .AddAzureDevopsItg()
            .AddApplication();
    }
}
