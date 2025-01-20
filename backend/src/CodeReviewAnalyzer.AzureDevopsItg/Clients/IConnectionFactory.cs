using CodeReviewAnalyzer.Application.Models;
using Microsoft.VisualStudio.Services.WebApi;

namespace CodeReviewAnalyzer.AzureDevopsItg.Clients;

public interface IConnectionFactory
{
    IVssConnection CreateConnection(Configuration configuration);
}
