using SysBiblioteca.API.Models.ADM;
using SysBiblioteca.API.Models.CTL;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SysBiblioteca.API.Models.PRS
{
    [Table("Multas")]
    public class Multas : TokenManager
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long? IdMulta { get; set; }
        public String? ComprobantePago { get; set; }
        public Boolean? PagoFisico { get; set; }
        public DateTime? FechaValidacion { get; set; }

        //Propiedades Foraneas
        public long? IdPrestamo { get; set; }
        public int? IdEstadoMulta { get; set; }
        public long? IdUsuarioValidacion { get; set; }


        //Objetos
        [ForeignKey("IdPrestamo")]
        public Prestamos? Prestamo { get; set; }
        [ForeignKey("IdEstadoMulta")]
        public EstadosMultas? EstadoMulta { get; set; }
        [ForeignKey("IdUsuarioValidacion")]
        public Usuarios? UsuarioValidacion { get; set; }
    }
}