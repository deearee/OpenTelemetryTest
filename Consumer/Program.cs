using Consumer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddSingleton<DataStore>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<LogsService>();
app.MapGrpcService<MetricsService>();
app.MapGrpcService<TraceService>();

app.MapGet("/",
    (DataStore store) => Results.Json(store.telemetryData));

app.MapGet("/length",
    (DataStore store) => Results.Json(new object?[] {store.longestTrace, store.shortestTrace}));

app.MapGet("/speed",
    (DataStore store) => Results.Json(new object?[] {store.slowestTrace, store.fastestTrace}));

app.Run();