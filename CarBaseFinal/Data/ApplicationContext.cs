using Microsoft.EntityFrameworkCore;

namespace CarBaseFinal.Data
{

    public class ApplicationContext : DbContext
    {

        public ApplicationContext() { }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Саша\\Documents\\CarBase.mdf;Integrated Security=True;Connect Timeout=30;MultipleActiveResultSets=true");
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Korzina> Korzina { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<AllOrders> Orders { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Marks> Marks { get; set; }
        public DbSet<Types> Types { get; set; }

    }
}
