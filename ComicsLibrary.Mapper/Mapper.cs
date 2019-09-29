using System;
using AutoMapper;
using IMapper = ComicsLibrary.Common.Services.IMapper;

namespace ComicsLibrary.Mapper
{
    public class Mapper : IMapper
    {
        private readonly Lazy<AutoMapper.IMapper> _autoMapper;

        public Mapper()
        {
            _autoMapper = new Lazy<AutoMapper.IMapper>(() => new AutoMapper.Mapper(ConfigureAutoMapper()));
        }

        private MapperConfiguration ConfigureAutoMapper()
        {
            return new MapperConfiguration(cfg => {
                cfg.AddProfile<ComicsProfile>();
            });
        }

        public T1 Map<T2, T1>(T2 source, T1 destination = null) where T2 : class where T1 : class
        {
            return _autoMapper.Value.Map(source, destination);
        }
    }
}
