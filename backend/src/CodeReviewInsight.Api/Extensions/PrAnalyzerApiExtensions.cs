using CodeReviewInsight.Application.Extensions;
using CodeReviewInsight.AzureDevopsItg.Extensions;
using CodeReviewInsight.Database.Extensions;

namespace CodeReviewInsight.Api.Extensions;

public static class CodeReviewInsightApiExtensions
{
    public static IServiceCollection ConfigureApplication(this IServiceCollection services)
    {
        return services
            .AddDatabase()
            .AddAzureDevopsItg()
            .AddApplication();
    }
}
