using Microsoft.EntityFrameworkCore;

namespace SysBiblioteca.API.Data
{
    public class DataContext : DbContext 
    {
        private String connectionString = String.Empty;

        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {
            var appsettings = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            connectionString = appsettings.GetConnectionString("SysBiblioteca_Context");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(connectionString);
        }


    }
}