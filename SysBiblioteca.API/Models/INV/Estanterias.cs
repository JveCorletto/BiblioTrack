using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SysBiblioteca.API.Models.INV
{
    [Table("Estanterias")]
    public class Estanterias : TokenManager
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64? IdEstanteria { get; set; }
        public Int32? Estanteria { get; set; }


        //Propiedades Foraneas
        public Int64? IdSeccion { get; set; }

        //Objetos
        [ForeignKey("IdSeccion")]
        public Secciones? Seccion { get; set; }


        //Campos de Auditoria Básica
        public String? UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public String? UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}
