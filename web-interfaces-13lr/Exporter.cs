using System.Diagnostics;
using OpenTelemetry;

namespace WebApplication13
{
    public class Exporter : BaseExporter<Activity>
    {
        public override ExportResult Export(in Batch<Activity> batch)
        {
            foreach (var activity in batch)
            {
                Console.WriteLine("Start Time: {0}", activity.StartTimeUtc);
                Console.WriteLine("Trace Id: {0}", activity.Context.TraceId);
                Console.WriteLine("Span Id: {0}", activity.Context.SpanId);
                Console.WriteLine("Trace Flags: {0}", activity.Context.TraceFlags);
                Console.WriteLine("Name: {0}", activity.DisplayName);
                Console.WriteLine("Kind: {0}", activity.Kind);
                Console.WriteLine("Attributes:");
                foreach (var attribute in activity.Tags)
                {
                    Console.WriteLine("\t{0}: {1}", attribute.Key, attribute.Value);
                }
                Console.WriteLine();
            }

            return ExportResult.Success;
        }

    }
}
