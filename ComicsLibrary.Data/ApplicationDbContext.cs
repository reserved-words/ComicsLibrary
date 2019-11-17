using ComicsLibrary.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace ComicsLibrary.Data
{
    public class ApplicationDbContext : DbContext
    {
        public static string SchemaName = "ComicsLibrary";

        private const string DefaultConnectionString = "Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=ComicsLibrary;Integrated Security=True;";

        private readonly string _connectionString;

        public ApplicationDbContext()
            : this(DefaultConnectionString)
        {
        }

        public ApplicationDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString, x => x.MigrationsHistoryTable("__MigrationsHistory", SchemaName));
        }

        public virtual DbSet<Series> Series { get; set; }
        public virtual DbSet<Comic> ComicSeries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(SchemaName);
        }
    }
}
