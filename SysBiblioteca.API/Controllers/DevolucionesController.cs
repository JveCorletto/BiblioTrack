using SysBiblioteca.API.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using SysBiblioteca.API.Management;
using SysBiblioteca.API.Models.ADM;
using SysBiblioteca.API.Models.PRS;
using SysBiblioteca.API.Services.PRS.MultasService;
using SysBiblioteca.API.Services.INV.LibrosService;
using SysBiblioteca.API.Services.ADM.UsuariosService;
using SysBiblioteca.API.Services.PRS.PrestamosService;
using SysBiblioteca.API.Services.ADM.LinkRolMenuService;

namespace SysBiblioteca.API.Controllers
{
    [ApiController]
    [EnableCors("SysBiblioteca_Policy")]
    [Route("SysBiblioteca/API/[controller]")]
    public class DevolucionesController : ControllerBase
    {
        private readonly iUsuariosService iUsuarios;
        private readonly iLibrosService iLibrosService;
        private readonly iMultasService iMultasService;
        private readonly iPrestamosService iPrestamosService;
        private readonly iLinkRolMenuService iLinkRolMenuService;
        public DevolucionesController(iUsuariosService usuariosService, iLinkRolMenuService linkRolMenuService, 
            iPrestamosService prestamosService, iMultasService multasService, iLibrosService librosService)
        {
            iUsuarios = usuariosService;
            iPrestamosService = prestamosService;
            iLinkRolMenuService = linkRolMenuService;
            iMultasService = multasService;
            iLibrosService = librosService;
        }

        [HttpPost]
        [Route("GetLoan")]
        // SysBiblioteca/API/Devoluciones/GetLoan
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
                                PrestamoDTO prestamoDTO = new PrestamoDTO
                                {
                                    IdPrestamo = prestamo.IdPrestamo,
                                    Libro = prestamo.Libro,
                                    DiasPrestamo = prestamo.DiasPrestamo,
                                    DUI = prestamo.Usuario.DatosPersonales.DUI,
                                    Usuario = prestamo.Usuario.Usuario,
                                    Telefono = prestamo.Usuario.DatosPersonales.Telefono,
                                    Correo = prestamo.Usuario.DatosPersonales.Correo,

                                    FechaPrestamo = prestamo.FechaPrestamo,
                                    UsuarioEntrego = prestamo.UsuarioEntrego != null ? prestamo.UsuarioEntrego.Usuario : "Sin registro",
                                    Estado = getStatus(prestamo.DiasPrestamo, prestamo.FechaPrestamo, prestamo.Entregado)
                                };

                                _rp.Resultado = 1;
                                _rp.Datos = prestamoDTO;
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
        [Route("FinishLoan")]
        // SysBiblioteca/API/Devoluciones/FinishLoan
        // Método que obtiene los datos del prestamo seleccionado
        public IActionResult FinishLoan([FromBody] Prestamos _prestamo)
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
                                if (getStatus(prestamo.DiasPrestamo, prestamo.FechaPrestamo, prestamo.Entregado) == "Demorado")
                                {
                                    DateTime fechaActual = DateTime.Now;
                                    TimeSpan diferencia = fechaActual - prestamo.FechaPrestamo.Value;
                                    int diasExcedidos = diferencia.Days;

                                    double penalizacion = 0.5 * diasExcedidos;
                                    penalizacion = Math.Max(0, penalizacion);

                                    _rp.Resultado = 2;
                                    _rp.Datos = new { 
                                        prestamo.Usuario.Usuario, 
                                        prestamo.DiasPrestamo,
                                        prestamo.FechaPrestamo,
                                        diasExcedidos, 
                                        penalizacion };
                                    _rp.Mensaje = $"Parece que el Usuario {prestamo.Usuario.Usuario} tiene un retraso de {diasExcedidos} días en su prestamo";
                                    return Ok(_rp);
                                }
                                else
                                {
                                    iPrestamosService.MarkAsFinished(prestamo.IdPrestamo, user.IdUsuario);
                                    iLibrosService.agregarUnidad(prestamo.IdLibro);

                                    _rp.Resultado = 1;
                                    _rp.Mensaje = "Pr&eacute;stamo finalizado con &eacute;xito";
                                    return Ok(_rp);
                                }
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
        [Route("PayFine")]
        // SysBiblioteca/API/Devoluciones/PayFine
        // Método que paga la multa del prestamo y marca como finalizado el prestamo
        public IActionResult PayFine([FromBody] Multas _multa)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_multa.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_multa.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _multa.ActualRute);
                        if (permisos != null && permisos.Create)
                        {
                            Prestamos prestamo = iPrestamosService.getById(_multa.IdPrestamo);
                            if (prestamo != null)
                            {
                                Multas newMulta = new Multas {
                                    IdEstadoMulta = 3,
                                    IdPrestamo = prestamo.IdPrestamo,
                                    PagoFisico = true,
                                    Monto = _multa.Monto,
                                    DiasRetraso = _multa.DiasRetraso,
                                    FechaValidacion = DateTime.Now,
                                    IdUsuarioValidacion = user.IdUsuario
                                };

                                iMultasService.Create(newMulta);
                                if (newMulta.IdMulta > 0)
                                {
                                    iPrestamosService.MarkAsFinished(prestamo.IdPrestamo, user.IdUsuario);
                                    iLibrosService.agregarUnidad(prestamo.IdLibro);

                                    _rp.Resultado = 1;
                                    _rp.Mensaje = "Pr&eacute;stamo finalizado con &eacute;xito";
                                    return Ok(_rp);
                                }
                                else
                                {
                                    _rp.Mensaje = "No se pudo registrar el pago de la multa, intente nuevamente.";
                                    return Ok(_rp);
                                }
                            }
                            else
                            {
                                _rp.Mensaje = "No se pudieron encontrar los datos del prestamo, intente nuevamente.";
                                return Ok(_rp);
                            }
                        }
                        else
                        {
                            _rp.Mensaje = "El usuario no tiene permisos de escritura en ésta pantalla.";
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
        [Route("GetFinishedLoans")]
        // SysBiblioteca/API/Devoluciones/GetFinishedLoans
        // Método que obtiene los datos del prestamo seleccionado
        public IActionResult GetFinishedLoans([FromBody] Prestamos _prestamo)
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
                        if (permisos != null && permisos.Read)
                        {
                            IEnumerable<Prestamos> prestamos = iPrestamosService.GetFinishedLoans();
                            if (prestamos.Count() > 0)
                            {
                                List<PrestamoDTO> misPrestamos = new List<PrestamoDTO>();
                                foreach (var item in prestamos)
                                {
                                    misPrestamos.Add(new PrestamoDTO
                                    {
                                        IdPrestamo = item.IdPrestamo,
                                        Libro = item.Libro,
                                        DiasPrestamo = item.DiasPrestamo,
                                        FechaPrestamo = item.FechaPrestamo,
                                        FechaDevolucion = item.FechaDevolucion,
                                        Estado = "Finalizado",
                                        Usuario = item.Usuario.Usuario,
                                        UsuarioEntrego = item.UsuarioEntrego.Usuario,
                                        UsuarioRecibio = item.UsuarioRecibio.Usuario
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

        private string getStatus(int? diasPrestamo, DateTime? fechaPrestamo, Boolean? Entregado)
        {
            DateTime fechaActual = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            DateTime fechaDevolucion = Convert.ToDateTime(fechaPrestamo.Value.AddDays((double)diasPrestamo).ToShortDateString());

            return Entregado.Value ? (fechaActual <= fechaDevolucion ? "A tiempo" : "Demorado") : "Pendiente";
        }
    }
}