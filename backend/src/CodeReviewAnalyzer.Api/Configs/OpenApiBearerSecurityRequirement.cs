using Microsoft.OpenApi.Models;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace CodeReviewAnalyzer.Api.Configs;

[Serializable]
[ExcludeFromCodeCoverage]
internal class OpenApiBearerSecurityRequirement : OpenApiSecurityRequirement
{
    public OpenApiBearerSecurityRequirement(OpenApiSecurityScheme securityScheme)
    {
        Add(securityScheme, new[] { "Bearer" });
    }

    protected OpenApiBearerSecurityRequirement(
        SerializationInfo serializationInfo,
        StreamingContext streamingContext)
    {
    }
}
