using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Abarnathy.DemographicsAPI.Infrastructure;
using Abarnathy.DemographicsAPI.Repositories;
using AutoMapper;
using ApplicationBuilderExtensions = Abarnathy.DemographicsAPI.Infrastructure.ApplicationBuilderExtensions;

namespace Abarnathy.DemographicsAPI
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
            ApplicationBuilderExtensions.UseForwardedHeaders(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (env.EnvironmentName != "Test")
            {
                app.ApplyMigrations();
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