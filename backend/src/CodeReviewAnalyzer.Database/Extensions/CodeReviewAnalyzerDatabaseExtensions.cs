using CodeReviewAnalyzer.Application.Reports;
using CodeReviewAnalyzer.Application.Repositories;
using CodeReviewAnalyzer.Database.Contexts;
using CodeReviewAnalyzer.Database.Contexts.Impl;
using CodeReviewAnalyzer.Database.Repositories;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Conventions;
using FluentMigrator.Runner.Initialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace CodeReviewAnalyzer.Database.Extensions;

public static class CodeReviewAnalyzerDatabaseExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services) =>
        services
            .AddSingleton<IConnectionFactory, NpgConnectionFactory>()
            .AddScoped<IDatabaseFacade, DapperDatabaseFacade>()
            .AddScoped<IConfigurations, ConfigurationsRepository>()
            .AddScoped<IPullRequests, PullRequestsRepository>()
            .AddScoped<IUsers, UserRepository>()
            .AddScoped<IDayOff, DayOffRepository>()
            .AddScoped<IReport, Report>()
            .AddScoped<ICodeRepository, CodeRepositoryRepository>()
            .ConfigureMigration();

    public static void ExecuteMigration(IServiceProvider provider)
    {
        using var scope = provider.CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>()
            .CreateLogger("migration");
        logger.LogInformation("Migration runner started at {MigrationStart}", DateTime.UtcNow);

        runner.MigrateUp();

        logger.LogInformation("Migration runner completed at {MigrationEnd}", DateTime.UtcNow);
    }

    private static IServiceCollection ConfigureMigration(this IServiceCollection services) =>
        services
            .AddFluentMigratorCore()
            .AddSingleton<IConventionSet>(provider =>
            {
                return new DefaultConventionSet(
                    defaultSchemaName: "public",
                    workingDirectory: "/");
            })
            .ConfigureRunner(runnerBuilder =>
            {
                runnerBuilder
                    .AddPostgres15_0()
                    .WithGlobalConnectionString(provider =>
                        GetConnectionString(provider))
                    .ScanIn(Assembly.GetExecutingAssembly())
                        .For.Migrations();
            })
            .Configure<RunnerOptions>(opt => opt.TransactionPerSession = false)
            .AddLogging(lb => lb.AddFluentMigratorConsole());

    private static string? GetConnectionString(IServiceProvider provider) =>
        provider.GetRequiredService<IConfiguration>()
            .GetConnectionString("Default");
}
