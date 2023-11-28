using SysBiblioteca.API.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using SysBiblioteca.API.Management;
using SysBiblioteca.API.Models.ADM;
using SysBiblioteca.API.Services.ADM.UsuariosService;
using SysBiblioteca.API.Services.ADM.LinkRolMenuService;
using SysBiblioteca.API.Services.ADM.DatosPersonalesService;

namespace SysBiblioteca.API.Controllers
{
    [ApiController]
    [EnableCors("SysBiblioteca_Policy")]
    [Route("SysBiblioteca/API/[controller]")]
    public class UsuariosExternosController : ControllerBase
    {
        private readonly iUsuariosService iUsuarios;
        private readonly iLinkRolMenuService iLinkRolMenuService;
        private readonly iDatosPersonalesService iDatosPersonalesService;

        public UsuariosExternosController(iUsuariosService usuariosService, iLinkRolMenuService linkRolMenuService, iDatosPersonalesService datosPersonalesService)
        {
            iUsuarios = usuariosService;
            iLinkRolMenuService = linkRolMenuService;
            iDatosPersonalesService = datosPersonalesService;
        }

        [HttpPost]
        [Route("GetMyData")]
        // SysBiblioteca/API/UsuariosExternos/GetMyData
        // Método que obtiene los datos del Usuario
        public IActionResult GetMyData([FromBody] Usuarios _usuario)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_usuario.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_usuario.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _usuario.ActualRute);
                        if (permisos != null && permisos.Read)
                        {
                            Usuarios usuario = iUsuarios.getById(user.IdUsuario);
                            if (usuario != null)
                            {
                                _rp.Datos = usuario;
                                _rp.Resultado = 1;
                                return Ok(_rp);
                            }
                            else
                            {
                                _rp.Mensaje = "No se pudo cargar la información del perfil, intente nuevamente.";
                                return Ok(_rp);
                            }
                        }
                        else
                        {
                            _rp.Mensaje = "El usuario no tiene permisos de lectura en ésta pantalla.";
                            return Ok(_rp);
                        }
                    }
                    else
                    {
                        _rp.Mensaje = "Usuario no autenticado, inicie sesión nuevamente.";
                        return Ok(_rp);
                    }
                }
                else
                {
                    _rp.Mensaje = "Usuario no autenticado, inicie sesión nuevamente.";
                    return Ok(_rp);
                }
            }
            catch (Exception ex)
            {
                _rp.Mensaje = ex.Message;
                return Ok(_rp);
            }
        }

        [HttpPost]
        [Route("UpdateMyData")]
        // SysBiblioteca/API/UsuariosExternos/UpdateMyData
        // Método que modifica la contraseña del usuario
        public IActionResult UpdateMyData([FromBody] Usuarios _usuario)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_usuario.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_usuario.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _usuario.ActualRute);
                        if (permisos != null && permisos.Update)
                        {
                            Usuarios usuario = iUsuarios.getById(user.IdUsuario);
                            if (usuario != null)
                            {
                                iDatosPersonalesService.UpdateMyData(new DatosPersonales
                                {
                                    IdDatosPersonales = usuario.IdDatosPersonales,
                                    Nombres = _usuario.DatosPersonales.Nombres,
                                    Apellidos = _usuario.DatosPersonales.Apellidos,
                                    DUI = _usuario.DatosPersonales.DUI,
                                    Correo = _usuario.DatosPersonales.Correo,
                                    Direccion = _usuario.DatosPersonales.Direccion,
                                    Telefono = _usuario.DatosPersonales.Telefono,
                                    FechaNacimiento = _usuario.DatosPersonales.FechaNacimiento,
                                    IdGenero = _usuario.DatosPersonales.IdGenero
                                });

                                _rp.Resultado = 1;
                                _rp.Mensaje = "Informaci&oacute;n actualizada con &eacute;xito.";
                                return Ok(_rp);
                            }
                            else
                            {
                                _rp.Mensaje = "No se pudo cargar la informaci&oacute;n del perfil, intente nuevamente.";
                                return Ok(_rp);
                            }
                        }
                        else
                        {
                            _rp.Mensaje = "El usuario no tiene permisos de lectura en esta pantalla.";
                            return Ok(_rp);
                        }
                    }
                    else
                    {
                        _rp.Mensaje = "Usuario no autenticado, inicie sesión nuevamente.";
                        return Ok(_rp);
                    }
                }
                else
                {
                    _rp.Mensaje = "Usuario no autenticado, inicie sesión nuevamente.";
                    return Ok(_rp);
                }
            }
            catch (Exception ex)
            {
                _rp.Mensaje = ex.Message;
                return Ok(_rp);
            }
        }

        [HttpPost]
        [Route("ChangeMyPassword")]
        // SysBiblioteca/API/UsuariosExternos/ChangeMyPassword
        // Método que modifica la contraseña del usuario
        public IActionResult ChangeMyPassword([FromBody] PasswordDTO _password)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_password.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_password.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _password.ActualRute);
                        if (permisos != null && permisos.Update)
                        {
                            Usuarios usuario = iUsuarios.getById(user.IdUsuario);
                            if (usuario != null)
                            {
                                if (crypto.Decrypt(user.Contrasenia) == _password.OldPassword)
                                {
                                    iUsuarios.changePassword(user.IdUsuario, crypto.Encrypt(_password.NewPassword));

                                    _rp.Mensaje = "Contraseña actualizada con exito";
                                    _rp.Resultado = 1;
                                    return Ok(_rp);
                                }
                                else
                                {
                                    _rp.Mensaje = "La contraseña anterior no es correcta.";
                                    return Ok(_rp);
                                }
                            }
                            else
                            {
                                _rp.Mensaje = "No se pudo cargar la información del perfil, intente nuevamente.";
                                return Ok(_rp);
                            }
                        }
                        else
                        {
                            _rp.Mensaje = "El usuario no tiene permisos de lectura en ésta pantalla.";
                            return Ok(_rp);
                        }
                    }
                    else
                    {
                        _rp.Mensaje = "Usuario no autenticado, inicie sesión nuevamente.";
                        return Ok(_rp);
                    }
                }
                else
                {
                    _rp.Mensaje = "Usuario no autenticado, inicie sesión nuevamente.";
                    return Ok(_rp);
                }
            }
            catch (Exception ex)
            {
                _rp.Mensaje = ex.Message;
                return Ok(_rp);
            }
        }
    }
}