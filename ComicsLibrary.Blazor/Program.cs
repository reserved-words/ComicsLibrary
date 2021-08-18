using ComicsLibrary.Blazor.Mocks;
using ComicsLibrary.Blazor.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ComicsLibrary.Blazor
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddMudServices();

            builder.Services.AddTransient<IMessenger, Messenger>();

#if DEBUG
            builder.Services.AddTransient<IReadingRepository, MockReadingRepository>();
            builder.Services.AddTransient<ISeriesRepository, MockSeriesRepository>();
            builder.Services.AddTransient<ISearchService, MockSearchService>();
#else
            builder.Services.AddTransient<IReadingRepository, ReadingRepository>();
            builder.Services.AddTransient<ISeriesRepository, SeriesRepository>();
            builder.Services.AddTransient<ISearchService, SearchService>();
#endif

            await builder.Build().RunAsync();
        }
    }
}
