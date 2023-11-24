using SysBiblioteca.API.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using SysBiblioteca.API.Management;
using SysBiblioteca.API.Models.PRS;
using SysBiblioteca.API.Models.ADM;
using SysBiblioteca.API.Services.ADM.UsuariosService;
using SysBiblioteca.API.Services.PRS.PrestamosService;
using SysBiblioteca.API.Services.ADM.LinkRolMenuService;
using SysBiblioteca.API.Services.INV.LibrosService;
using SysBiblioteca.API.Models.INV;

namespace SysBiblioteca.API.Controllers
{
    [ApiController]
    [EnableCors("SysBiblioteca_Policy")]
    [Route("SysBiblioteca/API/[controller]")]
    public class PrestamosController : ControllerBase
    {
        private readonly iLibrosService iLibros;
        private readonly iUsuariosService iUsuarios;
        private readonly iPrestamosService iPrestamosService;
        private readonly iLinkRolMenuService iLinkRolMenuService;
        public PrestamosController(iUsuariosService usuariosService, iLinkRolMenuService linkRolMenuService, iPrestamosService prestamosService, iLibrosService librosService)
        {
            iLibros = librosService;
            iUsuarios = usuariosService;
            iPrestamosService = prestamosService;
            iLinkRolMenuService = linkRolMenuService;
        }

        #region Gestión de Prestamos para Usuarios Internos

        [HttpPost]
        [Route("CreatePrestamo")]
        // SysBiblioteca/API/Prestamos/CreatePrestamo
        // Método que obtiene el listado de TODOS los Prestamos Activos
        public IActionResult CreatePrestamo([FromBody] Prestamos _prestamo)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_prestamo.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_prestamo.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _prestamo.ActualRute);
                        if (permisos != null && permisos.Create)
                        {
                            // Primero se valida que aún existan unidades del libro en el sistema
                            Libros libro = iLibros.getById(_prestamo.IdLibro);
                            if (libro != null && libro.Cantidad > 0)
                            {
                                Prestamos newPrestamo = new Prestamos { 
                                    DiasPrestamo = _prestamo.DiasPrestamo,

                                    Entregado = true,
                                    FechaPrestamo = DateTime.Now,
                                    IdLibro = _prestamo.IdLibro,
                                    IdUsuario = _prestamo.IdUsuario,
                                    IdUsuarioEntrego = user.IdUsuario
                                };
                                iPrestamosService.Create(newPrestamo);

                                if (newPrestamo.IdPrestamo > 0)
                                {
                                    _rp.Resultado = 1;
                                    _rp.Mensaje = "Se guard&oacute; correctamente el prestamo.";
                                    return Ok(_rp);
                                }
                                else
                                {
                                    _rp.Mensaje = "Parece que hubo un error y el prestamo no pudo ser procesado correctamente, intente nuevamente.";
                                    return Ok(_rp);
                                }
                            }
                            else
                            {
                                _rp.Mensaje = "Parece que no quedan unidades de éste libro.";
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
        [Route("GetActivePrestamos")]
        // SysBiblioteca/API/Prestamos/GetActivePrestamos
        // Método que obtiene el listado de TODOS los Prestamos Activos
        public IActionResult GetActivePrestamos([FromBody] Prestamos _prestamos)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_prestamos.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_prestamos.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _prestamos.ActualRute);
                        if (permisos != null && permisos.Read)
                        {
                            IEnumerable<Prestamos> prestamos = iPrestamosService.Read();
                            if (prestamos.Count() > 0)
                            {
                                List<MisPrestamosDTO> misPrestamos = new List<MisPrestamosDTO>();
                                foreach (var item in prestamos)
                                {
                                    misPrestamos.Add(new MisPrestamosDTO
                                    {
                                        IdPrestamo = item.IdPrestamo,
                                        Libro = item.Libro,
                                        DiasPrestamo = item.DiasPrestamo,
                                        FechaPrestamo = item.FechaPrestamo,
                                        FechaDevolucion = item.FechaDevolucion,
                                        Estado = getStatus(item.DiasPrestamo, item.FechaPrestamo, item.Entregado),
                                        Usuario = item.Usuario.Usuario
                                    });
                                }

                                _rp.Datos = misPrestamos;
                                _rp.Resultado = 1;
                                return Ok(_rp);
                            }
                            else
                            {
                                _rp.Mensaje = "Sin datos que mostrar.";
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

        #endregion

        #region Gestion de Prestamos para Usuario Externo

        [HttpPost]
        [Route("GetMyBooks")]
        // SysBiblioteca/API/Prestamos/GetMyBooks
        // Método que obtiene el listado de Prestamos Activos para el Usuario
        public IActionResult GetMyBooks([FromBody] Prestamos _prestamos)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_prestamos.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_prestamos.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _prestamos.ActualRute);
                        if (permisos != null && permisos.Read)
                        {
                            IEnumerable<Prestamos> prestamos = iPrestamosService.GetMyBooks(user.IdUsuario);
                            if (prestamos.Count() > 0)
                            {
                                List<MisPrestamosDTO_User> misPrestamos = new List<MisPrestamosDTO_User>();
                                foreach (var item in prestamos)
                                {
                                    misPrestamos.Add(new MisPrestamosDTO_User
                                    {
                                        IdPrestamo = item.IdPrestamo,
                                        Libro = item.Libro,
                                        DiasPrestamo = item.DiasPrestamo,
                                        FechaPrestamo = item.FechaPrestamo,
                                        FechaDevolucion = item.FechaDevolucion,
                                        Estado = getStatus(item.DiasPrestamo, item.FechaPrestamo, item.Entregado)
                                    });
                                }

                                _rp.Datos = misPrestamos;
                                _rp.Resultado = 1;
                                return Ok(_rp);
                            }
                            else
                            {
                                _rp.Mensaje = "Sin datos que mostrar.";
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

        #endregion

        private string getStatus(int? diasPrestamo, DateTime? fechaPrestamo, Boolean? Entregado)
        {
            DateTime fechaActual = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            DateTime fechaDevolucion = Convert.ToDateTime(fechaPrestamo.Value.AddDays((double)diasPrestamo).ToShortDateString());
            
            return Entregado.Value ? (fechaActual <= fechaDevolucion ? "A tiempo" : "Demorado") : "No Entregado" ;
        }
    }
}