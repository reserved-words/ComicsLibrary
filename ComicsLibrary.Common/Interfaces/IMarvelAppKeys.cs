using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComicsLibrary.Common.Interfaces
{
    public interface IMarvelAppKeys
    {
        string PrivateKey { get; }
        string PublicKey { get; }
    }
}
