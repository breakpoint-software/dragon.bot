using Microsoft.EntityFrameworkCore;
using Models.Domain;


namespace Models
{

    public class DragonBotDbContext : DbContext
    {
        public virtual DbSet<Order> Orders { get; set; }

        public DragonBotDbContext()
        {

        }

        public DragonBotDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }


}
