using System;
using System.Threading.Tasks;

namespace ComicsLibrary.Common
{
    public interface ILogger
    {
        Task Log(Exception ex, int level);
    }
}
