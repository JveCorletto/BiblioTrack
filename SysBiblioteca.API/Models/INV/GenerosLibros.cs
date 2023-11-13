using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SysBiblioteca.API.Models.INV
{
    [Table("GenerosLibros")]
    public class GenerosLibros : TokenManager
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64? IdGeneroLibro { get; set; }
        
        
        //Propiedades Foraneas
        public Int64? IdLibro { get; set; }
        public Int64? IdGenero { get; set; }


        //Objetos
        [ForeignKey("IdLibro")]
        public Libros? Libro { get; set; }
        [ForeignKey("IdGenero")]
        public GenerosLiterarios? GeneroLiterario { get; set; }
    }
}