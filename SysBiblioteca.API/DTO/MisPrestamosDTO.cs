using SysBiblioteca.API.Models.INV;

namespace SysBiblioteca.API.DTO
{
    public class MisPrestamosDTO
    {
        public long? IdPrestamo { get; set; }
        public Libros? Libro { get; set; }
        public int? DiasPrestamo { get; set; }
        public DateTime? FechaPrestamo { get; set; }
        public String? Estado { get; set; }
    }
}