using HRGeoLocator.Services;
using HRGeoLocator.Services.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace HRGeoLocator
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        private static readonly String _VERSION_FOR_SWAGGER_DISLPAY = "Version 1 Release candidate";
        private static readonly String _NAME_FOR_SWAGGER_DISLPAY = "HR GeoLocator Services";
        private readonly string _ALLOW_SPECIFIC_ORIGIN = "_myAllowSpecificOrigins";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }
        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container. 
        /// </summary>
        /// <param name="services"></param>

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddTransient<IHRGeonameService, HRGeonameService>();
            services.AddTransient<IWebCamService, WebCamService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            // Register the Swagger services
            services.AddSwaggerDocument(swagger => {
                swagger.Version = _VERSION_FOR_SWAGGER_DISLPAY;
                swagger.Title = _NAME_FOR_SWAGGER_DISLPAY;
                swagger.GenerateEnumMappingDescription = true;
            });

            //Allow CORS
            services.AddCors(options =>
            {
                options.AddPolicy(_ALLOW_SPECIFIC_ORIGIN,
                builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });
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
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            // Register the Swagger generator and the Swagger UI middlewares
            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseHttpsRedirection();
            app.UseCors(_ALLOW_SPECIFIC_ORIGIN);
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
