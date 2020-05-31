using System.Linq;
using Abarnathy.AssessmentService.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Abarnathy.AssessmentService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .ConfigureSwagger()
                .ConfigureLocalServices(Configuration)
                .ConfigureCors()
                .AddControllers(options =>
                {
                    var noContentFormatter =
                        options.OutputFormatters.OfType<HttpNoContentOutputFormatter>().FirstOrDefault();

                    if (noContentFormatter != null)
                    {
                        noContentFormatter.TreatNullValueAsNoContent = false;
                    }
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app
                .UseSwaggerUI()
                .UseRouting()
                .UseAuthorization()
                .UseCors()
                .UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}