using Greggs.Products.Api.DataAccess; // Namespace containing data access related classes/interfaces
using Greggs.Products.Api.Models; // Contains models representing entities in the product domain
using Greggs.Products.Api.Services; // Namespace containing service interfaces and implementations
using Microsoft.AspNetCore.Builder; // Provides the IApplicationBuilder interface for configuring an application's request pipeline
using Microsoft.AspNetCore.Hosting; // Provides a hosting environment for the application
using Microsoft.Extensions.DependencyInjection; // Provides extension methods for configuring services
using Microsoft.Extensions.Hosting; // Provides host-related extension methods

namespace Greggs.Products.Api
{
    public class Startup
    {
        // Configures services for the application
        public void ConfigureServices(IServiceCollection services)
        {
            // Registers singleton instances of various services/interfaces in the dependency injection container
            services.AddSingleton<IDataAccess<Product>, ProductAccess>();
            services.AddSingleton<IProductAccessService, ProductAccessService>();
            services.AddSingleton<ICurrencyExchangeAccess, CurrencyExchangeAccess>();

            // Adds Swagger generation services
            services.AddSwaggerGen();

            // Adds memory cache services
            services.AddMemoryCache();

            // Adds a hosted service (CacheService) to the service collection
            services.AddHostedService<CacheService>();

            // Adds controllers as services
            services.AddControllers();
        }

        // Configures the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Checks if the environment is development; adds developer exception page if true
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Adds Swagger middleware to generate Swagger JSON and UI
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Greggs Products API V1"); });

            // Redirects HTTP requests to HTTPS
            app.UseHttpsRedirection();

            // Enables routing for the application
            app.UseRouting();

            // Authorizes users' requests
            app.UseAuthorization();

            // Maps endpoint routes to controller actions
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
