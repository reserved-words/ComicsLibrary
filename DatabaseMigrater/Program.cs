using System;
using System.IO;
using System.Text;

namespace DatabaseMigrater
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length != 3)
                    throw new Exception("Incorrect numer of arguments");

                var connectionString = args[0];
                var databaseName = args[1];
                var webAppUser = args[2];

                CreateSchema.Run(connectionString);
                CreateUsers.Run(connectionString, databaseName, webAppUser);
            }
            catch (Exception exc)
            {
                Log(exc, args);
            }
        }

        private static void Log(Exception exc, string[] args)
        {
            var logFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "ReservedWords",
                "GameDeals");

            Directory.CreateDirectory(logFolder);

            var str = new StringBuilder();

            str.Append(DateTime.Now.ToString(@"dd/MM/yy HH:mm:ss"));
            str.Append(Environment.NewLine);

            while (exc != null)
            {
                str.Append(exc.Message);
                str.Append(Environment.NewLine);

                str.Append(exc.StackTrace);
                str.Append(Environment.NewLine);

                if (exc.Data != null)
                {
                    foreach (var key in exc.Data.Keys)
                    {
                        str.Append($"{key}: {exc.Data[key]}");
                        str.Append(Environment.NewLine);
                    }
                }

                exc = exc.InnerException;
            }

            foreach (var arg in args)
            {
                str.Append($"arg: {arg}");
                str.Append(Environment.NewLine);
            }

            str.Append(Environment.NewLine);
            str.Append(Environment.NewLine);

            var logFile = Path.Combine(logFolder, "errors.log");

            File.AppendAllText(logFile, str.ToString());
        }
    }
}
