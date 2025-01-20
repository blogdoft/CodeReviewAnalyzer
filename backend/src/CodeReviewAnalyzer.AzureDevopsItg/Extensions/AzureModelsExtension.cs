using CodeReviewAnalyzer.Application.Integrations.Models;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.WebApi;

namespace CodeReviewAnalyzer.AzureDevopsItg.Extensions;

internal static class AzureModelsExtension
{
    public static User ToUser(this IdentityRef identifyRef) => new()
    {
        Id = identifyRef.Id,
        Name = identifyRef.DisplayName,
        Active = !identifyRef.Inactive,
    };

    public static User ToUser(this IdentityRefWithVote identityRef) => new()
    {
        Id = identityRef.Id,
        Name = identityRef.DisplayName,
        Active = !identityRef.Inactive,
    };
}
