using Microsoft.OpenApi.Models;
using OpenTelemetry;

using OpenTelemetry.Metrics;
using System.Diagnostics.Metrics;


namespace WebApplication13
{
    public class Startup
    {
        private readonly Meter _meter;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _meter = new Meter("meter");
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "lr13", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapGet("/metrics", async context =>
                {
                    var counter = _meter.CreateCounter<long>("requests_count");
                    counter.Add(1);
                    await context.Response.WriteAsync("Metrics endpoint accessed.");
                });
            });
        }
    }
}