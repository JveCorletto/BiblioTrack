using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SysBiblioteca.API.Models.CTL
{
    [Table("Generos")]
    public class Generos : TokenManager
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? IdGenero { get; set; }
        public string? Genero { get; set; }
    }
}