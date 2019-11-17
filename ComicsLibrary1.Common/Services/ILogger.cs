using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComicsLibrary.Data.Contracts;

namespace ComicsLibrary.Common.Services
{
    public interface ILogger
    {
        void Log(Exception ex);
    }
}
