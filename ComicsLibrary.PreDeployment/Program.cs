using PreDeploymentTools;
using System;

namespace ComicsLibrary.PreDeployment
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3)
                throw new Exception("Incorrect number of arguments");

            var appName = args[0];
            var domainName = args[1];
            var servicePassword = args[2];

            var preDeploymentService = new PreDeploymentService(appName, domainName);
            preDeploymentService.CreateApi();
            preDeploymentService.CreateWebApp();
            preDeploymentService.CreateService(servicePassword);
        }
    }
}
