using System;
using Abarnathy.HistoryAPI.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Abarnathy.HistoryAPI.Infrastructure;
using Abarnathy.HistoryAPI.Services;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using Serilog;

namespace Abarnathy.HistoryAPI
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            ConventionRegistry.Register("CamelCase", new ConventionPack { new CamelCaseElementNameConvention() }, _ => true);

            services.AddSingleton<IMongoClient>(s =>
                new MongoClient(Configuration["PatientHistoryDatabaseSettings:ConnectionString"]));

            services.AddScoped(s => new PatientHistoryDbContext(s.GetRequiredService<IMongoClient>(),
                Configuration["PatientHistoryDatabaseSettings:DatabaseName"]));

            services.AddTransient<NoteService>();

            services.ConfigureControllers();
            // services.ConfigureDbContext(Configuration);
            // services.ConfigureLocalServices();
            services.ConfigureSwagger();
            services.ConfigureCors();
            services.AddAuthorization();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCustomExceptionHandlerExtension();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseCors();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}