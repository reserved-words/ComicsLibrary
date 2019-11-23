using ComicsLibrary.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace ComicsLibrary.Services
{
    public class AppKeys : IAppKeys, IMarvelAppKeys
    {
        private readonly IConfiguration _config;

        public AppKeys(IConfiguration config)
        {
            _config = config;
        }

        public string ValidGmailLogin => GetValue("ValidGmailLogin");

        public string GoogleClientId => GetValue("GoogleClientId");

        public string GoogleClientSecret => GetValue("GoogleClientSecret");

        public string PrivateKey => GetValue("MarvelApiPrivateKey");

        public string PublicKey => GetValue("MarvelApiPublicKey");

        private string GetValue(string key) => _config[key];
    }
}