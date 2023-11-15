using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using SysBiblioteca.API.Management;
using SysBiblioteca.API.Models.ADM;
using SysBiblioteca.API.Models.INV;
using SysBiblioteca.API.Services.INV.NivelesService;
using SysBiblioteca.API.Services.ADM.UsuariosService;
using SysBiblioteca.API.Services.INV.SeccionesService;
using SysBiblioteca.API.Services.INV.EstanteriasService;
using SysBiblioteca.API.Services.ADM.LinkRolMenuService;

namespace SysBiblioteca.API.Controllers
{
    [ApiController]
    [EnableCors("SysBiblioteca_Policy")]
    [Route("SysBiblioteca/API/[controller]")]
    public class InventarioController : Controller
    {
        private readonly iNivelesService iNivelesService;
        private readonly iUsuariosService iUsuariosService;
        private readonly iSeccionesService iSeccionesService;
        private readonly iLinkRolMenuService iLinkRolMenuService;
        private readonly iEstanteriasService iEstanteriasService;

        public InventarioController(iNivelesService nivelesService, iUsuariosService usuariosService, iLinkRolMenuService linkRolMenuService, iSeccionesService seccionesService, iEstanteriasService estanteriasService)
        {
            iNivelesService = nivelesService;
            iUsuariosService = usuariosService;
            iSeccionesService = seccionesService;
            iLinkRolMenuService = linkRolMenuService;
            iEstanteriasService = estanteriasService;
        }

        #region Secciones

        [HttpPost]
        [Route("GetSecciones")]
        // SysBiblioteca/API/Inventario/GetSecciones
        // Método que obtiene las secciones de la biblioteca
        public IActionResult GetSecciones([FromBody] Secciones _secciones)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_secciones.Token != null)
                {
                    Usuarios user = iUsuariosService.getTokenActual(_secciones.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _secciones.ActualRute);
                        if (permisos != null && permisos.Read)
                        {
                            List<Secciones> secciones = iSeccionesService.Read();
                            _rp.Datos = secciones;
                            _rp.Resultado = 1;

                            return Ok(_rp);
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
        [Route("GetSeccion")]
        // SysBiblioteca/API/Inventario/GetSeccion
        // Método que obtiene los datos de la sección seleccionada
        public IActionResult GetSeccion([FromBody] Secciones _secciones)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_secciones.Token != null)
                {
                    Usuarios user = iUsuariosService.getTokenActual(_secciones.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _secciones.ActualRute);
                        if (permisos != null && permisos.Read)
                        {
                            Secciones seccion = iSeccionesService.getById(_secciones.IdSeccion);

                            if (seccion != null)
                            {
                                _rp.Datos = seccion;
                                _rp.Resultado = 1;
                                return Ok(_rp);
                            }
                            else
                            {
                                _rp.Mensaje = "No se pudo encontrar la secci&oacute;n seleccionada";
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
        [Route("CreateSeccion")]
        // SysBiblioteca/API/Inventario/CreateSeccion
        // Método que crea una nueva sección
        public IActionResult CreateSeccion([FromBody] Secciones _secciones)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_secciones.Token != null)
                {
                    Usuarios user = iUsuariosService.getTokenActual(_secciones.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _secciones.ActualRute);
                        if (permisos != null && permisos.Create)
                        {
                            Secciones validation = iSeccionesService.getByName(_secciones.Seccion);
                            if (validation == null)
                            {
                                iSeccionesService.Create(new Secciones { Seccion = _secciones.Seccion, UsuarioCreacion = user.Usuario, FechaCreacion = DateTime.Now });

                                _rp.Resultado = 1;
                                _rp.Mensaje = "Sección creada con Éxito";

                                return Ok(_rp);
                            }
                            else
                            {
                                _rp.Mensaje = "Secci&oacute;n ya existe";

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
        [Route("UpdateSeccion")]
        // SysBiblioteca/API/Inventario/UpdateSeccion
        // Método que modifica los datos de una sección
        public IActionResult UpdateSeccion([FromBody] Secciones _secciones)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_secciones.Token != null)
                {
                    Usuarios user = iUsuariosService.getTokenActual(_secciones.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _secciones.ActualRute);
                        if (permisos != null && permisos.Update)
                        {
                            Secciones validation = iSeccionesService.getById(_secciones.IdSeccion);
                            if (validation != null)
                            {
                                _secciones.UsuarioModificacion = user.Usuario;
                                _secciones.FechaModificacion = DateTime.Now;
                                iSeccionesService.Update(_secciones);

                                _rp.Resultado = 1;
                                _rp.Mensaje = "Sección modificada con Éxito";

                                return Ok(_rp);
                            }
                            else
                            {
                                _rp.Mensaje = "no se ha podido encontrar la sección especificada, intente nuevamente";
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
        [Route("DeleteSeccion")]
        // SysBiblioteca/API/Inventario/DeleteSeccion
        // Método que elimina una sección
        public IActionResult DeleteSeccion([FromBody] Secciones _secciones)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_secciones.Token != null)
                {
                    Usuarios user = iUsuariosService.getTokenActual(_secciones.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _secciones.ActualRute);
                        if (permisos != null && permisos.Delete)
                        {
                            Secciones validation = iSeccionesService.getById(_secciones.IdSeccion);
                            if (validation != null)
                            {
                                try
                                {
                                    iSeccionesService.Delete(validation);

                                    _rp.Resultado = 1;
                                    _rp.Mensaje = "Secci&oacute;n eliminada con &Eacute;xito";

                                    return Ok(_rp);
                                }
                                catch (Exception ex)
                                {
                                    _rp.Mensaje = "La Secci&oacute;n no pudo ser eliminada, asegurese que no contenga ninguna estanter&iacute;a asignada";
                                    return Ok(_rp);
                                }
                            }
                            else
                            {
                                _rp.Mensaje = "no se ha podido encontrar la secci&pacute;n especificada, intente nuevamente";
                                return Ok(_rp);
                            }
                        }
                        else
                        {
                            _rp.Mensaje = "El usuario no tiene permisos de eliminación en ésta pantalla.";
                            return Ok(_rp);
                        }
                    }
                    else
                    {
                        _rp.Mensaje = "Usuario no autenticado, inicie sesi&pacute;n nuevamente.";
                        return Ok(_rp);
                    }
                }
                else
                {
                    _rp.Mensaje = "Usuario no autenticado, inicie sesi&pacute;n nuevamente.";
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

        #region Estanterías

        [HttpPost]
        [Route("GetEstanterias")]
        // SysBiblioteca/API/Inventario/GetEstanterias
        // Método que obtiene las estanterías de una sección
        public IActionResult GetEstanterias([FromBody] Estanterias _estanterias)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_estanterias.Token != null)
                {
                    Usuarios user = iUsuariosService.getTokenActual(_estanterias.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _estanterias.ActualRute);
                        if (permisos != null && permisos.Read)
                        {
                            List<Estanterias> estanterias = iEstanteriasService.getBySeccion(_estanterias.IdSeccion);
                            _rp.Datos = estanterias;
                            _rp.Resultado = 1;

                            return Ok(_rp);
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
        [Route("GetEstanteria")]
        // SysBiblioteca/API/Inventario/GetEstanteria
        // Método que obtiene los datos de la Estanteria seleccionada
        public IActionResult GetEstanteria([FromBody] Estanterias _estanterias)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_estanterias.Token != null)
                {
                    Usuarios user = iUsuariosService.getTokenActual(_estanterias.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _estanterias.ActualRute);
                        if (permisos != null && permisos.Read)
                        {
                            Estanterias estanteria = iEstanteriasService.getById(_estanterias.IdEstanteria);

                            if (estanteria != null)
                            {
                                _rp.Datos = estanteria;
                                _rp.Resultado = 1;
                                return Ok(_rp);
                            }
                            else
                            {
                                _rp.Mensaje = "No se pudo encontrar la estanter&iacute;a seleccionada";
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
        [Route("CreateEstanteria")]
        // SysBiblioteca/API/Inventario/CreateEstanteria
        // Método que crea una nueva estanteríaa las secciones de la biblioteca
        public IActionResult CreateEstanteria([FromBody] Estanterias _estanterias)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_estanterias.Token != null)
                {
                    Usuarios user = iUsuariosService.getTokenActual(_estanterias.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _estanterias.ActualRute);
                        if (permisos != null && permisos.Create)
                        {
                            iEstanteriasService.Create(new Estanterias { Estanteria = _estanterias.Estanteria, IdSeccion = _estanterias.IdSeccion, UsuarioCreacion = user.Usuario, FechaCreacion = DateTime.Now });

                            _rp.Resultado = 1;
                            _rp.Mensaje = "Estanter&iacute;a creada con Éxito";

                            return Ok(_rp);
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
        [Route("UpdateEstanteria")]
        // SysBiblioteca/API/Inventario/UpdateEstanteria
        // Método que modifica los datos de la estanterái especificada
        public IActionResult UpdateEstanteria([FromBody] Estanterias _estanterias)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_estanterias.Token != null)
                {
                    Usuarios user = iUsuariosService.getTokenActual(_estanterias.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _estanterias.ActualRute);
                        if (permisos != null && permisos.Update)
                        {
                            Estanterias validation = iEstanteriasService.getById(_estanterias.IdEstanteria);
                            if (validation != null)
                            {
                                _estanterias.UsuarioModificacion = user.Usuario;
                                _estanterias.FechaModificacion = DateTime.Now;
                                iEstanteriasService.Update(_estanterias);

                                _rp.Resultado = 1;
                                _rp.Mensaje = "Estanter&iacute;a modificada con Éxito";

                                return Ok(_rp);
                            }
                            else
                            {
                                _rp.Mensaje = "no se ha podido encontrar la Estanter&iacute;a especificada, intente nuevamente";
                                return Ok(_rp);
                            }
                        }
                        else
                        {
                            _rp.Mensaje = "El usuario no tiene permisos de edición en ésta pantalla.";
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
        [Route("DeleteEstanteria")]
        // SysBiblioteca/API/Inventario/DeleteEstanteria
        // Método que elimina una estantería de la sección
        public IActionResult DeleteEstanteria([FromBody] Estanterias _estanterias)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_estanterias.Token != null)
                {
                    Usuarios user = iUsuariosService.getTokenActual(_estanterias.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _estanterias.ActualRute);
                        if (permisos != null && permisos.Delete)
                        {
                            Estanterias validation = iEstanteriasService.getById(_estanterias.IdEstanteria);
                            if (validation != null)
                            {
                                try
                                {
                                    iEstanteriasService.Delete(validation);

                                    _rp.Resultado = 1;
                                    _rp.Mensaje = "Estanter&iacute;a eliminada con &Eacute;xito";

                                    return Ok(_rp);
                                }
                                catch (Exception ex)
                                {
                                    _rp.Mensaje = "La Estanter%iascute;a no pudo ser eliminada, asegurese que no contenga ning&uacute;n nivel asignado";
                                    return Ok(_rp);
                                }
                            }
                            else
                            {
                                _rp.Mensaje = "no se ha podido encontrar la Estanter&iacute;a especificada, intente nuevamente";
                                return Ok(_rp);
                            }
                        }
                        else
                        {
                            _rp.Mensaje = "El usuario no tiene permisos de eliminación en ésta pantalla.";
                            return Ok(_rp);
                        }
                    }
                    else
                    {
                        _rp.Mensaje = "Usuario no autenticado, inicie sesi&pacute;n nuevamente.";
                        return Ok(_rp);
                    }
                }
                else
                {
                    _rp.Mensaje = "Usuario no autenticado, inicie sesi&pacute;n nuevamente.";
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

        #region Estanterías

        [HttpPost]
        [Route("GetNiveles")]
        // SysBiblioteca/API/Inventario/GetNiveles
        // Método que obtiene los niveles de una estanteria
        public IActionResult GetNiveles([FromBody] Niveles _niveles)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_niveles.Token != null)
                {
                    Usuarios user = iUsuariosService.getTokenActual(_niveles.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _niveles.ActualRute);
                        if (permisos != null && permisos.Read)
                        {
                            List<Niveles> niveles = iNivelesService.getByEstanteria(_niveles.IdEstanteria);
                            _rp.Datos = niveles;
                            _rp.Resultado = 1;

                            return Ok(_rp);
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
        [Route("GetNivel")]
        // SysBiblioteca/API/Inventario/GetNivel
        // Método que obtiene los datos del nivel seleccionado
        public IActionResult GetNivel([FromBody] Niveles _niveles)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_niveles.Token != null)
                {
                    Usuarios user = iUsuariosService.getTokenActual(_niveles.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _niveles.ActualRute);
                        if (permisos != null && permisos.Read)
                        {
                            Niveles nivel = iNivelesService.getById(_niveles.IdNivel);

                            if (nivel != null)
                            {
                                _rp.Datos = nivel;
                                _rp.Resultado = 1;
                                return Ok(_rp);
                            }
                            else
                            {
                                _rp.Mensaje = "No se pudo encontrar el nivel seleccionado";
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
        [Route("CreateNivel")]
        // SysBiblioteca/API/Inventario/CreateNivel
        // Método que crea una nuevo nivel a la estanteria seleccionada
        public IActionResult CreateNivel([FromBody] Niveles _niveles)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_niveles.Token != null)
                {
                    Usuarios user = iUsuariosService.getTokenActual(_niveles.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _niveles.ActualRute);
                        if (permisos != null && permisos.Create)
                        {
                            iNivelesService.Create(new Niveles { Nivel = _niveles.Nivel, IdEstanteria = _niveles.IdEstanteria, UsuarioCreacion = user.Usuario, FechaCreacion = DateTime.Now });

                            _rp.Resultado = 1;
                            _rp.Mensaje = "Nivel creado con Éxito";

                            return Ok(_rp);
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
        [Route("UpdateNivel")]
        // SysBiblioteca/API/Inventario/UpdateNivel
        // Método que modifica los datos del nivel seleccionado
        public IActionResult UpdateNivel([FromBody] Niveles _niveles)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_niveles.Token != null)
                {
                    Usuarios user = iUsuariosService.getTokenActual(_niveles.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _niveles.ActualRute);
                        if (permisos != null && permisos.Update)
                        {
                            Niveles validation = iNivelesService.getById(_niveles.IdNivel);
                            if (validation != null)
                            {
                                _niveles.UsuarioModificacion = user.Usuario;
                                _niveles.FechaModificacion = DateTime.Now;
                                iNivelesService.Update(_niveles);

                                _rp.Resultado = 1;
                                _rp.Mensaje = "Nivel modificado con Éxito";

                                return Ok(_rp);
                            }
                            else
                            {
                                _rp.Mensaje = "no se ha podido encontrar el nivel especificado, intente nuevamente";
                                return Ok(_rp);
                            }
                        }
                        else
                        {
                            _rp.Mensaje = "El usuario no tiene permisos de edición en ésta pantalla.";
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
        [Route("DeleteNivel")]
        // SysBiblioteca/API/Inventario/DeleteNivel
        // Método que Elimina un nivel de la estantería
        public IActionResult DeleteNivel([FromBody] Niveles _niveles)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_niveles.Token != null)
                {
                    Usuarios user = iUsuariosService.getTokenActual(_niveles.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _niveles.ActualRute);
                        if (permisos != null && permisos.Delete)
                        {
                            Niveles validation = iNivelesService.getById(_niveles.IdNivel);
                            if (validation != null)
                            {
                                try
                                {
                                    iNivelesService.Delete(validation);

                                    _rp.Resultado = 1;
                                    _rp.Mensaje = "Nivel eliminado con &Eacute;xito";

                                    return Ok(_rp);
                                }
                                catch (Exception ex)
                                {
                                    _rp.Mensaje = "El nivel no pudo ser eliminado, asegurese que no contenga ning&uacute;n libro asignado";

                                    return Ok(_rp);
                                }
                            }
                            else
                            {
                                _rp.Mensaje = "No se ha podido encontrar el nivel especificado, intente nuevamente";
                                return Ok(_rp);
                            }
                        }
                        else
                        {
                            _rp.Mensaje = "El usuario no tiene permisos de eliminación en ésta pantalla.";
                            return Ok(_rp);
                        }
                    }
                    else
                    {
                        _rp.Mensaje = "Usuario no autenticado, inicie sesi&pacute;n nuevamente.";
                        return Ok(_rp);
                    }
                }
                else
                {
                    _rp.Mensaje = "Usuario no autenticado, inicie sesi&pacute;n nuevamente.";
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
    }
}