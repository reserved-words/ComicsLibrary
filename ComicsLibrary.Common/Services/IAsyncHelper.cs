using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComicsLibrary.Common.Services
{
    public interface IAsyncHelper
    {
        TResult RunSync<TResult>(Func<Task<TResult>> func);

        void RunSync(Func<Task> func);
    }
}
