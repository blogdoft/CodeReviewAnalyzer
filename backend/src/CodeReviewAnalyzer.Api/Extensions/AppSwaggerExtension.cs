using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using System.Diagnostics.CodeAnalysis;

namespace CodeReviewAnalyzer.Api.Extensions;

[ExcludeFromCodeCoverage]
public static class AppSwaggerExtension
{
    public static IApplicationBuilder ConfigureSwagger(
        this WebApplication app,
        IApiVersionDescriptionProvider provider,
        string? pathBase) =>
        app
            .UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    var host = httpReq.GetForwardedHost();

                    swaggerDoc.Servers = new List<OpenApiServer>
                    {
                        new OpenApiServer
                        {
                            Url = $"https://{host}{pathBase ?? "/"}",
                        },
                        new OpenApiServer
                        {
                            Url = $"http://{host}{pathBase ?? "/"}",
                        },
                    };
                });
            })
            .UseSwaggerUI(o => provider.ApiVersionDescriptions
                .ToList()
                .ForEach(d =>
                    o.SwaggerEndpoint($"{pathBase}/swagger/{d.GroupName}/swagger.json", d.GroupName.ToUpper())));
}
#pragma warning restore S5332 // Using http protocol is insecure. Use https instead.
