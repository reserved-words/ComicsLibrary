using ComicsLibrary.Data;
using Microsoft.EntityFrameworkCore;

namespace DatabaseMigrater
{
    public static class CreateSchema
    {
        public static void Run(string connectionString)
        {
            using (var dbContext = new ApplicationDbContext(connectionString))
            {
                dbContext.Database.Migrate();
            }
        }
    }
}
