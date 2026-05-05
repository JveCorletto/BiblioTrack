using SysBiblioteca.API.Models;
using SysBiblioteca.API.Models.INV;

namespace SysBiblioteca.API.DTO
{
    public class LibrosDTO
    {
        public Int64? IdLibro { get; set; }
        public String? FotoLibro { get; set; }
        public String? Libro { get; set; }
        public String? Autores { get; set; }
        public String? Generos { get; set; }
        public Int32? AnioPublicacion { get; set; }
        public Int32? Cantidad { get; set; }
    }

    public class LibrosDTO_ADD : TokenManager
    {
        public Int64? IdLibro { get; set; }
        public String? FotoLibro { get; set; }
        public String? Libro { get; set; }
        public String? Version { get; set; }
        public String? ISBN { get; set; }
        public Int32? AnioPublicacion { get; set; }
        public String? Descripcion { get; set; }
        public Int32? Cantidad { get; set; }
        public Int64? IdEditorial { get; set; }

        public List<Autores>? Autores { get; set; }
        public List<GenerosLiterarios>? GenerosLiterarios { get; set; }
    }
}