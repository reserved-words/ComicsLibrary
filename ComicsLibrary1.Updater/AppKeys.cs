using ComicsLibrary.Common.Services;
using System.Configuration;

namespace ComicsLibrary.Services
{
    public class AppKeys : IMarvelAppKeys
    {
        public string PrivateKey => GetValue("MarvelApiPrivateKey");

        public string PublicKey => GetValue("MarvelApiPublicKey");

        private string GetValue(string key) => ConfigurationManager.AppSettings[key];
    }
}