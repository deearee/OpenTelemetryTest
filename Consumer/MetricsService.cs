using Grpc.Core;
using OpenTelemetry.Proto.Collector.Metrics.V1;

namespace Consumer;

public class MetricsService(DataStore store) : OpenTelemetry.Proto.Collector.Metrics.V1.MetricsService.MetricsServiceBase
{
    public override Task<ExportMetricsServiceResponse> Export(ExportMetricsServiceRequest request, ServerCallContext context)
    {
        Console.WriteLine(request);
        store.telemetryData.Add(request);
        return Task.FromResult(new ExportMetricsServiceResponse());
    }
}