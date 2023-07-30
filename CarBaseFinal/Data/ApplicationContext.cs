using Microsoft.EntityFrameworkCore;

namespace CarBaseFinal.Data
{

    public class ApplicationContext : DbContext
    {

        public ApplicationContext() { }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }


        public DbSet<Car> Cars { get; set; }
        public DbSet<Korzina> Korzina { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<AllOrders> Orders { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Marks> Marks { get; set; }
        public DbSet<Types> Types { get; set; }

    }
}
