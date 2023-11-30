namespace SysBiblioteca.API.DTO
{
    public class UserReportDTO
    {
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public String? Token { get; set; }
        public String? ActualRute { get; set; }
    }
}