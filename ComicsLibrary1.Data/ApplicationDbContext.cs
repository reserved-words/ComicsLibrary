using ComicsLibrary.Common.Models;
using System.Data.Entity;

namespace ComicsLibrary.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
            : base("ComicsLibrary")
        {
        }

        public ApplicationDbContext(string connectionString)
            : base(connectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("ComicsLibrary");
        }

        public virtual DbSet<Comic> Comics { get; set; }
        public virtual DbSet<Series> ComicSeries { get; set; }
    }
}