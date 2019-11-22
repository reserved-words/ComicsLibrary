using ComicsLibrary.Data;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace DatabaseMigrater
{
    public static class CreateUsers
    {
        public static void Run(string connectionString, string databaseName, string webAppUser, string serviceUserName, string serviceUserPassword)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@DatabaseName", databaseName),
                    new SqlParameter("@ServiceUserName", serviceUserName),
                    new SqlParameter("@ServiceUserPassword", serviceUserPassword),
                    new SqlParameter("@WebAppUser", webAppUser)
                };

                using (var command = new SqlCommand($"[{ApplicationDbContext.SchemaName}].[CreateUsers]", connection) { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.AddRange(parameters.ToArray());
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }
    }
}
