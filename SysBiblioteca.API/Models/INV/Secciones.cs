using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SysBiblioteca.API.Models.INV
{
    [Table("Secciones")]
    public class Secciones : TokenManager
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64? IdSeccion { get; set; }
        public String? Seccion { get; set; }


        //Campos de Auditoria Básica
        public String? UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public String? UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}