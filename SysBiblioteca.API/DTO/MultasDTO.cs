namespace SysBiblioteca.API.DTO
{
    public class MultasDTO
    {
        public long? IdMulta { get; set; }
        public String? FotoLibro { get; set; }
        public String? Libro { get; set; }
        public String? Usuario { get; set; }
        public Int32? DiasRetraso { get; set; }
        public String? Estado { get; set; }
        public Decimal? Monto { get; set; }
        public DateTime? FechaValidacion { get; set; }
        public String? UsuarioValidacion { get; set; }
    }
}