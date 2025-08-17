using BlogDoFT.Libs.DapperUtils.Postgres;
using CodeReviewAnalyzer.Application.Reports;
using CodeReviewAnalyzer.Application.Repositories;
using CodeReviewAnalyzer.Application.TenantFeature;
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
            .AddDapperPostgres()
            .AddScoped<IConfigurations, ConfigurationsRepository>()
            .AddScoped<IPullRequests, PullRequestsRepository>()
            .AddScoped<IUsers, PeopleRepository>()
            .AddScoped<IDayOff, DayOffRepository>()
            .AddScoped<IReport, Report>()
            .AddScoped<ICodeRepository, CodeRepositoryRepository>()
            .AddScoped<ITeams, TeamRepository>()
            .AddScoped<ITeamUser, TeamUserRepository>()
            .AddScoped<IWorkItems, WorkItemsRepository>()
            .AddScoped<ITenantRepository, TenantRepository>()
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
            .Configure<RunnerOptions>(opt => opt.TransactionPerSession = true)
            .AddLogging(lb => lb.AddFluentMigratorConsole());

    private static string? GetConnectionString(IServiceProvider provider) =>
        provider.GetRequiredService<IConfiguration>()
            .GetConnectionString("Default");
}
