using System.ComponentModel.DataAnnotations.Schema;

namespace SysBiblioteca.API.Models
{
    public class TokenManager
    {
        [NotMapped]
        public String Token { get; set; }
        [NotMapped]
        public String ActualRute { get; set; }
    }
}