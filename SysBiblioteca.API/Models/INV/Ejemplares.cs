using SysBiblioteca.API.Models.CTL;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SysBiblioteca.API.Models.INV
{
    [Table("Ejemplares")]
    public class Ejemplares : TokenManager
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64? IdEjemplar { get; set; }
        public String? CodigoEjemplar { get; set; }


        //Propiedades Foraneas
        public Int64? IdLibro { get; set; }

        //Objetos
        [ForeignKey("IdLibro")]
        public Libros? Libro { get; set; }

        //Campos de Auditoria Básica
        public String? UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public String? UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}