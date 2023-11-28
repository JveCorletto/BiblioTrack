namespace SysBiblioteca.API.DTO
{
    public class PasswordDTO
    {
        public String OldPassword { get; set; }
        public String NewPassword { get; set; }

        public String Token { get; set; }
        public String ActualRute { get; set; }
    }
}