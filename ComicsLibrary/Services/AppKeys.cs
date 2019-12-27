using ComicsLibrary.Common.Interfaces;
using Microsoft.Extensions.Configuration;

namespace ComicsLibrary
{
    public class AppKeys : IMarvelAppKeys
    {
        private readonly IConfiguration _config;

        public AppKeys(IConfiguration config)
        {
            _config = config;
        }

        public string PrivateKey => GetValue("MarvelApiPrivateKey");

        public string PublicKey => GetValue("MarvelApiPublicKey");

        private string GetValue(string key) => _config[key];
    }
}