using System.Collections.Generic;

namespace ComicsLibrary.Common
{
    public interface ISeriesService
    {
        List<Comic> GetBooks(int seriesId, int typeId, int limit, int offset);
        SeriesBookLists GetById(int id, int numberOfComics);
        void Reinstate(int id);
        void Archive(int id);
        void Remove(int id);

    }
}
