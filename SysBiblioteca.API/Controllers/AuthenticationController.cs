using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using SysBiblioteca.API.Management;
using SysBiblioteca.API.Models.ADM;
using Microsoft.AspNetCore.Authorization;
using SysBiblioteca.API.Services.ADM.UsuariosService;

namespace SysBiblioteca.API.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("SysBiblioteca/API/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly iUsuariosService iUsuarios;
        private readonly IConfiguration _configuration;

        public AuthenticationController(IConfiguration configuration, iUsuariosService usuariosService)
        {
            iUsuarios = usuariosService;
            _configuration = configuration;
        }

        // SysBiblioteca/API/Authentication/LogIn
        [HttpPost]
        [Route("LogIn")]
        public IActionResult LogIn([FromBody] Object _user)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(_user.ToString());
                String Usuario = data.Usuario.ToString();
                String Contrasenia = data.Contrasenia.ToString();

                Usuarios usuario = iUsuarios.getUserInfo(Usuario);
                if (usuario != null)
                {
                    //Se valida que el usuario esté activo
                    if (usuario.IdEstado == 1)
                    {
                        Usuarios _loggedUser = iUsuarios.LogIn(Usuario, Contrasenia, _configuration);
                        if (_loggedUser != null)
                        {
                            if (_loggedUser.ConteoIntentos == 0)
                            {
                                if (_loggedUser.IdRol != null)
                                {
                                    _rp.Resultado = 1;
                                    _rp.Mensaje = "Bienvenid@ " + _loggedUser.Usuario;
                                    _rp.Datos = new { _loggedUser.Usuario, _loggedUser.Token };
                                    return Ok(_rp);
                                }
                                else
                                {
                                    _rp.Resultado = 2;
                                    _rp.Mensaje = "Usuario no posee rol, comuníquese con el administrador para que verifique la configuración del rol";
                                    _rp.Datos = null;
                                    return Ok(_rp);
                                }
                            }
                            else if (_loggedUser.ConteoIntentos == 3)
                            {
                                _rp.Mensaje = "Usuario Bloqueado";
                                return Ok(_rp);
                            }
                            else
                            {
                                _rp.Mensaje = "Datos incorrectos, revise usuario y contraseña.";
                                return BadRequest(_rp);
                            }
                        }
                        else
                        {
                            _rp.Mensaje = "Datos incorrectos, revise usuario y contraseña.";
                            return BadRequest(_rp);
                        }
                    }
                    else
                    {
                        _rp.Mensaje = "Usuario Inactivo";
                        return BadRequest(_rp);
                    }
                }
                else
                {
                    _rp.Mensaje = "Datos incorrectos, revise usuario y contraseña.";
                    return BadRequest(_rp);
                }
            }
            catch (Exception ex)
            {
                _rp.Mensaje = ex.ToString();
                return BadRequest(_rp);
            }
        }
    }
}