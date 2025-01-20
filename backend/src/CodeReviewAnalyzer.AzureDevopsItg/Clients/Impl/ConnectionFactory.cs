using CodeReviewAnalyzer.Application.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;

namespace CodeReviewAnalyzer.AzureDevopsItg.Clients;

public class ConnectionFactory : IConnectionFactory
{
    public IVssConnection CreateConnection(Configuration configuration)
    {
        return new VssConnection(
            new Uri($"https://dev.azure.com/{configuration.Organization}"),
            new VssBasicCredential(string.Empty, configuration.AccessToken));
    }
}
