using ComicsLibrary.Common.Interfaces;

namespace ComicsLibrary.Updater
{
    public class AppKeys : IMarvelAppKeys
    {
        public AppKeys(string privateKey, string publicKey)
        {
            PrivateKey = privateKey;
            PublicKey = publicKey;
        }

        public string PrivateKey { get; }
        public string PublicKey { get;}
    }
}