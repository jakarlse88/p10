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
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureControllers();

            services.ConfigureDbContext(Configuration);

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
            
            app.UseCustomExceptionHandler();

            app.ApplyMigrations();

            app.UseSwaggerUI();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}