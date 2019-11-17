namespace ComicsLibrary.Common.Interfaces
{
    public interface IMapper
    {
        T1 Map<T2, T1>(T2 source, T1 destination = null) where T1 : class where T2 : class;
    }
}
