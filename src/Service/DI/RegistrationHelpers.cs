using Abstractions.Logic;
using Abstractions.State;
using Abstractions.Transport;
using Logic;
using Logic.Configuration.Validation;
using Logic.Configuration;
using Service.Services;
using Transport;
using Transport.Configuration;
using Models;
using Transport.Mapping;
using Transport.Configuration.Validation;

using Microsoft.Extensions.Options;

namespace Service.DI;

/// <summary>
/// DI Registration helpers.
/// </summary>
public static class RegistrationHelpers
{
    /// <summary>
    /// Register hosted services.
    /// </summary>
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddHostedService<SenderService>();
        services.AddHostedService<ResourceObserverService>();
    }

    /// <summary>
    /// Register resources state.
    /// </summary>
    public static void RegisterState(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IHealthChecksState, HealthChecksState>();
        services.Configure<HealthChecksStateConfiguration>(configuration.GetSection(nameof(HealthChecksStateConfiguration)));
        services.AddSingleton<IValidateOptions<HealthChecksStateConfiguration>, HealthChecksStateConfigurationValidator>();
        services.AddSingleton<Func<ResourceConfiguration, ResourceHealthCheck>>(conf =>
        {
            var name = new ResourceName(conf.Name);
            var settings = new ResourceRequestSettings(conf.Url, conf.CheckInterval, conf.Timeout);
            return new ResourceHealthCheck(name,
                                           conf.ExpirationPeriod,
                                           settings);
        });
    }

    /// <summary>
    /// Register resources observer logic.
    /// </summary>
    /// <param name="services"></param>
    public static void RegisterObserver(this IServiceCollection services)
    {
        services.AddTransient<IRawResourceChecker, HttpRawResourceChecker>();
        services.AddTransient<IResourceChecker, ResourceChecker>();
        services.AddSingleton<IResourcesObserver, ResourcesObserver>();
        services.AddSingleton<Func<ResourceHealthCheck, IResourceCheckerProcessor>>(sp =>
        {
            IResourceCheckerProcessor factory(ResourceHealthCheck res)
            {
                return ActivatorUtilities.CreateInstance<ResourceCheckerProcessor>(sp, res);
            }
            return factory;
        });
    }

    /// <summary>
    /// Register HttpClients.
    /// </summary>
    public static void RegisterHttpClient(this IServiceCollection services)
    {
        services.AddSingleton<IHttpClientProxy, HttpClientProxy>();
        services.AddSingleton(sp =>
        {
            var config = sp.GetRequiredService<IOptions<ReportSenderConfiguration>>().Value;
            // TODO Check If Validation Starts.
            return new HttpClient()
            {
                Timeout = config.Timeout
            };
        });
        services.AddSingleton<Func<TimeSpan, HttpClient>>(ts =>
        {
            return new HttpClient
            {
                Timeout = ts
            };
        });
    }

    /// <summary>
    /// Register healtcheck sender logic.
    /// </summary>
    public static void RegisterSender(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddSingleton<IReportProcessor, ReportProcessor>();
        services.AddSingleton<ISerializer<Transport.Models.HealthCheckReport, string>, HealthCheckReportSerializer>();
        services.AddSingleton<IReportSender, ReportSender>();
        services.AddSingleton<IRawSender<string>, HttpJsonSender>();
        services.AddAutoMapper(cfg => cfg.AddProfile<HealthCheckMappingProfile>());
        services.Configure<ReportSenderConfiguration>(configuration.GetSection(nameof(ReportSenderConfiguration)));
        services.AddSingleton<IValidateOptions<ReportSenderConfiguration>, ReportSenderConfigurationValidator>();
        services.Configure<ReportProcessorConfiguration>(configuration.GetSection(nameof(ReportProcessorConfiguration)));
        services.AddSingleton<IValidateOptions<ReportProcessorConfiguration>, ReportProcessorConfigurationValidator>();
    }

}