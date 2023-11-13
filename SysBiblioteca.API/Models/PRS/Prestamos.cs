using SysBiblioteca.API.Models.ADM;
using SysBiblioteca.API.Models.INV;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SysBiblioteca.API.Models.PRS
{
    [Table("Prestamos")]
    public class Prestamos : TokenManager
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long? IdPrestamo { get; set; }
        public int? DiasPrestamo { get; set; }
        public DateTime? FechaPrestamo { get; set; }
        public bool? Entregado { get; set; }


        public DateTime? FechaDevolucion { get; set; }
        public bool? Finalizado { get; set; }


        //Propiedades Foraneas
        public long? IdUsuario { get; set; }
        public long? IdLibro { get; set; }
        public long? IdUsuarioEntrego { get; set; }
        public long? IdUsuarioRecibio { get; set; }


        //Objetos
        [ForeignKey("IdUsuario")]
        public Usuarios? Usuario { get; set; }
        [ForeignKey("IdLibro")]
        public Libros? Libro { get; set; }
        [ForeignKey("IdUsuarioEntrego")]
        public Usuarios? UsuarioEntrego { get; set; }
        [ForeignKey("IdUsuarioRecibio")]
        public Usuarios? UsuarioRecibio { get; set; }
    }
}