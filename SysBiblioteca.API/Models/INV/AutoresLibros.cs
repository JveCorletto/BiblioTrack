using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SysBiblioteca.API.Models.INV
{
    [Table("AutoresLibros")]
    public class AutoresLibros : TokenManager
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64? IdAutorLibro { get; set; }
        
        
        //Propiedades Foraneas
        public Int64? IdLibro { get; set; }
        public Int64? IdAutor { get; set; }
        
        
        //Objetos
        [ForeignKey("IdLibro")]
        public Libros? Libro { get; set; }
        [ForeignKey("IdAutor")]
        public Autores? Autor { get; set; }
    }
}