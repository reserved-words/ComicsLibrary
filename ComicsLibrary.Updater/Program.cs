using ComicsLibrary.Common.Interfaces;
using ComicsLibrary.Common.Services;
using ComicsLibrary.Data;
using Microsoft.Extensions.Configuration;
using System;
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
                var service = GetService(config);
                service.UpdateSeries(config.GetValue<int>("NumberOfSeriesToUpdate"));
            }
            catch (Exception ex)
            {
                logger.Log(ex);
            }
        }

        private static Service GetService(IConfiguration config)
        {
            var logger = GetLogger();
            var asyncHelper = new AsyncHelper();
            var unitOfWorkFactory = new Func<IUnitOfWork>(() => new UnitOfWork(config["UpdaterConnectionString"], config["SchemaName"]));

            var mapper = new MarvelUnlimited.Mapper();
            var apiService = new MarvelUnlimited.Service(mapper, config);
            var marvelUnlimitedService = new MarvelUnlimited.UpdateService(config, logger, mapper);

            var altSourceService = new AltSource.UpdateService(config);

            var serviceFactory = new Func<int, ISourceUpdateService>(i => 
            {
                if (i == 1)
                    return marvelUnlimitedService;

                if (i == 2)
                    return altSourceService;

                throw new NotImplementedException();
            });

            return new Service(serviceFactory, unitOfWorkFactory, logger, asyncHelper);
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
