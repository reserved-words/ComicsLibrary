using System.Threading.Tasks;

namespace ComicsLibrary.Common.Interfaces
{
    public interface ISourceUpdateService
    {
        Task<SeriesUpdate> GetUpdatedSeries(int sourceItemID, string url, bool isBackgroundProcess);
    }
}