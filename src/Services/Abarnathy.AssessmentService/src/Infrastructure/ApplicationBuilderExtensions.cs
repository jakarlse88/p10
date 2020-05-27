using Microsoft.AspNetCore.Builder;

namespace Abarnathy.AssessmentService.Infrastructure
{
    internal static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Configure Swagger UI.
        /// </summary>
        /// <param name="app"></param>
        internal static IApplicationBuilder UseSwaggerUI(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Abarnathy Assessment Service API 1.0");
                c.RoutePrefix = string.Empty;
            });

            return app;
        }       
    }
}