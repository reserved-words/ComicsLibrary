using ComicsLibrary.Common.Interfaces;
using ComicsLibrary.Data;
using ComicsLibrary.Common.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Logger = ComicsLibrary.Common.Services.Logger;

namespace ComicsLibrary.Updater
{
    public class Program
    {
        static void Main(string[] args)
        {
            var config = GetConfig();

            var logger = new Logger(config);

            try
            {
                var services = GetServices(config);

                foreach (var service in services)
                {
                    service.UpdateSeries(1);
                }
            }
            catch (Exception ex)
            {
                logger.Log(ex);
            }
        }

        private static List<IUpdateService> GetServices(IConfiguration config)
        {
            var mapper = new MarvelUnlimited.Mapper();
            var apiService = new MarvelUnlimited.Service(mapper, config);
            var logger = GetLogger();
            var asyncHelper = new AsyncHelper();
            Func<IUnitOfWork> unitOfWorkFactory = () => new UnitOfWork(config["UpdaterConnectionString"], config["SchemaName"]);
            var marvelUnlimitedService = new UpdateService(unitOfWorkFactory, mapper, apiService, logger, asyncHelper);

            return new List<IUpdateService> { marvelUnlimitedService };
        }

        private static Logger GetLogger()
        {
            return new Logger(GetConfig());
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
