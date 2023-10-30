using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SysBiblioteca.API.Models.ADM
{
    [Table("Menus")]
    public class Menus : TokenManager
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 IdMenu { get; set; }
        public Int64 IdParent { get; set; }
        public Int64 IdSubParent { get; set; }
        public String Nombre { get; set; }
        public String Url { get; set; }
        public String Icono { get; set; }
    }
}