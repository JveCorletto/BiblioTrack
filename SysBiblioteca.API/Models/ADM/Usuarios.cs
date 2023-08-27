using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SysBiblioteca.API.Models.CTL;

namespace SysBiblioteca.API.Models.ADM
{
    [Table("Usuarios")]
    public class Usuarios
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 IdUsuario { get; set; }
        public String Usuario { get; set; }
        public String Contrasenia { get; set; }
        public String? Token { get; set; }
        public DateTime? UltimoAcceso { get; set; }
        public Int32? ConteoIntentos { get; set; }


        //Propiedades Foraneas
        public Int32? IdEstado { get; set; }
        public Int32? IdRol { get; set; }
        public Int32? IdCargo { get; set; }
        public Int64? IdDatosPersonales { get; set; }

        //Objetos
        [ForeignKey("IdEstado")]
        public Estados? Estado { get; set; }

        [ForeignKey("IdRol")]
        public Roles? Rol { get; set; }
        
        [ForeignKey("IdCargo")]
        public Cargos? Cargo { get; set; }
        
        [ForeignKey("IdDatosPersonales")]
        public DatosPersonales? DatosPersonales { get; set; }


        //Campos de Auditoria Básica
        public String UsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public String? UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}