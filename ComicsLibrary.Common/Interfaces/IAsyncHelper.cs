using System;
using System.Threading.Tasks;

namespace ComicsLibrary.Common
{
    public interface IAsyncHelper
    {
        TResult RunSync<TResult>(Func<Task<TResult>> func);

        void RunSync(Func<Task> func);
    }
}
