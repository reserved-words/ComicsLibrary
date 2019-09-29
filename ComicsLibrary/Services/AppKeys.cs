using ComicsLibrary.Common.Services;
using System.Configuration;

namespace ComicsLibrary.Services
{
    public class AppKeys : IAppKeys, IMarvelAppKeys
    {
        public string ValidGmailLogin => GetValue("ValidGmailLogin");

        public string GoogleClientId => GetValue("GoogleClientId");

        public string GoogleClientSecret => GetValue("GoogleClientSecret");

        public string PrivateKey => GetValue("MarvelApiPrivateKey");

        public string PublicKey => GetValue("MarvelApiPublicKey");

        private string GetValue(string key) => ConfigurationManager.AppSettings[key];
    }
}