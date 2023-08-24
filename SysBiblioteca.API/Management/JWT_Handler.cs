using System.Text;
using System.Security.Claims;
using SysBiblioteca.API.Models;
using SysBiblioteca.API.Models.ADM;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace SysBiblioteca.API.Management
{
    public class JWT_Handler
    {
        private readonly IConfiguration _configuration;
        public JWT_Handler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public String GenerateToken(Usuarios usuarioLoggeado)
        {
            String newToken = String.Empty;
            var _jwt = _configuration.GetSection("JWT").Get<JWT>();

            var _claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _jwt.JWT_SUBJECT_TOKEN),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()),
                new Claim("Rol", usuarioLoggeado.Rol.Rol)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.JWT_SECRET_KEY));
            var singIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_jwt.JWT_ISSUER_TOKEN, _jwt.JWT_AUDIENCE_TOKEM, _claims, expires: DateTime.Now.AddMinutes(_jwt.JWT_EXPIRE_MINUTES), signingCredentials: singIn);

            newToken = new JwtSecurityTokenHandler().WriteToken(token);
            return newToken;
        }
    }
}