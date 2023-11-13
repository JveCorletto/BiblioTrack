using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SysBiblioteca.API.Models.CTL
{
    [Table("EstadosMultas")]
    public class EstadosMultas : TokenManager
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32? IdEstadoMulta { get; set; }
        public String? EstadoMulta { get; set; }
    }
}