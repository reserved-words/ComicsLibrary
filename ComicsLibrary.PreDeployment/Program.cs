using ErrorLog.Logger;
using Microsoft.Extensions.Configuration;
using PreDeploymentTools;
using System;
using System.IO;
using System.Reflection;

namespace ComicsLibrary.PreDeployment
{
    class Program
    {
        private static string _logFile = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
            "ReservedWords", 
            "ComicsLibrary", 
            "Logs",
            $"{DateTime.Today.ToString("yyyy-MM-dd")}.log");

        private static IConfiguration _config = GetConfig();

        static void Main(string[] args)
        {
            var appName = _config["AppName"];
            var domainName = _config["DomainName"];
            var servicePassword = _config["ServiceUserPassword"];

            var preDeploymentService = new PreDeploymentService(appName, domainName, _logFile, ex => Log(ex));
            preDeploymentService.CreateApi();
            preDeploymentService.CreateWebApp();
            preDeploymentService.CreateService(servicePassword);
        }

        private static void Log(Exception ex)
        {
            new Logger(_config).Log(ex);
        }

        private static IConfiguration GetConfig()
        {
            var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            return new ConfigurationBuilder()
                .SetBasePath(directory)
                .AddJsonFile("appSettings.json", false, true)
                .Build();
        }
    }
}
