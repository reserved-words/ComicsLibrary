using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace ComicsLibrary.ComicsUpdater
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(60 * 5);
            var url = ConfigurationManager.AppSettings["Url"];
            var result = client.PostAsync(new Uri(url), null).Result;
        }
    }
}
