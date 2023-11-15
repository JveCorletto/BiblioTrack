using SysBiblioteca.API.Models.CTL;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SysBiblioteca.API.Models.INV
{
    [Table("Libros")]
    public class Libros : TokenManager
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64? IdLibro { get; set; }
        public String? FotoLibro { get; set; }
        public String? Libro { get; set; }
        public String? Version { get; set; }
        public String? ISBN { get; set; }
        public Int32? AnioPublicacion { get; set; }
        public String? Descripcion { get; set; }
        public Int32? Cantidad { get; set; }


        //Propiedades Foraneas
        public Int32? IdEstado { get; set; }
        public Int64? IdEditorial { get; set; }


        //Objetos
        [ForeignKey("IdEstado")]
        public Estados? Estado { get; set; }
        [ForeignKey("IdEditorial")]
        public Editoriales? Editorial { get; set; }


        //Campos de Auditoria Básica
        public String? UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public String? UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}