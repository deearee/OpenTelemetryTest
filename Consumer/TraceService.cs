using Grpc.Core;
using OpenTelemetry.Proto.Collector.Trace.V1;

namespace Consumer;

public class TraceService(DataStore store) : OpenTelemetry.Proto.Collector.Trace.V1.TraceService.TraceServiceBase
{
    public override Task<ExportTraceServiceResponse> Export(ExportTraceServiceRequest request, ServerCallContext context)
    {
        Console.WriteLine(request);
        store.telemetryData.Add(request);
        
        foreach (var requestResourceSpan in request.ResourceSpans)
        {
            var allSpans = requestResourceSpan.ScopeSpans.SelectMany(scopeSpan => scopeSpan.Spans).ToList();

            if (store.shortestTrace == null || store.shortestTrace.Length > allSpans.Count)
                store.shortestTrace = new TraceWithLength(requestResourceSpan, allSpans.Count);
            
            if (store.longestTrace == null || store.longestTrace.Length < allSpans.Count)
                store.longestTrace = new TraceWithLength(requestResourceSpan, allSpans.Count);
            
            var duration = allSpans.Max(s => s.EndTimeUnixNano) - allSpans.Min(s => s.StartTimeUnixNano);

            if (store.fastestTrace == null || store.fastestTrace.Duration > duration)
                store.fastestTrace = new TraceWithDuration(requestResourceSpan, duration);

            if (store.slowestTrace == null || store.slowestTrace.Duration < duration)
                store.slowestTrace = new TraceWithDuration(requestResourceSpan, duration);
        }
        
        return Task.FromResult(new ExportTraceServiceResponse());
    }
}