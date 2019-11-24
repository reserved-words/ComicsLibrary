using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComicsLibrary.Common.Delegates;
using ComicsLibrary.Common.Interfaces;
using ComicsLibrary.Data;
using ComicsLibrary.Mapper;
using ComicsLibrary.Services;
using MarvelSharp;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ComicsLibrary
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IService, Service>();
            services.AddTransient<IMapper, Mapper.Mapper>();
            services.AddTransient<IApiService, MarvelComicsApi.Service>();
            services.AddTransient<IMarvelAppKeys, AppKeys>();
            services.AddTransient<ILogger, Logger>();
            services.AddTransient<IAsyncHelper, AsyncHelper>();

            services.AddScoped(sp => new GetCurrentDateTime(() => DateTime.Now));
            services.AddScoped<Func<IUnitOfWork>>(sp => () => new UnitOfWork(Configuration));

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
