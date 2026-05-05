namespace SysBiblioteca.API.DTO
{
    public class searchQuery
    {
        public String? Titulo { get; set; }
        public Int32? IdAutor { get; set; }
        public Int32? IdGenero { get; set; }
        public Boolean? Prestamo { get; set; }

        public String? Token { get; set; }
        public String? ActualRute { get; set; }
    }
}