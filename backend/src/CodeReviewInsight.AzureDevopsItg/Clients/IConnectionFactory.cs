using CodeReviewInsight.Application.Models;
using Microsoft.VisualStudio.Services.WebApi;

namespace CodeReviewInsight.AzureDevopsItg.Clients;

public interface IConnectionFactory
{
    IVssConnection CreateConnection(Configuration configuration);
}
