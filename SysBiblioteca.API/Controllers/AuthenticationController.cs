using Microsoft.AspNetCore.Mvc;
using SysBiblioteca.API.DTO;
using SysBiblioteca.API.Management;
using SysBiblioteca.API.Models.ADM;
using SysBiblioteca.API.Services.ADM.UsuariosService;

namespace SysBiblioteca.API.Controllers
{
    [ApiController]
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
        public IActionResult LogIn([FromBody] newLogger user)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                Usuarios usuario = iUsuarios.getUserInfo(user.Usuario.ToString());
                if (usuario != null)
                {
                    //Se valida que el usuario esté activo
                    if (usuario.IdEstado == 1)
                    {
                        Usuarios _loggedUser = iUsuarios.LogIn(user.Usuario, user.Contrasenia, _configuration);
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

        [HttpPost]
        [Route("encriptString")]
        public IActionResult encriptString([FromBody] String _cadena)
        {
            try
            {
                Reply _rp = new Reply
                {
                    Resultado = 0,
                    Datos = crypto.Encrypt(_cadena)
                };

                return Ok(_rp);
            }
            catch (Exception ex)
            {
                Reply _rp = new Reply { Resultado = 0 };
                _rp.Mensaje = ex.Message;
                return Ok(_rp);
            }
        }

        [HttpPost]
        [Route("decriptString")]
        public IActionResult decriptString([FromBody] String _cadena)
        {
            try
            {
                Reply _rp = new Reply
                {
                    Resultado = 0,
                    Datos = crypto.Decrypt(_cadena)
                };

                return Ok(_rp);
            }
            catch (Exception ex)
            {
                Reply _rp = new Reply { Resultado = 0 };
                _rp.Mensaje = ex.Message;
                return Ok(_rp);
            }
        }
    }
}