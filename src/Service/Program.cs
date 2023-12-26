using Serilog;

using Service.DI;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks();
builder.Services.RegisterServices();
builder.Services.RegisterState(builder.Configuration);
builder.Services.RegisterObserver();
builder.Services.RegisterHttpClient();
builder.Services.RegisterSender(builder.Configuration);
builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration));
var app = builder.Build();
app.UseHealthChecks("/hc");
app.UseHttpsRedirection();

await app.RunAsync();

