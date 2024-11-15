namespace SysBiblioteca.API.DTO
{
    public class PrestamosDTO
    {
        public Int64? IdLibro { get; set; }
        public Int64? IdUsuario { get; set; }
        public Int32? DiasPrestamo { get; set; }

        public String? Token { get; set; }    
        public String? ActualRute { get; set; }    
    }
}