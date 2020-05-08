using System;
using System.Collections.Generic;
using System.Text;

namespace ComicsLibrary.Common
{
    public static class UrlExtensionMethods
    {
        public static string Secure(this string url)
        {
            return url?.Replace("http://", "https://");
        }
    }
}
