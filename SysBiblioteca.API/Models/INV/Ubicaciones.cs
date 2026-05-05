using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SysBiblioteca.API.Models.INV
{
    [Table("Ubicaciones")]
    public class Ubicaciones : TokenManager
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64? IdAutorLibro { get; set; }
        
        
        //Propiedades Foraneas
        public Int64? IdLibro { get; set; }
        public Int64? IdNivel { get; set; }


        //Objetos
        [ForeignKey("IdLibro")]
        public Libros? Libro { get; set; }
        [ForeignKey("IdNivel")]
        public GenerosLiterarios? GeneroLiterario { get; set; }
    }
}