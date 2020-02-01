using System;
using ComicsLibrary.Common.Delegates;
using ComicsLibrary.Common.Interfaces;
using ComicsLibrary.Data;
using ComicsLibrary.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ComicsLibrary.API
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        public string ApiAllowedCorsOrigin => _config.GetValue<string>("ApiAllowedCorsOrigin");
        public string ApiAuthorityUrl => _config.GetValue<string>("ApiAuthorityUrl");
        public string ApiConnectionString => _config.GetValue<string>("ApiConnectionString");
        public string ApiName => _config.GetValue<string>("ApiName");


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = ApiAuthorityUrl;
                    options.RequireHttpsMetadata = false;
                    options.Audience = ApiName;
                });

            services.AddCors();

            services.AddMvcCore()
                .AddMvcOptions(opt => opt.EnableEndpointRouting = false)
                .AddAuthorization();

            services.AddTransient<IService, Service>();
            services.AddTransient<IMapper, Mapper.Mapper>();
            services.AddTransient<IApiService, MarvelComicsApi.Service>();
            services.AddTransient<IMarvelAppKeys, AppKeys>();
            services.AddTransient<ILogger, Logger>();
            services.AddTransient<IAsyncHelper, AsyncHelper>();

            services.AddScoped(sp => new GetCurrentDateTime(() => DateTime.Now));
            services.AddScoped<Func<IUnitOfWork>>(sp => () => new UnitOfWork(ApiConnectionString));

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(
                options => options
                    .WithOrigins(ApiAllowedCorsOrigin)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
            );

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
