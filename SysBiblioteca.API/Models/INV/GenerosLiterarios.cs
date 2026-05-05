using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SysBiblioteca.API.Models.INV
{
    [Table("GenerosLiterarios")]
    public class GenerosLiterarios : TokenManager
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64? IdGenero { get; set; }
        public String? Genero { get; set; }
    }
}