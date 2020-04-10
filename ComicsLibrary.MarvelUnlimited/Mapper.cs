using System;
using AutoMapper;
using IMapper = ComicsLibrary.Common.Interfaces.IMapper;

namespace ComicsLibrary.MarvelUnlimited
{
    public class Mapper : IMapper
    {
        private readonly Lazy<AutoMapper.IMapper> _autoMapper;

        public Mapper()
        {
            _autoMapper = new Lazy<AutoMapper.IMapper>(() => new AutoMapper.Mapper(new MapperConfiguration(cfg => {
                cfg.AddProfile<MappingProfile>();
            })));
        }

        public T1 Map<T2, T1>(T2 source, T1 destination = null) where T2 : class where T1 : class
        {
            return _autoMapper.Value.Map(source, destination);
        }
    }
}
