using SysBiblioteca.API.Models.ADM;
using SysBiblioteca.API.Models.CTL;
using SysBiblioteca.API.Models.INV;
using SysBiblioteca.API.Models.PRS;
using Microsoft.EntityFrameworkCore;

namespace SysBiblioteca.API.dbContext
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

        //Catálogos
        public DbSet<Estados> Estados { get; set; }
        public DbSet<Generos> Generos { get; set; }
        public DbSet<EstadosMultas> EstadosMultas { get; set; }

        //Administracion
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Cargos> Cargos { get; set; }
        public DbSet<DatosPersonales> DatosPersonales { get; set; }
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Menus> Menus { get; set; }
        public DbSet<Link_Rol_Menu> Link_Rol_Menu { get; set; }

        //Inventario
        public DbSet<Autores> Autores { get; set; }
        public DbSet<GenerosLiterarios> GenerosLiterarios { get; set; }
        public DbSet<Editoriales> Editoriales { get; set; }
        public DbSet<Libros> Libros { get; set; }
        public DbSet<AutoresLibros> AutoresLibros { get; set; }
        public DbSet<GenerosLibros> GenerosLibros { get; set; }
        public DbSet<Secciones> Secciones { get; set; }
        public DbSet<Estanterias> Estanterias { get; set; }
        public DbSet<Niveles> Niveles { get; set; }
        public DbSet<Ubicaciones> Ubicaciones { get; set; }

        //Procesos
        public DbSet<Prestamos> Prestamos { get; set; }
        public DbSet<Multas> Multas { get; set; }
    }
}