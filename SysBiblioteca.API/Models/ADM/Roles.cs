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
        public int IdRol { get; set; }
        public string Rol { get; set; }

        //Propiedades Foraneas
        public int? IdEstado { get; set; }

        //Objetos
        [ForeignKey("IdEstado")]
        public Estados Estado { get; set; }


        //Campos de Auditoria Básica
        public string UsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}