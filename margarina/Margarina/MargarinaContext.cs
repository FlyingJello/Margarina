using Margarina.Models;
using Microsoft.EntityFrameworkCore;

namespace Margarina
{
    public class MargarinaContext : DbContext
    {

        public DbSet<User> Users { get; set; }

        public MargarinaContext(DbContextOptions options) : base(options)
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User { Username = "Eugenie", Password = "1234" },
                new User { Username = "Doug", Password = "1234" });

            base.OnModelCreating(modelBuilder);
        }
    }
}
