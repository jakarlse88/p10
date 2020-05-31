using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Abarnathy.DemographicsService.Infrastructure;
using Abarnathy.DemographicsService.Repositories;
using AutoMapper;

namespace Abarnathy.DemographicsService
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;
        
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            _environment = environment;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        { 
            services.ConfigureDbContext(Configuration, _environment);
            services.ConfigureControllers();
            services.ConfigureSwagger();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.ConfigureLocalServices();
            services.AddAutoMapper(typeof(Startup));
            services.ConfigureCors();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Infrastructure.ApplicationBuilderExtensions.UseForwardedHeaders(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCustomExceptionHandler();
            app.UseSwaggerUI();
            app.UseCors();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}