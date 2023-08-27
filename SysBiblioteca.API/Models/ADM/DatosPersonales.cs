using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SysBiblioteca.API.Models.ADM
{
    [Table("DatosPersonales")]
    public class DatosPersonales
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
        public String FechaNacimiento { get; set; }
    }
}