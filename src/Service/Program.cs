using Abstractions.Logic;
using Abstractions.State;
using Abstractions.Transport;
using Service.Services;
using Transport.Configuration;
using Transport.Configuration.Validation;
using Transport.Mapping;
using Transport;
using Logic.Configuration;
using Logic.Configuration.Validation;
using Logic;
using Models;

using Microsoft.Extensions.Options;

using Serilog;

var builder = WebApplication.CreateBuilder(args);
_ = builder.Services.AddHealthChecks();
_ = builder.Services.AddHostedService<SenderService>();
_ = builder.Services.AddHostedService<ResourceObserverService>();
_ = builder.Services.AddSingleton<IHealthChecksState, HealthChecksState>();
_ = builder.Services.AddSingleton<ISerializer<Transport.Models.HealthCheckReport, string>, HealthCheckReportSerializer>();
_ = builder.Services.AddSingleton<IReportSender, ReportSender>();
_ = builder.Services.AddSingleton<IHttpClientProxy, HttpClientProxy>();
_ = builder.Services.AddSingleton<IRawSender<string>, HttpJsonSender>();
_ = builder.Services.AddSingleton<IResourcesObserver, ResourcesObserver>();
_ = builder.Services.AddSingleton<IReportProcessor, ReportProcessor>();
_ = builder.Services.AddTransient<IRawResourceChecker, HttpRawResourceChecker>();
_ = builder.Services.AddTransient<IResourceChecker, ResourceChecker>();
_ = builder.Services.Configure<ReportSenderConfiguration>(builder.Configuration.GetSection(nameof(ReportSenderConfiguration)));
_ = builder.Services.AddSingleton<IValidateOptions<ReportSenderConfiguration>, ReportSenderConfigurationValidator>();
_ = builder.Services.Configure<ReportProcessorConfiguration>(builder.Configuration.GetSection(nameof(ReportProcessorConfiguration)));
_ = builder.Services.AddSingleton<IValidateOptions<ReportProcessorConfiguration>, ReportProcessorConfigurationValidator>();
_ = builder.Services.Configure<HealthChecksStateConfiguration>(builder.Configuration.GetSection(nameof(HealthChecksStateConfiguration)));
_ = builder.Services.AddSingleton<IValidateOptions<HealthChecksStateConfiguration>, HealthChecksStateConfigurationValidator>();
_ = builder.Services.AddAutoMapper(cfg => cfg.AddProfile<HealthCheckMappingProfile>());
_ = builder.Services.AddSingleton(sp =>
    {
        var config = sp.GetRequiredService<IOptions<ReportSenderConfiguration>>().Value;
        // TODO Check If Validation Starts.
        return new HttpClient()
        {
            Timeout = config.Timeout
        };
    });
_ = builder.Services.AddSingleton<Func<TimeSpan, HttpClient>>(ts =>
{
    return new HttpClient
    {
        Timeout = ts
    };
});
_ = builder.Services.AddSingleton<Func<ResourceConfiguration, ResourceHealthCheck>>(conf =>
{
    var name = new ResourceName(conf.Name);
    var settings = new ResourceRequestSettings(conf.Uri, conf.CheckInterval, conf.Timeout);
    return new ResourceHealthCheck(name,
                                   conf.ExpirationPeriod,
                                   settings);
});
_ = builder.Services.AddSingleton<Func<ResourceHealthCheck, IResourceCheckerProcessor>>(sp =>
{
    IResourceCheckerProcessor factory(ResourceHealthCheck res)
    {
        return ActivatorUtilities.CreateInstance<ResourceCheckerProcessor>(sp, res);
    }
    return factory;
});
_ = builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration));

//appsettins.json

var app = builder.Build();
_ = app.UseHealthChecks("/hc");
_ = app.UseHttpsRedirection();

await app.RunAsync();

