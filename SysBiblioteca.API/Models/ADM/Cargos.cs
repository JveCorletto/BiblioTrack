using SysBiblioteca.API.Models.CTL;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SysBiblioteca.API.Models.ADM
{
    [Table("Cargos")]
    public class Cargos : TokenManager
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? IdCargo { get; set; }
        public string? Cargo { get; set; }

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