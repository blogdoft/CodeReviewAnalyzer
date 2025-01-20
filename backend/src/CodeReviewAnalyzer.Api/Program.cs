using CodeReviewAnalyzer.Api.Extensions;
using CodeReviewAnalyzer.Database.Extensions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddVersionedApiExplorer();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.ConfigureApplication();
builder.Services.ConfigSwagger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    var apiVersionDescription = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    var pathBase = Environment.GetEnvironmentVariable("ASPNETCORE_BASEURL");
    app.ConfigureSwagger(apiVersionDescription, pathBase);
}

app
    .UseRouting()
    .UseEndpoints(endpoints => endpoints.MapControllers());

CodeReviewAnalyzerDatabaseExtensions.ExecuteMigration(app.Services);

await app.RunAsync();
