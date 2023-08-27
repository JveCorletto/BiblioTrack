using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SysBiblioteca.API.Models.CTL;

namespace SysBiblioteca.API.Models.ADM
{
    [Table("Roles")]
    public class Roles
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 IdRol { get; set; }
        public String Rol { get; set; }

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