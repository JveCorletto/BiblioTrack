using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SysBiblioteca.API.Models.INV
{
    [Table("Autores")]
    public class Autores : TokenManager
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64? IdAutor { get; set; }
        public String? Autor { get; set; }
    }
}