using Serilog;
using Serilog.Events;
using Serilog.Templates;
using Serilog.Templates.Themes;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace CodeReviewAnalyzer.Api.Configs;

[ExcludeFromCodeCoverage]
public class LogConfigBuilder
{
    private readonly string logTemplate =
        "{ {date: @t, level: @l, message: @m, exception: @x, dd:{trace_id:@p['dd_trace_id'], span_id:@p['dd_span_id']}, ..@p} }" +
        Environment.NewLine;

    private readonly string _environment;

    public LogConfigBuilder(string environment)
    {
        _environment = environment;
    }

    public static void AutoWire()
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
            ?? "Production";

        var log = new LogConfigBuilder(environment);
        log.Build();
    }

    public void Build() =>
        Log.Logger = GetLoggerConfiguration();

    private static string GetVersion() => Assembly.GetExecutingAssembly()?
        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
        .InformationalVersion ?? "undefined";

    private Serilog.Core.Logger GetLoggerConfiguration() =>
        new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc.*", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker", LogEventLevel.Error)
            .MinimumLevel.Override("Serilog.AspNetCore.RequestLoggingMiddleware", ApplicationLogLevel())
            .Enrich.WithExceptionData()
            .Enrich.WithMachineName()
            .Enrich.WithProperty("AppVersion", GetVersion())
            .Enrich.WithProperty("Source", "csharp")
            .Enrich.FromLogContext()
            .Filter.ByExcluding("@l = 'Information' and RequestPath = '/api/health'")
            .WriteTo.Console(
                formatter: new ExpressionTemplate(logTemplate, theme: TemplateTheme.Code),
                restrictedToMinimumLevel: ApplicationLogLevel(),
                standardErrorFromLevel: LogEventLevel.Error)
            .CreateLogger();

    private LogEventLevel ApplicationLogLevel() =>
        _environment.Equals("Production", StringComparison.OrdinalIgnoreCase) ?
            LogEventLevel.Information :
            LogEventLevel.Debug;
}
