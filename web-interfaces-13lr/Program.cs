using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;


namespace WebApplication13
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var sampler = new TraceIdRatioBasedSampler(0.5);
            var processor = new BatchActivityExportProcessor(new Exporter());

            using var openTelemetry = Sdk.CreateTracerProviderBuilder()
                .AddSource("OpenTelemetry")
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("ASP.NET Web Server"))
                .SetSampler(sampler)
                .AddProcessor(processor)
                .AddOtlpExporter(otlpOptions =>
                {
                    otlpOptions.Endpoint = new Uri("http://localhost:4318");
                })
                .Build();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();

                });
    }

}
