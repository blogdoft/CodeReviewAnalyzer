using System.Diagnostics.CodeAnalysis;

namespace CodeReviewInsight.Api.Extensions;

[ExcludeFromCodeCoverage]
public static class HeadersExtension
{
    public static string GetForwardedHost(this HttpRequest httpRequest)
    {
        var host = httpRequest.Headers["X-Forwarded-Host"];
        if (string.IsNullOrWhiteSpace(host))
        {
            host = httpRequest.Host.Value;
        }

        return host.ToString();
    }
}
