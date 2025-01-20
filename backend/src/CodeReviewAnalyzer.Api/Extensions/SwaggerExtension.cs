using CodeReviewAnalyzer.Api.Configs;
using CodeReviewAnalyzer.Api.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace CodeReviewAnalyzer.Api.Extensions;

[ExcludeFromCodeCoverage]
public static class SwaggerExtension
{
    private const string OperationId = "{0}_{1}_{2}_{3}";

    public static IServiceCollection ConfigSwagger(this IServiceCollection services) => services
        .AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
        })
        .AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        })
        .AddSwaggerGen(options =>
        {
            options.DescribeAllParametersInCamelCase();
            options.CustomSchemaIds(type => type.FullName);
            options.CustomOperationIds(operation => string.Format(
                OperationId,
                operation.ActionDescriptor.RouteValues["controller"] ?? string.Empty,
                operation.ActionDescriptor.RouteValues["action"] ?? string.Empty,
                operation.HttpMethod ?? string.Empty,
                operation.ActionDescriptor.AttributeRouteInfo?.Order ?? 0));

            options.LoadDocumentationFiles();
            options.ExampleFilters();
            options.OperationFilter<SwaggerDefaultValues>();
            options.OperationFilter<AddResponseHeadersFilter>();

            OpenApiSecurityScheme securityScheme = new OpenApiBearerSecurityScheme();
            OpenApiSecurityRequirement securityRequirement = new OpenApiBearerSecurityRequirement(securityScheme);
            options.AddSecurityDefinition("Bearer", securityScheme);
            options.AddSecurityRequirement(securityRequirement);

            options.EnableAnnotations();
            options.UseAllOfToExtendReferenceSchemas();
        })
        .AddSwaggerExamplesFromAssemblies(Assembly.GetExecutingAssembly())
        .AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

    private static void LoadDocumentationFiles(this SwaggerGenOptions options)
    {
        var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly);
        foreach (var xml in xmlFiles)
        {
            options.IncludeXmlComments(xml);
        }
    }
}
