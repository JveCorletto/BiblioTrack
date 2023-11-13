using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SysBiblioteca.API.Models.INV
{
    [Table("Niveles")]
    public class Niveles : TokenManager
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64? IdNivel { get; set; }
        public Int32? Nivel { get; set; }


        //Propiedades Foraneas
        public Int64? IdEstanteria { get; set; }

        //Objetos
        [ForeignKey("IdEstanteria")]
        public Estanterias? Estanteria { get; set; }


        //Campos de Auditoria Básica
        public String? UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public String? UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}
