using System;
using Unity;
using ComicsLibrary.Common.Delegates;
using ComicsLibrary.Common.Services;
using ComicsLibrary.Data.Contracts;
using ComicsLibrary.Data;
using ComicsLibrary.Services;
using MapperService = ComicsLibrary.Mapper.Mapper;

namespace ComicsLibrary
{
    public static class UnityConfig
    {
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        public static IUnityContainer Container => container.Value;

        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IService, Service>();
            container.RegisterType<IUpdateService, UpdateService>();
            container.RegisterType<IMapper, MapperService>();
            container.RegisterType<IAppKeys, AppKeys>();
            container.RegisterType<IMarvelAppKeys, AppKeys>();
            container.RegisterType<IApiService, MarvelComicsApi.Service>();
            container.RegisterType<ILogger, Logger>();
            container.RegisterType<IAsyncHelper, AsyncHelper>();

            container.RegisterFactory<GetCurrentDateTime>(c => new GetCurrentDateTime(() => DateTime.Now));
            container.RegisterFactory<Func<IUnitOfWork>>(c => new Func<IUnitOfWork>(() => new UnitOfWork()));
        }
    }
}