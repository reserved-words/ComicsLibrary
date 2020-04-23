using System;
using ComicsLibrary.Common.Delegates;
using ComicsLibrary.Common.Interfaces;
using ComicsLibrary.Data;
using ComicsLibrary.Common.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ComicsLibrary.Services.Mapper;
using ComicsLibrary.Common;

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
        public string SchemaName => _config.GetValue<string>("SchemaName");


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
            services.AddTransient<ISearchService, SearchService>();
            services.AddTransient<IMapper, Mapper>();
            services.AddTransient<ILogger, Logger>();
            services.AddTransient<IAsyncHelper, AsyncHelper>();
            services.AddTransient<ISeriesUpdater, SeriesUpdater>();

            services.AddTransient<MarvelUnlimited.IMapper, MarvelUnlimited.Mapper>();
            services.AddTransient<MarvelUnlimited.SourceSearcher>();
            services.AddTransient<MarvelUnlimited.UpdateService>();
            services.AddTransient<AltSource.SourceSearcher>();
            services.AddTransient<AltSource.UpdateService>();

            services.AddScoped(sp => new GetCurrentDateTime(() => DateTime.Now));
            services.AddScoped<Func<IUnitOfWork>>(sp => () => new UnitOfWork(ApiConnectionString, SchemaName));

            services.AddScoped<Func<int, ISourceSearcher>>(sp => (i) =>
            {
                if (i == 1)
                    return sp.GetService<MarvelUnlimited.SourceSearcher>();

                if (i == 2)
                    return sp.GetService<AltSource.SourceSearcher>();

                throw new NotImplementedException();
            });
            services.AddScoped<Func<int, ISourceUpdateService>>(sp => (i) =>
            {
                if (i == 1)
                    return sp.GetService<MarvelUnlimited.UpdateService>();

                if (i == 2)
                    return sp.GetService<AltSource.UpdateService>();

                throw new NotImplementedException();
            });

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
