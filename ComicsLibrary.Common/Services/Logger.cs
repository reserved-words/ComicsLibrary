using ComicsLibrary.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using ErrorLogger = ErrorLog.Logger.Logger;

namespace ComicsLibrary.Common.Services
{
    public class Logger : ILogger
    {
        private readonly ErrorLogger _logger;

        public Logger(IConfiguration config)
        {
            _logger = new ErrorLogger(config);
        }

        public void Log(Exception ex)
        {
            _logger.Log(ex);
        }
    }
}
