using ComicsLibrary.Common.Data;
using Microsoft.EntityFrameworkCore;
using PostDeploymentTools;

namespace ComicsLibrary.Data
{
    public class ApplicationDbContext : MigratableDbContext
    {
        private const string DefaultConnectionString = "Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=ComicsLibrary;Integrated Security=True;";
        private const string SchemaName = "ComicsLibrary";

        public ApplicationDbContext()
            : this(DefaultConnectionString, SchemaName)
        {
        }

        public ApplicationDbContext(string connectionString, string schemaName)
            : base(connectionString, schemaName)
        {
        }

        public virtual DbSet<Source> Sources { get; set; }
        public virtual DbSet<Series> Series { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<HomeBookType> HomeBookTypes { get; set; }
    }
}
