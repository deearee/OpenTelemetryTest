using Grpc.Core;
using OpenTelemetry.Proto.Collector.Logs.V1;

namespace Consumer;

public class LogsService(DataStore store) : OpenTelemetry.Proto.Collector.Logs.V1.LogsService.LogsServiceBase
{
    public override Task<ExportLogsServiceResponse> Export(ExportLogsServiceRequest request, ServerCallContext context)
    {
        Console.WriteLine(request);
        store.telemetryData.Add(request);
        return Task.FromResult(new ExportLogsServiceResponse());
    }
}