using SysBiblioteca.API.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using SysBiblioteca.API.Management;
using SysBiblioteca.API.Models.PRS;
using SysBiblioteca.API.Models.ADM;
using SysBiblioteca.API.Models.INV;
using SysBiblioteca.API.Services.INV.LibrosService;
using SysBiblioteca.API.Services.ADM.UsuariosService;
using SysBiblioteca.API.Services.PRS.PrestamosService;
using SysBiblioteca.API.Services.INV.EjemplaresService;
using SysBiblioteca.API.Services.ADM.LinkRolMenuService;
using SysBiblioteca.API.Services.INV.AutoresLibrosService;

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
        private readonly iEjemplaresService iEjemplaresService;
        private readonly iLinkRolMenuService iLinkRolMenuService;
        private readonly iAutoresLibrosService iAutoresLibrosService;
        public PrestamosController(iUsuariosService usuariosService, iLinkRolMenuService linkRolMenuService, 
            iPrestamosService prestamosService, iLibrosService librosService, iAutoresLibrosService autoresLibrosService, 
            iEjemplaresService ejemplaresService)
        {
            iLibros = librosService;
            iUsuarios = usuariosService;
            iPrestamosService = prestamosService;
            iLinkRolMenuService = linkRolMenuService;
            iAutoresLibrosService = autoresLibrosService;
            iEjemplaresService = ejemplaresService;
        }

        [HttpPost]
        [Route("ProcesarPrestamo")]
        // SysBiblioteca/API/Prestamos/ProcesarPrestamo
        // Método que solicita el préstamo o la reserva de un libro
        public IActionResult ProcesarPrestamo([FromBody] PrestamosDTO _prestamo, bool esReserva)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_prestamo.Token == null)
                {
                    _rp.Mensaje = "Usuario no autenticado, inicie sesión nuevamente.";
                    return BadRequest(_rp);
                }

                Usuarios user = iUsuarios.getTokenActual(_prestamo.Token);
                if (user == null)
                {
                    _rp.Mensaje = "Usuario no autenticado, inicie sesión nuevamente.";
                    return BadRequest(_rp);
                }

                Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _prestamo.ActualRute);
                if (permisos == null || !permisos.Create)
                {
                    _rp.Mensaje = "El usuario no tiene permisos de escritura en esta pantalla.";
                    return Forbid();
                }

                // Verifica que no haya un préstamo activo para el mismo libro y usuario
                Prestamos prestamo = iPrestamosService.validatePrestamo(_prestamo.IdLibro, _prestamo.IdUsuario);
                if (prestamo != null)
                {
                    _rp.Mensaje = "Parece ser que el usuario tiene un préstamo activo con este libro.";
                    return Conflict(_rp);
                }

                // Verifica si hay unidades disponibles del libro
                Ejemplares ejemplar = iEjemplaresService.getEjemplarToLoan(_prestamo.IdLibro);
                if (ejemplar == null)
                {
                    _rp.Mensaje = "Parece que no quedan unidades de este libro.";
                    return NotFound(_rp);
                }

                // Crea un nuevo préstamo o reserva dependiendo del parámetro `esReserva`
                Prestamos newPrestamo = new Prestamos
                {
                    DiasPrestamo = _prestamo.DiasPrestamo,
                    IdEjemplar = ejemplar.IdEjemplar,
                    IdUsuario = !esReserva ? _prestamo.IdUsuario : user.IdUsuario,
                    Entregado = !esReserva,
                    FechaPrestamo = esReserva ? (DateTime?)null : DateTime.Now,
                    IdUsuarioEntrego = esReserva ? (long?)null : user.IdUsuario,
                    Finalizado = false,
                };
                iPrestamosService.Create(newPrestamo);

                if (newPrestamo.IdPrestamo > 0)
                {
                    _rp.Resultado = 1;
                    _rp.Mensaje = esReserva
                        ? "Se reservó el libro correctamente, puede pasar a la biblioteca a recogerlo."
                        : "Préstamo registrado correctamente.";
                    return Ok(_rp);
                }
                else
                {
                    _rp.Mensaje = "Parece que hubo un error y el préstamo no pudo ser procesado correctamente, intente nuevamente.";
                    return BadRequest(_rp);
                }
            }
            catch (Exception ex)
            {
                _rp.Mensaje = "Ocurrió un error inesperado. Por favor, intente nuevamente.";
                return BadRequest(_rp);
            }
        }

        [HttpPost]
        [Route("PrestamoRFID")]
        // SysBiblioteca/API/Prestamos/PrestamoRFID
        // Método que solicita el préstamo o la reserva de un libro
        public IActionResult PrestamoRFID([FromBody] PrestamosDTO _prestamo)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_prestamo.Token == null)
                {
                    _rp.Mensaje = "Usuario no autenticado, inicie sesión nuevamente.";
                    return Ok(_rp);
                }

                Usuarios user = iUsuarios.getTokenActual(_prestamo.Token);
                if (user == null)
                {
                    _rp.Mensaje = "Usuario no autenticado, inicie sesión nuevamente.";
                    return Ok(_rp);
                }

                if (_prestamo.ActualRute != "/PrestamosDevoluciones/EventHandler" && (user.Rol.Rol != "Administrador" || user.Rol.Rol != "Empleado"))
                {
                    _rp.Mensaje = "El usuario no tiene permisos de escritura en esta pantalla.";
                    return Ok(_rp);
                }

                Prestamos ongoingPrestamo = null;
                if (_prestamo.IdPrestamo > 0)
                {
                    ongoingPrestamo = iPrestamosService.getById(_prestamo.IdPrestamo);
                }
                // Verifica que no haya un préstamo activo para el mismo libro y usuario
                else
                {
                    ongoingPrestamo = iPrestamosService.validatePrestamo(_prestamo.IdLibro, _prestamo.IdUsuario);
                    if (ongoingPrestamo != null && ongoingPrestamo.IdEjemplar != _prestamo.IdEjemplar)
                    {
                        _rp.Mensaje = "Parece ser que el usuario tiene un préstamo activo con este libro.";
                        return Ok(_rp);
                    }
                }

                // Es un préstamo nuevo
                if (ongoingPrestamo == null)
                {
                    Prestamos newPrestamo = new Prestamos {
                        IdUsuario = _prestamo.IdUsuario,
                        IdEjemplar = _prestamo.IdEjemplar,
                        DiasPrestamo = _prestamo.DiasPrestamo,

                        Entregado = true,
                        Finalizado = false,
                        FechaPrestamo = DateTime.Now,
                        IdUsuarioEntrego = user.IdUsuario
                    };

                    iPrestamosService.Create(newPrestamo);
                    if (newPrestamo.IdPrestamo > 0)
                    {
                        _rp.Resultado = 1;
                        _rp.Mensaje = "Préstamo registrado correctamente.";
                        return Ok(_rp);
                    }
                    else
                    {
                        _rp.Mensaje = "Parece que hubo un error y el préstamo no pudo ser procesado correctamente, intente nuevamente.";
                        return BadRequest(_rp);
                    }
                }
                // Es la entrega de un libro reservado
                else
                {
                    iPrestamosService.LoanBook(ongoingPrestamo.IdPrestamo, user.IdUsuario);

                    _rp.Resultado = 1;
                    _rp.Mensaje = "Libro marcado como entregado.";
                    return Ok(_rp);
                }
            }
            catch (Exception ex)
            {
                _rp.Mensaje = "Ocurrió un error inesperado. Por favor, intente nuevamente.";
                return BadRequest(_rp);
            }
        }

        #region Gestión de Prestamos para Usuarios Internos

        [HttpPost]
        [Route("GetPendingLoans")]
        // SysBiblioteca/API/Prestamos/GetPendingLoans
        // Método que obtiene el listado de TODOS los Prestamos Pendientes de entregar
        public IActionResult GetPendingLoans([FromBody] Prestamos _prestamos)
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
                            IEnumerable<Prestamos> prestamos = iPrestamosService.GetPendingLoans();
                            if (prestamos.Count() > 0)
                            {
                                List<MisPrestamosDTO> misPrestamos = new List<MisPrestamosDTO>();
                                foreach (var item in prestamos)
                                {
                                    misPrestamos.Add(new MisPrestamosDTO
                                    {
                                        IdPrestamo = item.IdPrestamo,
                                        Libro = item.Ejemplar.Libro,
                                        DiasPrestamo = item.DiasPrestamo,
                                        Estado = "Pendiente",
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

        [HttpPost]
        [Route("GetOngoingLoans")]
        // SysBiblioteca/API/Prestamos/GetOngoingLoans
        // Método que obtiene el listado de TODOS los Prestamos que han sido entregados
        public IActionResult GetOngoingLoans([FromBody] Prestamos _prestamos)
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
                            IEnumerable<Prestamos> prestamos = iPrestamosService.GetOngoingLoans();
                            if (prestamos.Count() > 0)
                            {
                                List<MisPrestamosDTO> misPrestamos = new List<MisPrestamosDTO>();
                                foreach (var item in prestamos)
                                {
                                    misPrestamos.Add(new MisPrestamosDTO
                                    {
                                        IdPrestamo = item.IdPrestamo,
                                        Libro = item.Ejemplar.Libro,
                                        DiasPrestamo = item.DiasPrestamo,
                                        FechaPrestamo = item.FechaPrestamo,
                                        Estado = getStatus(item.DiasPrestamo, item.FechaPrestamo, item.Entregado),
                                        Usuario = item.Usuario.Usuario,
                                        UsuarioEntrego = item.UsuarioEntrego.Usuario,
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

        [HttpPost]
        [Route("GetUserForLoans")]
        // SysBiblioteca/API/Prestamos/GetUserForLoans
        // Método que obtiene el listado de los usuarios a los que se le puede hacer prestamos
        public IActionResult GetUserForLoans([FromBody] Prestamos _prestamos)
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
                        if (permisos != null && permisos.Read || _prestamos.ActualRute == "/PrestamosDevoluciones/EventHandler")
                        {
                            IEnumerable<Usuarios> usuarios = iPrestamosService.getUserForLoans();
                            if (usuarios.Count() > 0)
                            {
                                _rp.Datos = usuarios;
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

        [HttpPost]
        [Route("GetLoan")]
        // SysBiblioteca/API/Prestamos/GetLoan
        // Método que obtiene los datos del prestamo seleccionado
        public IActionResult GetLoan([FromBody] Prestamos _prestamo)
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
                            Prestamos prestamo = iPrestamosService.getById(_prestamo.IdPrestamo);
                            if (prestamo != null)
                            {
                                _rp.Resultado = 1;
                                _rp.Datos = prestamo;
                                return Ok(_rp);
                            }
                            else
                            {
                                _rp.Mensaje = "No se pudieron encontrar los datos del prestamo, intente nuevamente.";
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
        [Route("LoanBook")]
        // SysBiblioteca/API/Prestamos/LoanBook
        // Método que marca como entregado el prestamo seleccionado
        public IActionResult LoanBook([FromBody] Prestamos _prestamo)
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
                        if (permisos != null && permisos.Update)
                        {
                            Prestamos prestamo = iPrestamosService.getById(_prestamo.IdPrestamo);
                            if (prestamo != null)
                            {
                                iPrestamosService.LoanBook(prestamo.IdPrestamo, user.IdUsuario);

                                _rp.Resultado = 1;
                                _rp.Mensaje = "Libro marcado como entregado.";
                                return Ok(_rp);
                            }
                            else
                            {
                                _rp.Mensaje = "No se pudieron encontrar los datos del prestamo, intente nuevamente.";
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
                                        Libro = item.Ejemplar.Libro,
                                        Autores = getAutoresName(iAutoresLibrosService.getAutoresLibro(item.Ejemplar.IdLibro)),
                                        DiasPrestamo = item.DiasPrestamo,
                                        FechaPrestamo = item.FechaPrestamo,
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

        private string getAutoresName(List<Autores> autoresLibros)
        {
            String autores = String.Empty;
            foreach (var autor in autoresLibros)
            {
                autores += autor.Autor + ", ";
            }
            return autores.Remove(autores.Length - 2);
        }

        private string getStatus(int? diasPrestamo, DateTime? fechaPrestamo, Boolean? Entregado)
        {
            DateTime fechaActual = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            DateTime fechaDevolucion = Convert.ToDateTime(fechaPrestamo.Value.AddDays((double)diasPrestamo).ToShortDateString());
            
            return Entregado.Value ? (fechaActual <= fechaDevolucion ? "A tiempo" : "Demorado") : "Pendiente" ;
        }
    }
}