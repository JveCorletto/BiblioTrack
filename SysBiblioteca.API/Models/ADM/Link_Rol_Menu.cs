using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SysBiblioteca.API.Models.ADM
{
    [Table("Link_Rol_Menu")]
    public class Link_Rol_Menu : TokenManager
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64? IdLinkRolMenu { get; set; }

        //Propiedades Foraneas
        public Int32? IdRol { get; set; }
        public Int64? IdMenu { get; set; }

        //Objetos
        [ForeignKey("IdRol")]
        public Roles? Rol { get; set; }

        [ForeignKey("IdMenu")]
        public Menus? Menu { get; set; }

        public Boolean Create { get; set; }
        public Boolean Read { get; set; }
        public Boolean Update { get; set; }
        public Boolean Delete { get; set; }
    }
}