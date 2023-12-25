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
builder.Services.AddHealthChecks();
builder.Services.AddHostedService<SenderService>();
builder.Services.AddHostedService<ResourceObserverService>();
builder.Services.AddSingleton<IHealthChecksState, HealthChecksState>();
builder.Services.AddSingleton<ISerializer<Transport.Models.HealthCheckReport, string>, HealthCheckReportSerializer>();
builder.Services.AddSingleton<IReportSender, ReportSender>();
builder.Services.AddSingleton<IHttpClientProxy, HttpClientProxy>();
builder.Services.AddSingleton<IRawSender<string>, HttpJsonSender>();
builder.Services.AddSingleton<IResourcesObserver, ResourcesObserver>();
builder.Services.AddSingleton<IReportProcessor, ReportProcessor>();
builder.Services.AddTransient<IRawResourceChecker, HttpRawResourceChecker>();
builder.Services.AddTransient<IResourceChecker, ResourceChecker>();
builder.Services.Configure<ReportSenderConfiguration>(builder.Configuration.GetSection(nameof(ReportSenderConfiguration)));
builder.Services.AddSingleton<IValidateOptions<ReportSenderConfiguration>, ReportSenderConfigurationValidator>();
builder.Services.Configure<ReportProcessorConfiguration>(builder.Configuration.GetSection(nameof(ReportProcessorConfiguration)));
builder.Services.AddSingleton<IValidateOptions<ReportProcessorConfiguration>, ReportProcessorConfigurationValidator>();
builder.Services.Configure<HealthChecksStateConfiguration>(builder.Configuration.GetSection(nameof(HealthChecksStateConfiguration)));
builder.Services.AddSingleton<IValidateOptions<HealthChecksStateConfiguration>, HealthChecksStateConfigurationValidator>();
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<HealthCheckMappingProfile>());
builder.Services.AddSingleton(sp =>
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
builder.Services.AddSingleton<Func<ResourceConfiguration, ResourceHealthCheck>>(conf =>
{
    var name = new ResourceName(conf.Name);
    var settings = new ResourceRequestSettings(conf.Uri, conf.CheckInterval, conf.Timeout);
    return new ResourceHealthCheck(name,
                                   conf.ExpirationPeriod,
                                   settings);
});
builder.Services.AddSingleton<Func<ResourceHealthCheck, IResourceCheckerProcessor>>(sp =>
{
    IResourceCheckerProcessor factory(ResourceHealthCheck res)
    {
        return ActivatorUtilities.CreateInstance<ResourceCheckerProcessor>(sp, res);
    }
    return factory;
});
builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration));

var app = builder.Build();
app.UseHealthChecks("/hc");
app.UseHttpsRedirection();

await app.RunAsync();

