using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using SysBiblioteca.API.Management;
using SysBiblioteca.API.Models.ADM;
using SysBiblioteca.API.Services.ADM.UsuariosService;
using SysBiblioteca.API.Services.ADM.DatosPersonalesService;

namespace SysBiblioteca.API.Controllers
{
    [ApiController]
    [Route("SysBiblioteca/API/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly iUsuariosService iUsuarios;
        private readonly IConfiguration _configuration;
        private readonly iDatosPersonalesService iDatosPersonalesService;

        public AuthenticationController(IConfiguration configuration, iUsuariosService usuariosService, iDatosPersonalesService datosPersonalesService)
        {
            iUsuarios = usuariosService;
            _configuration = configuration;
            iDatosPersonalesService = datosPersonalesService;
        }

        [HttpPost]
        [Route("LogIn")]
        // SysBiblioteca/API/Authentication/LogIn
        // Método para el inicio de Sesión de un usuario
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

        [HttpPost]
        [Route("LogOut")]
        // SysBiblioteca/API/Authentication/LogOut
        // Método para el cierre de Sesión de un usuario
        public IActionResult LogOut([FromBody] Object _user)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(_user.ToString());

                Usuarios usuario = iUsuarios.getUserInfo(data.Usuario.ToString());
                if (usuario != null)
                {
                    iUsuarios.LogOut(usuario);
                    _rp.Resultado = 1;
                    _rp.Mensaje = $"Hasta pronto {usuario.Usuario}";
                    return Ok(_rp);
                }
                else
                {
                    _rp.Mensaje = "Error en la petición.";
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
        [Route("Register")]
        // SysBiblioteca/API/Authentication/Register
        // Método para el registro de una persona (nuevo usuario)
        public IActionResult Register([FromBody] Usuarios _newUser)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_newUser != null)
                {
                    //Se valida que no haya un usuario con el mismo nombre de usuario ingresado
                    Usuarios usuario = iUsuarios.getUserInfo(_newUser.Usuario.ToString());
                    DatosPersonales datosPersonales = iDatosPersonalesService.getByDUI(_newUser.DatosPersonales.DUI.ToString());

                    if (datosPersonales == null && usuario == null)
                    {
                        DatosPersonales newPeronsa = new DatosPersonales
                        {
                            Nombres = _newUser.DatosPersonales.Nombres.ToString(),
                            Apellidos = _newUser.DatosPersonales.Apellidos.ToString(),
                            DUI = _newUser.DatosPersonales.DUI.ToString(),
                            Correo = _newUser.DatosPersonales.Correo.ToString(),
                            Direccion = _newUser.DatosPersonales.Direccion.ToString(),
                            Telefono = _newUser.DatosPersonales.Telefono.ToString(),
                            FechaNacimiento = _newUser.DatosPersonales.FechaNacimiento.ToString()
                        };

                        iDatosPersonalesService.Create(newPeronsa);
                        if (newPeronsa.IdDatosPersonales > 0)
                        {
                            Usuarios newUsuario = new Usuarios
                            {
                                IdRol = 3,
                                IdEstado = 1,
                                IdDatosPersonales = newPeronsa.IdDatosPersonales,

                                Usuario = _newUser.Usuario.ToString(),
                                Contrasenia = crypto.Encrypt(_newUser.Contrasenia.ToString()),

                                UsuarioCreacion = "UIRegistered",
                                FechaCreacion = DateTime.Now,
                            };

                            iUsuarios.Create(newUsuario);
                            if (newUsuario.IdUsuario > 0)
                            {
                                _rp.Resultado = 1;
                                _rp.Mensaje = "Usuario registrado con éxito";
                                return Ok(_rp);
                            }
                            else
                            {
                                _rp.Resultado = 3;
                                _rp.Mensaje = "Ocurrió un error y el usuario no se registró completamente";
                                return Ok(_rp);
                            }
                        }
                        else
                        {
                            _rp.Resultado = 2;
                            _rp.Mensaje = "Ocurrió un error y no se ha podido crear el usuario, intente nuevamente";
                            return Ok(_rp);
                        }
                    }
                    else
                    {
                        _rp.Mensaje = datosPersonales == null && usuario != null ? "Ese nombre de usuario ya existe en el sistema" : "Ya se ha registrado este DUI en el sistema";
                        return BadRequest(_rp);
                    }
                }
                else
                {
                    _rp.Mensaje = "Petición Inválida";
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