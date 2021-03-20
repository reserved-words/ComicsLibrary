using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ComicsLibrary.Common
{
    public class Logger : ILogger
    {
        private readonly IConfiguration _config;

        public Logger(IConfiguration config)
        {
            _config = config;
        }

        public async Task Log(Exception ex, int level)
        {
            var loggingConfig = _config.GetSection("Logging");
            var url = loggingConfig["Url"];
            var app = loggingConfig["App"];

            var body = new
            {
                app = app,
                level = level,
                message = ex.Message,
                stackTrace = ex.StackTrace
            };

            var content = JsonContent.Create(body);

            using (var httpClient = new HttpClient())
            {
                await httpClient.PostAsync(url, content);
            }
        }
    }
}
