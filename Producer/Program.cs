using System.Diagnostics;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

const string serviceName = "otel-test";
const string activitySourceName = "SampleActivity";

builder.Logging.AddOpenTelemetry(options =>
{
    options
        .SetResourceBuilder(
            ResourceBuilder.CreateDefault()
                .AddService(serviceName))
        .AddOtlpExporter();
});

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService(serviceName))
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation()
        .AddSource(activitySourceName)
        .AddOtlpExporter())
    .WithMetrics(metrics => metrics
        .AddAspNetCoreInstrumentation()
        .AddOtlpExporter());

var activitySource = new ActivitySource(activitySourceName, "1.0.0");

var app = builder.Build();

string HandleTest(string? player)
{
    var spanCount = Random.Shared.Next(1, 20);
    List<Activity?> activities = [];

    for (var i = 0; i < spanCount; i++)
    {
        activities.Add(activitySource.StartActivity($"span{i}"));
        Thread.Sleep(Random.Shared.Next(1, 100));
    }

    foreach (var a in activities)
        a?.Stop();
    
    return "OK";
}

app.MapGet("/test", HandleTest);

app.Run();