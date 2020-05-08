using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ComicsLibrary.SqlData
{
    public class Database : IDatabase
    {
        private const string SchemaName = "ComicsLibrary";

        private readonly IConfiguration _config;

        public Database(IConfiguration config)
        {
            _config = config;
        }

        public void Execute(string storedProcedure, object parameters = null)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("Library"));
            connection.Execute(Format(storedProcedure), parameters, commandType: CommandType.StoredProcedure);
        }

        public List<T> Query<T>(string storedProcedure, object parameters = null)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("Library"));
            return connection.Query<T>(Format(storedProcedure), parameters, commandType: CommandType.StoredProcedure).ToList();
        }

        public T QuerySingle<T>(string storedProcedure, object parameters = null)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("Library"));
            return connection.QuerySingle<T>(Format(storedProcedure), parameters, commandType: CommandType.StoredProcedure);
        }

        private string Format(string storedProcedure)
        {
            return $"[{SchemaName}].[{storedProcedure}]";
        }
    }
}
