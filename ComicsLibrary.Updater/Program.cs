using ComicsLibrary.Common;
using ComicsLibrary.Data;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Logger = ComicsLibrary.Common.Logger;

namespace ComicsLibrary.Updater
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var config = GetConfig();

            var logger = new Logger(config);

            try
            {
                var service = GetService(config);
                await service.UpdateSeries(config.GetValue<int>("NumberOfSeriesToUpdate"));
            }
            catch (Exception ex)
            {
                await logger.Log(ex, 3);
            }
        }

        private static Service GetService(IConfiguration config)
        {
            var logger = GetLogger();
            var asyncHelper = new AsyncHelper();
            var unitOfWorkFactory = new Func<IUnitOfWork>(() => new UnitOfWork(config["UpdaterConnectionString"], config["SchemaName"]));

            var updater = new SeriesUpdater(unitOfWorkFactory);

            var mapper = new MarvelUnlimited.Mapper();
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

            return new Service(serviceFactory, unitOfWorkFactory, logger, asyncHelper, updater);
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
