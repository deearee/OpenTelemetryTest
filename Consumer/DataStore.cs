using OpenTelemetry.Proto.Trace.V1;

namespace Consumer;

public record TraceWithLength(ResourceSpans Trace, int Length);

public record TraceWithDuration(ResourceSpans Trace, ulong Duration);

public class DataStore
{
    public List<object> telemetryData = [];
    
    public TraceWithLength? longestTrace;
    public TraceWithLength? shortestTrace;
    
    public TraceWithDuration? fastestTrace;
    public TraceWithDuration? slowestTrace;
}