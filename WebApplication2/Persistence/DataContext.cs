using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Common.Entities;

namespace WebApplication2.Persistence
{
    public class DataContext: IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);

          //  builder.Entity<Car>().HasOne(p => p.).WithMany(s => s.Products).OnDelete(DeleteBehavior.NoAction);
        }
        public DbSet<Car> Car { get; set; }

    }
}
