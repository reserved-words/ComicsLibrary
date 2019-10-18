using ComicsLibrary.Common.Services;
using ComicsLibrary.Data;
using ComicsLibrary.Data.Contracts;
using ComicsLibrary.Services;
using System;
using System.Configuration;

namespace ComicsLibrary.ComicsUpdater
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var updateService = GetService(); 
            updateService.UpdateSeries();
        }

        private static IUpdateService GetService()
        {
            var appKeys = new AppKeys();
            var mapper = new Mapper.Mapper();
            var apiService = new MarvelComicsApi.Service(mapper, appKeys);
            var logger = new Logger();
            var asyncHelper = new AsyncHelper();
            var connectionString = ConfigurationManager.ConnectionStrings["ComicsTaskConnectionString"].ConnectionString;
            Func<IUnitOfWork> unitOfWorkFactory = () => new UnitOfWork(connectionString);
            return new UpdateService(unitOfWorkFactory, mapper, apiService, logger, asyncHelper);
        }
    }
}
