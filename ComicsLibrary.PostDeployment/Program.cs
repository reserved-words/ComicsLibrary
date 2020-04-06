using ComicsLibrary.Data;
using ErrorLog.Logger;
using Microsoft.Extensions.Configuration;
using PostDeploymentTools;
using System;
using System.IO;
using System.Reflection;

namespace ComicsLibrary.PostDeployment
{
    class Program
    {
        private static IConfiguration _config = GetConfig();

        static void Main(string[] args)
        {
            var appName = _config["AppName"];
            var connectionString = _config["ConnectionString"];
            var databaseName = _config["DatabaseName"];
            var schemaName = _config["SchemaName"];
            var domainName = _config["DomainName"];

            var postDeploymentService = new PostDeploymentService(domainName, appName, connectionString, databaseName, schemaName, ex => Log(ex));
            postDeploymentService.UpdateDatabase(() => new ApplicationDbContext(connectionString, schemaName));

            postDeploymentService.CreateTaskUser();
            postDeploymentService.GrantTaskPermission("SELECT, INSERT, UPDATE, DELETE");

            postDeploymentService.CreateApiUser();
            postDeploymentService.GrantApiPermission("SELECT, INSERT, UPDATE, DELETE");
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