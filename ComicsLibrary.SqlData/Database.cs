using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
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
            using var connection = GetConnection();
            connection.Execute(Format(storedProcedure), parameters, commandType: CommandType.StoredProcedure);
        }

        public void Execute(string storedProcedure, object parameters, out int id)
        {
            var dp = new DynamicParameters();
            var props = parameters.GetType().GetProperties();
            foreach (var p in props)
            {
                dp.Add(p.Name, p.GetValue(parameters));
            }
            dp.Add("Id", dbType: DbType.Int32, direction: ParameterDirection.Output);

            using var connection = GetConnection();
            connection.Execute(Format(storedProcedure), dp, commandType: CommandType.StoredProcedure);
            id = dp.Get<int>("Id");
        }

        public void Populate<T>(T model, string storedProcedure, object parameters = null)
        {
            using var connection = GetConnection();
            using var query = connection.QueryMultiple(Format(storedProcedure), parameters, commandType: CommandType.StoredProcedure);

            var properties = typeof(T).GetProperties();
            foreach (var p in properties)
            {
                if (typeof(IEnumerable).IsAssignableFrom(p.PropertyType))
                {
                    var genericType = p.PropertyType.GenericTypeArguments.Single();
                    var values = query.Read(genericType).ToList();

                    var listType = typeof(List<>);
                    var constructedListType = listType.MakeGenericType(genericType);
                    var list = (IList)Activator.CreateInstance(constructedListType);

                    foreach (var v in values)
                    {
                        list.Add(v);
                    }

                    p.SetValue(model, list);
                }
                else
                {
                    var value = query.ReadSingle(p.PropertyType);
                    p.SetValue(model, value);
                }
            }
        }

        public List<T> Query<T>(string storedProcedure, object parameters = null)
        {
            using var connection = GetConnection();
            return connection.Query<T>(Format(storedProcedure), parameters, commandType: CommandType.StoredProcedure).ToList();
        }

        public T QuerySingle<T>(string storedProcedure, object parameters = null)
        {
            using var connection = GetConnection();
            return connection
                .Query<T>(Format(storedProcedure), parameters, commandType: CommandType.StoredProcedure)
                .SingleOrDefault();
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_config.GetConnectionString("Library"));
        }

        private string Format(string storedProcedure)
        {
            return $"[{SchemaName}].[{storedProcedure}]";
        }
    }
}
