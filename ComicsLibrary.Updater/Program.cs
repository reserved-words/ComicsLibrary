using ComicsLibrary.Common.Interfaces;
using ComicsLibrary.Data;
using ComicsLibrary.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Reflection;

namespace ComicsLibrary.Updater
{
    public class Program
    {
        static void Main(string[] args)
        {
            var logger = new Logger();

            try
            {
                var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                var config = new ConfigurationBuilder()
                    .SetBasePath(directory)
                    .AddJsonFile("appSettings.json", false, true)
                    .Build();

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
            var logger = new Logger();
            var asyncHelper = new AsyncHelper();
            var connectionString = config["UpdaterConnectionString"];
            Func<IUnitOfWork> unitOfWorkFactory = () => new UnitOfWork(connectionString);
            return new UpdateService(unitOfWorkFactory, mapper, apiService, logger, asyncHelper);
        }
    }
}
