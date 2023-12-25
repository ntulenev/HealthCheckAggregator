using Service.Services;
using Transport.Configuration;
using Logic.Configuration;
using Logic;
using Abstractions.State;

using Microsoft.Extensions.Options;

using Serilog;
using Abstractions.Transport;
using Transport;

var builder = WebApplication.CreateBuilder(args);
_ = builder.Services.AddHealthChecks();
_ = builder.Services.AddHostedService<SenderService>();
_ = builder.Services.AddHostedService<ResourceObserverService>();
_ = builder.Services.AddSingleton<IHealthChecksState, HealthChecksState>();
_ = builder.Services.AddSingleton<ISerializer<Transport.Models.HealthCheckReport, string>, HealthCheckReportSerializer>();
_ = builder.Services.AddSingleton<IReportSender, ReportSender>();
_ = builder.Services.AddSingleton<IRawSender<string>, HttpJsonSender>();
_ = builder.Services.Configure<ReportSenderConfiguration>(builder.Configuration.GetSection(nameof(ReportSenderConfiguration)));
_ = builder.Services.AddSingleton<IValidateOptions<ReportSenderConfiguration>, ReportSenderConfigurationValidator>();
_ = builder.Services.Configure<ReportProcessorConfiguration>(builder.Configuration.GetSection(nameof(ReportProcessorConfiguration)));
_ = builder.Services.AddSingleton<IValidateOptions<ReportProcessorConfiguration>, ReportProcessorConfigurationValidator>();
_ = builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration));

//TODO IHttpClientProxy
//IRawResourceChecker
//IHealthChecksState
//IResourceCheckerProcessor
//IResoueceChecker
//Factory methods

var app = builder.Build();
_ = app.UseHealthChecks("/hc");
_ = app.UseHttpsRedirection();

await app.RunAsync();

