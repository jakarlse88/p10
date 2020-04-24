using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Abarnathy.DemographicsAPI.Infrastructure;
using Abarnathy.DemographicsAPI.Infrastructure.ActionFilters;
using AutoMapper;
using FluentValidation.AspNetCore;
using ApplicationBuilderExtensions = Abarnathy.DemographicsAPI.Infrastructure.ApplicationBuilderExtensions;

namespace Abarnathy.DemographicsAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureControllers();

            services.ConfigureDbContext(Configuration);

            services.ConfigureSwagger();

            services.AddAutoMapper(typeof(Startup));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ApplicationBuilderExtensions.UseForwardedHeaders(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionHandler();

            app.ApplyMigrations();

            app.UseSwaggerUI();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}