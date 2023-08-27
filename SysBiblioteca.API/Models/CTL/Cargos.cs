using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SysBiblioteca.API.Models.CTL
{
    [Table("Cargos")]
    public class Cargos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 IdCargo { get; set; }
        public String Cargo { get; set; }


        //Propiedades Foraneas
        public Int32? IdEstado { get; set; }

        //Objetos
        [ForeignKey("IdEstado")]
        public Estados? Estado { get; set; }

        //Campos de Auditoria Básica
        public String UsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public String? UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}
