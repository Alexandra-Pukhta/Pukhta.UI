using Cars.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Pukhta.API.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Car> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    }
}
