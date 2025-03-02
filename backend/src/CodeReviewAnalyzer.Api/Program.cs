using CodeReviewAnalyzer.Api.Configs;
using CodeReviewAnalyzer.Api.Extensions;
using CodeReviewAnalyzer.Api.Filters;
using CodeReviewAnalyzer.Database.Extensions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Serilog;
using System.Reflection;


LogConfigBuilder.AutoWire();
try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog();

    // Add services to the container.
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddVersionedApiExplorer();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddControllers(options =>
    {
        options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
    });
    builder.Services.ConfigureApplication();
    builder.Services.ConfigSwagger();
    builder.Services.Configure<RouteOptions>(options =>
    {
        options.LowercaseUrls = true;
        options.LowercaseQueryStrings = true;
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        var apiVersionDescription = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        var pathBase = Environment.GetEnvironmentVariable("ASPNETCORE_BASEURL");
        app.UsePathBase(pathBase);
        app.ConfigureSwagger(apiVersionDescription, pathBase);
    }

    app
        .UseSerilogRequestLogging()
        .UseRouting()
        .UseEndpoints(endpoints => endpoints.MapControllers());

    CodeReviewAnalyzerDatabaseExtensions.ExecuteMigration(app.Services);

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(
        exception: ex,
        messageTemplate: $"Failed to start the {Assembly.GetExecutingAssembly().GetName().Name}: {ex.Message}");
}
finally
{
    Log.CloseAndFlush();
}