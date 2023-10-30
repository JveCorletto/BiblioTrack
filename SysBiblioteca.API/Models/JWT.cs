namespace SysBiblioteca.API.Models
{
    public class JWT
    {
        public int JWT_EXPIRE_MINUTES { get; set; }
        public int JWT_EXPIRE_DAYS_SERVICES { get; set; }

        public string JWT_ISSUER_TOKEN { get; set; }
        public string JWT_AUDIENCE_TOKEM { get; set; }
        public string JWT_SUBJECT_TOKEN { get; set; }
        public string JWT_SECRET_KEY { get; set; }
    }
}