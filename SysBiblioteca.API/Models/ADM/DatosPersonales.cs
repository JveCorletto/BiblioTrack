using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SysBiblioteca.API.Models.ADM
{
    [Table("DatosPersonales")]
    public class DatosPersonales : TokenManager
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 IdDatosPersonales { get; set; }
        public String Nombres { get; set; }
        public String Apellidos { get; set; }
        public String DUI { get; set; }
        public String Correo { get; set; }
        public String Direccion { get; set; }
        public String Telefono { get; set; }

        [RegularExpression(@"^(0[1-9]|[12][0-9]|3[01])\/(0[1-9]|1[012])\/(19|20)\d\d$", ErrorMessage = "La Fecha de Nacimiento debe tener el formato dd/mm/aaaa")]
        public String FechaNacimiento { get; set; }
    }
}