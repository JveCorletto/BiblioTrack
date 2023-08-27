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

            ClaimsIdentity claims;
            claims = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Role, usuarioLoggeado.Rol.Rol),
                new Claim(ClaimTypes.Name, usuarioLoggeado.Usuario)
            });

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.JWT_SECRET_KEY));
            var singIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var jwtHandler = new JwtSecurityTokenHandler();

            var jwtSecurityToken = jwtHandler.CreateJwtSecurityToken(
                audience: _jwt.JWT_AUDIENCE_TOKEM,
                issuer: _jwt.JWT_ISSUER_TOKEN,
                subject: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(_jwt.JWT_EXPIRE_MINUTES),
                signingCredentials: singIn
                );

            newToken = jwtHandler.WriteToken(jwtSecurityToken);
            return newToken;
        }
    }
}