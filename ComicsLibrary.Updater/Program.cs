using ComicsLibrary.Common.Interfaces;
using ComicsLibrary.Data;
using ComicsLibrary.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Reflection;
using Logger = ComicsLibrary.Services.Logger;

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
                var updateService = GetService(config);
                updateService.UpdateSeries();
            }
            catch (Exception ex)
            {
                logger.Log(ex);
            }
        }

        private static IUpdateService GetService(IConfiguration config)
        {
            var appKeys = new AppKeys(config["MarvelApiPrivateKey"], config["MarvelApiPublicKey"]);
            var mapper = new Mapper.Mapper();
            var apiService = new MarvelComicsApi.Service(mapper, appKeys);
            var logger = GetLogger();
            var asyncHelper = new AsyncHelper();
            Func<IUnitOfWork> unitOfWorkFactory = () => new UnitOfWork(config["UpdaterConnectionString"], config["SchemaName"]);
            return new UpdateService(unitOfWorkFactory, mapper, apiService, logger, asyncHelper);
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
