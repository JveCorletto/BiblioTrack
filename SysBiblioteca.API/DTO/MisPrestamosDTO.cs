using SysBiblioteca.API.Models.INV;

namespace SysBiblioteca.API.DTO
{
    public class MisPrestamosDTO
    {
        public long? IdPrestamo { get; set; }
        public Libros? Libro { get; set; }
        public int? DiasPrestamo { get; set; }
        public DateTime? FechaPrestamo { get; set; }
        public DateTime? FechaDevolucion { get; set; }
        public String? Usuario { get; set;}
        public String? UsuarioEntrego { get; set;}
        public String? Estado { get; set; }
    }

    public class MisPrestamosDTO_User
    {
        public long? IdPrestamo { get; set; }
        public Libros? Libro { get; set; }
        public int? DiasPrestamo { get; set; }
        public DateTime? FechaPrestamo { get; set; }
        public DateTime? FechaDevolucion { get; set; }
        public String? Estado { get; set; }
    }

    public class PrestamoDTO
    {
        public long? IdPrestamo { get; set; }
        public Libros? Libro { get; set; }
        public int? DiasPrestamo { get; set; }
        public DateTime? FechaPrestamo { get; set; }
        public DateTime? FechaDevolucion { get; set; }
        public String? DUI { get; set; }
        public String? Usuario { get; set; }
        public String? Telefono { get; set; }
        public String? Correo { get; set; }
        public String? UsuarioEntrego { get; set; }
        public String? UsuarioRecibio { get; set; }
        public String? Estado { get; set; }
    }
}