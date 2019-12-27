using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComicsLibrary.Common.Interfaces
{
    public interface IAppKeys
    {
        string ValidGmailLogin { get; }
        string GoogleClientId { get; }
        string GoogleClientSecret { get; }
    }
}
