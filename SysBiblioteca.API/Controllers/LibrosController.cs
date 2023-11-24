using SysBiblioteca.API.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using SysBiblioteca.API.Management;
using SysBiblioteca.API.Models.ADM;
using SysBiblioteca.API.Models.INV;
using SysBiblioteca.API.Services.INV.LibrosService;
using SysBiblioteca.API.Services.INV.AutoresService;
using SysBiblioteca.API.Services.ADM.UsuariosService;
using SysBiblioteca.API.Services.ADM.LinkRolMenuService;
using SysBiblioteca.API.Services.INV.EditorialesService;
using SysBiblioteca.API.Services.INV.AutoresLibrosService;
using SysBiblioteca.API.Services.INV.GenerosLibrosService;
using SysBiblioteca.API.Services.INV.GenerosLiterariosService;

namespace SysBiblioteca.API.Controllers
{
    [ApiController]
    [EnableCors("SysBiblioteca_Policy")]
    [Route("SysBiblioteca/API/[controller]")]
    public class LibrosController : Controller
    {
        private readonly iUsuariosService iUsuarios;
        private readonly iLibrosService iLibrosService;
        private readonly iAutoresService iAutoresService;
        private readonly iEditorialesService iEditorialesService;
        private readonly iLinkRolMenuService iLinkRolMenuService;
        private readonly iGenerosLibrosService iGenerosLibrosService;
        private readonly iAutoresLibrosService iAutoresLibrosService;
        private readonly iGenerosLiterariosService iGenerosLiterariosService;

        public LibrosController(iUsuariosService usuariosService, iLinkRolMenuService linkRolMenuService, iAutoresService autoresService, iGenerosLiterariosService generosLiterariosService, iLibrosService librosService,
            iAutoresLibrosService autoresLibrosService, iGenerosLibrosService generosLibrosService, iEditorialesService editorialesService)
        {
            iUsuarios = usuariosService;
            iLibrosService = librosService;
            iAutoresService = autoresService;
            iEditorialesService = editorialesService;
            iLinkRolMenuService = linkRolMenuService;
            iGenerosLibrosService = generosLibrosService;
            iAutoresLibrosService = autoresLibrosService;
            iGenerosLiterariosService = generosLiterariosService;
        }

        #region Mantenimiento de Catálogos

        #region Autores

        [HttpPost]
        [Route("GetAutores")]
        // SysBiblioteca/API/Libros/GetAutores
        // Método que obtiene el listado de Autores
        public IActionResult GetAutores([FromBody] Autores _autores)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_autores.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_autores.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _autores.ActualRute);
                        if (permisos != null && permisos.Read)
                        {
                            IEnumerable<Autores> autores = iAutoresService.Read();
                            if (autores.Count() > 0)
                            {
                                _rp.Datos = autores;
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
        [Route("GetAutor")]
        // SysBiblioteca/API/Libros/GetAutor
        // Método que obtiene los datos del Autor seleccionado
        public IActionResult GetAutor([FromBody] Autores _autores)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_autores.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_autores.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _autores.ActualRute);
                        if (permisos != null && permisos.Read)
                        {
                            Autores autor = iAutoresService.getById(_autores.IdAutor);
                            if (autor != null)
                            {
                                _rp.Datos = autor;
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
        [Route("CreateAutor")]
        // SysBiblioteca/API/Libros/CreateAutor
        // Método que ingresa un nuevo Autor al sistema
        public IActionResult CreateAutor([FromBody] Autores _autores)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_autores.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_autores.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _autores.ActualRute);
                        if (permisos != null && permisos.Create)
                        {
                            Autores autor = iAutoresService.getByName(_autores.Autor);
                            if (autor == null)
                            {
                                iAutoresService.Create(new Autores
                                {
                                    Autor = _autores.Autor
                                });

                                _rp.Resultado = 1;
                                _rp.Mensaje = "Autor modificado con &eacute;xito";
                                return Ok(_rp);
                            }
                            else
                            {
                                _rp.Mensaje = "Ya existe un Autor con ese nombre";
                                return Ok(_rp);
                            }
                        }
                        else
                        {
                            _rp.Mensaje = "El usuario no tiene permisos de escritura en esta pantalla.";
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
        [Route("UpdateAutor")]
        // SysBiblioteca/API/Libros/UpdateAutor
        // Método que modifica los datos del Autor seleccionado
        public IActionResult UpdateAutor([FromBody] Autores _autores)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_autores.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_autores.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _autores.ActualRute);
                        if (permisos != null && permisos.Update)
                        {
                            Autores autor = iAutoresService.getById(_autores.IdAutor);
                            if (autor != null)
                            {
                                autor.Autor = _autores.Autor;
                                iAutoresService.Update(autor);

                                _rp.Resultado = 1;
                                _rp.Mensaje = "Autor creado con &eacute;xito";
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
                            _rp.Mensaje = "El usuario no tiene permisos de edicion en esta pantalla.";
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
        [Route("DeleteAutor")]
        // SysBiblioteca/API/Libros/DeleteAutor
        // Método que elimina al Autor seleccionado
        public IActionResult DeleteAutor([FromBody] Autores _autores)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_autores.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_autores.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _autores.ActualRute);
                        if (permisos != null && permisos.Update)
                        {
                            Autores autor = iAutoresService.getById(_autores.IdAutor);
                            if (autor != null)
                            {
                                try
                                {
                                    iAutoresService.Delete(autor);

                                    _rp.Resultado = 1;
                                    _rp.Mensaje = "Autor eliminado correctamente.";
                                    return Ok(_rp);
                                }
                                catch (Exception ex)
                                {
                                    _rp.Mensaje = "No se ha podido eliminar al autor selccionado. Parece que existen libros asignados a &eacute;l";
                                    return Ok(_rp);
                                }
                            }
                            else
                            {
                                _rp.Mensaje = "Sin datos que mostrar.";
                                return Ok(_rp);
                            }
                        }
                        else
                        {
                            _rp.Mensaje = "El usuario no tiene permisos de edicion en esta pantalla.";
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

        #region Editoriales

        [HttpPost]
        [Route("GetEditoriales")]
        // SysBiblioteca/API/Libros/GetEditoriales
        // Método que obtiene el listado de Editoriales
        public IActionResult GetEditoriales([FromBody] Editoriales _editoriales)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_editoriales.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_editoriales.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _editoriales.ActualRute);
                        if (permisos != null && permisos.Read)
                        {
                            IEnumerable<Editoriales> editoriales = iEditorialesService.Read();
                            if (editoriales.Count() > 0)
                            {
                                _rp.Datos = editoriales;
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
        [Route("GetEditorial")]
        // SysBiblioteca/API/Libros/GetEditorial
        // Método que obtiene los datos de la editorial seleccionada
        public IActionResult GetEditorial([FromBody] Editoriales _editoriales)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_editoriales.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_editoriales.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _editoriales.ActualRute);
                        if (permisos != null && permisos.Read)
                        {
                            Editoriales editorial = iEditorialesService.getById(_editoriales.IdEditorial);
                            if (editorial != null)
                            {
                                _rp.Datos = editorial;
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
        [Route("CreateEditorial")]
        // SysBiblioteca/API/Libros/CreateEditorial
        // Método que agrega una nueva editorial al sistema
        public IActionResult CreateEditorial([FromBody] Editoriales _editoriales)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_editoriales.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_editoriales.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _editoriales.ActualRute);
                        if (permisos != null && permisos.Create)
                        {
                            Editoriales editorial = iEditorialesService.GetByName(_editoriales.Editorial);
                            if (editorial == null)
                            {
                                iEditorialesService.Create(new Editoriales
                                {
                                    Editorial = _editoriales.Editorial
                                });
                                _rp.Resultado = 1;
                                _rp.Mensaje = "Editorial creada con &eacute;xito";
                                return Ok(_rp);
                            }
                            else
                            {
                                _rp.Mensaje = "Ya esxiste una editorial con ese nombre";
                                return Ok(_rp);
                            }
                        }
                        else
                        {
                            _rp.Mensaje = "El usuario no tiene permisos de escritura en esta pantalla.";
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
        [Route("UpdateEditorial")]
        // SysBiblioteca/API/Libros/UpdateEditorial
        // Método que modifica los datos de la editorial seleccionada
        public IActionResult UpdateEditorial([FromBody] Editoriales _editoriales)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_editoriales.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_editoriales.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _editoriales.ActualRute);
                        if (permisos != null && permisos.Create)
                        {
                            Editoriales editorial = iEditorialesService.getById(_editoriales.IdEditorial);
                            if (editorial != null)
                            {
                                editorial.Editorial = _editoriales.Editorial;
                                iEditorialesService.Update(editorial);

                                _rp.Resultado = 1;
                                _rp.Mensaje = "Editorial creada con &eacute;xito";
                                return Ok(_rp);
                            }
                            else
                            {
                                _rp.Mensaje = "No se pudo encontrar la editorial seleccionada, intente nuevamente.";
                                return Ok(_rp);
                            }
                        }
                        else
                        {
                            _rp.Mensaje = "El usuario no tiene permisos de edici&iacute;n en esta pantalla.";
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
        [Route("DeleteEditorial")]
        // SysBiblioteca/API/Libros/DeleteEditorial
        // Método que elimina la editorial seleccionada del sistema
        public IActionResult DeleteEditorial([FromBody] Editoriales _editoriales)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_editoriales.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_editoriales.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _editoriales.ActualRute);
                        if (permisos != null && permisos.Delete)
                        {
                            Editoriales editorial = iEditorialesService.getById(_editoriales.IdEditorial);
                            if (editorial != null)
                            {
                                try
                                {
                                    iEditorialesService.Delete(editorial);

                                    _rp.Resultado = 1;
                                    _rp.Mensaje = "Editorial eliminada con &eacute;xito";
                                    return Ok(_rp);
                                }
                                catch (Exception ex)
                                {
                                    _rp.Mensaje = "No se pudo eliminar la Editorial, parece que está asignada a algunos libros.";
                                    return Ok(_rp);
                                }
                            }
                            else
                            {
                                _rp.Mensaje = "No se pudo encontrar la editorial seleccionada, intente nuevamente.";
                                return Ok(_rp);
                            }
                        }
                        else
                        {
                            _rp.Mensaje = "El usuario no tiene permisos de eliminaci&iacute;n en esta pantalla.";
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

        #region Generos

        [HttpPost]
        [Route("GetGeneros")]
        // SysBiblioteca/API/Libros/GetGeneros
        // Método que obtiene el listado de Generos Literarios
        public IActionResult GetGeneros([FromBody] GenerosLiterarios _generos)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_generos.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_generos.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _generos.ActualRute);
                        if (permisos != null && permisos.Read)
                        {
                            IEnumerable<GenerosLiterarios> generos = iGenerosLiterariosService.Read();
                            if (generos.Count() > 0)
                            {
                                _rp.Datos = generos;
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
        [Route("GetGenero")]
        // SysBiblioteca/API/Libros/GetGenero
        // Método que obtiene los datos del Genero Literario seleccionado
        public IActionResult GetGenero([FromBody] GenerosLiterarios _generos)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_generos.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_generos.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _generos.ActualRute);
                        if (permisos != null && permisos.Read)
                        {
                            GenerosLiterarios genero = iGenerosLiterariosService.getById(_generos.IdGenero);
                            if (genero != null)
                            {
                                _rp.Datos = genero;
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
        [Route("CreateGenero")]
        // SysBiblioteca/API/Libros/CreateGenero
        // Método que agrega un nuevo Genero Literario al sistema
        public IActionResult CreateGenero([FromBody] GenerosLiterarios _generos)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_generos.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_generos.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _generos.ActualRute);
                        if (permisos != null && permisos.Create)
                        {
                            GenerosLiterarios genero = iGenerosLiterariosService.GetByname(_generos.Genero);
                            if (genero == null)
                            {
                                iGenerosLiterariosService.Create(new GenerosLiterarios
                                {
                                    Genero = _generos.Genero
                                });

                                _rp.Mensaje = "G&eacute;nero literario agregado correctamente";
                                _rp.Resultado = 1;
                                return Ok(_rp);
                            }
                            else
                            {
                                _rp.Mensaje = "Ya existe un este G&eacute;nero literario en el sistema";
                                return Ok(_rp);
                            }
                        }
                        else
                        {
                            _rp.Mensaje = "El usuario no tiene permisos de escritura en esta pantalla.";
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
        [Route("UpdateGenero")]
        // SysBiblioteca/API/Libros/UpdateGenero
        // Método que modifica los datos del Genero Literario seleccionado
        public IActionResult UpdateGenero([FromBody] GenerosLiterarios _generos)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_generos.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_generos.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _generos.ActualRute);
                        if (permisos != null && permisos.Update)
                        {
                            GenerosLiterarios genero = iGenerosLiterariosService.getById(_generos.IdGenero);
                            if (genero != null)
                            {
                                genero.Genero = _generos.Genero;
                                iGenerosLiterariosService.Update(genero);

                                _rp.Mensaje = "G&eacute;nero literario modificado correctamente";
                                _rp.Resultado = 1;
                                return Ok(_rp);
                            }
                            else
                            {
                                _rp.Mensaje = "No se pudo modificar el G&eacute;nero literario en el sistema, intente nuevamente";
                                return Ok(_rp);
                            }
                        }
                        else
                        {
                            _rp.Mensaje = "El usuario no tiene permisos de edicion en esta pantalla.";
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
        [Route("DeleteGenero")]
        // SysBiblioteca/API/Libros/DeleteGenero
        // Método que elimina el Genero Literario seleccionado del sistema
        public IActionResult DeleteGenero([FromBody] GenerosLiterarios _generos)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_generos.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_generos.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _generos.ActualRute);
                        if (permisos != null && permisos.Delete)
                        {
                            GenerosLiterarios genero = iGenerosLiterariosService.getById(_generos.IdGenero);
                            if (genero != null)
                            {
                                try
                                {
                                    iGenerosLiterariosService.Delete(genero);

                                    _rp.Mensaje = "G&eacute;nero literario eliminado correctamente";
                                    _rp.Resultado = 1;
                                    return Ok(_rp);
                                }
                                catch (Exception)
                                {
                                    _rp.Mensaje = "No se pudo eliminar el G&eacute;nero literario seleccionado, parece que ya fue agregado a algun libro";
                                    return Ok(_rp);
                                }
                            }
                            else
                            {
                                _rp.Mensaje = "No se pudo modificar el G&eacute;nero literario en el sistema, intente nuevamente";
                                return Ok(_rp);
                            }
                        }
                        else
                        {
                            _rp.Mensaje = "El usuario no tiene permisos de eliminacion en esta pantalla.";
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

        #endregion

        #region Mantenimiento de Libros

        [HttpPost]
        [Route("CreateLibro")]
        // SysBiblioteca/API/Libros/CreateLibro
        // Método que agrega un nuevo Libro al sistema
        public IActionResult CreateLibro([FromBody] LibrosDTO_ADD _libro)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_libro.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_libro.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _libro.ActualRute);
                        if (permisos != null && permisos.Create)
                        {
                            Libros newLibro = new Libros {
                                FotoLibro = _libro.FotoLibro,
                                Libro = _libro.Libro,
                                Version = (_libro.Version != null ? _libro.Version : "N/A"),
                                ISBN = _libro.ISBN,
                                AnioPublicacion = _libro.AnioPublicacion,
                                Descripcion = _libro.Descripcion,
                                Cantidad = _libro.Cantidad,

                                IdEditorial = _libro.IdEditorial,
                                IdEstado = 1,

                                UsuarioCreacion = user.Usuario,
                                FechaCreacion = DateTime.Now
                            };

                            iLibrosService.Create(newLibro);

                            if (newLibro.IdLibro > 0)
                            {
                                foreach (var generoLiterario in _libro.GenerosLiterarios)
                                {
                                    GenerosLibros newGenero = new GenerosLibros {
                                        IdLibro = newLibro.IdLibro,
                                        IdGenero = generoLiterario.IdGenero
                                    };

                                    iGenerosLibrosService.Create(newGenero);
                                }

                                foreach (var autor in _libro.Autores)
                                {
                                    AutoresLibros newAutor = new AutoresLibros
                                    {
                                        IdLibro = newLibro.IdLibro,
                                        IdAutor = autor.IdAutor
                                    };

                                    iAutoresLibrosService.Create(newAutor);
                                }

                                _rp.Resultado = 1;
                                _rp.Mensaje = "Libro ingresado correctamente.";
                                return Ok(_rp);
                            }
                            else
                            {
                                _rp.Mensaje = "No se pudo crear el libro, intente nuevamente.";
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
        [Route("SearchLibros")]
        // SysBiblioteca/API/Libros/SearchLibros
        // Método que busca un libro que encaje en la descripción específicada
        public IActionResult SearchLibros([FromBody] searchQuery searchQuery)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (searchQuery.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(searchQuery.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, searchQuery.ActualRute);
                        if (permisos != null && permisos.Read)
                        {
                            IEnumerable<Libros> libros = iLibrosService.search(searchQuery.Titulo, searchQuery.IdAutor, searchQuery.IdGenero, searchQuery.Prestamo);
                            if (libros.Count() > 0)
                            {
                                List<LibrosDTO> resultsLibros = new List<LibrosDTO>();

                                foreach (var libro in libros)
                                {
                                    resultsLibros.Add(new LibrosDTO
                                    {
                                        IdLibro = libro.IdLibro,
                                        FotoLibro = libro.FotoLibro,
                                        Libro = libro.Libro,
                                        Autores = getAutoresName(iAutoresLibrosService.getAutoresLibro(libro.IdLibro)),
                                        Generos = getGenerosName(iGenerosLibrosService.getGenerosLibro(libro.IdLibro)),
                                        AnioPublicacion = libro.AnioPublicacion,
                                        Cantidad = libro.Cantidad
                                    });
                                }
                                _rp.Datos = resultsLibros;
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
        [Route("SearchLibrosInactivos")]
        // SysBiblioteca/API/Libros/SearchLibrosInactivos
        // Método que busca un libro inactivo que encaje en la descripción específicada
        public IActionResult SearchLibrosInactivos([FromBody] searchQuery searchQuery)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (searchQuery.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(searchQuery.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, searchQuery.ActualRute);
                        if (permisos != null && permisos.Read)
                        {
                            IEnumerable<Libros> libros = iLibrosService.searchInactivos(searchQuery.Titulo, searchQuery.IdAutor, searchQuery.IdGenero);
                            if (libros.Count() > 0)
                            {
                                List<LibrosDTO> resultsLibros = new List<LibrosDTO>();

                                foreach (var libro in libros)
                                {
                                    resultsLibros.Add(new LibrosDTO
                                    {
                                        IdLibro = libro.IdLibro,
                                        FotoLibro = libro.FotoLibro,
                                        Libro = libro.Libro,
                                        Autores = getAutoresName(iAutoresLibrosService.getAutoresLibro(libro.IdLibro)),
                                        Generos = getGenerosName(iGenerosLibrosService.getGenerosLibro(libro.IdLibro)),
                                        AnioPublicacion = libro.AnioPublicacion,
                                        Cantidad = libro.Cantidad
                                    });
                                }
                                _rp.Datos = resultsLibros;
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
        [Route("GetLibro")]
        // SysBiblioteca/API/Libros/GetLibro
        // Método que obtiene el libro seleccionado
        public IActionResult GetLibro([FromBody] Libros _libros)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_libros.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_libros.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _libros.ActualRute);
                        if (permisos != null && permisos.Read)
                        {
                            Libros libro = iLibrosService.getById(_libros.IdLibro);
                            if (libro != null)
                            {
                                _rp.Datos = libro;
                                _rp.Resultado = 1;
                                return Ok(_rp);
                            }
                            else
                            {
                                _rp.Mensaje = "No se pudo cargar el libro seleccionado, intente nuevamente.";
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
        [Route("GetAutoresLibro")]
        // SysBiblioteca/API/Libros/GetAutoresLibro
        // Método que obtiene los autores del libro seleccionado
        public IActionResult GetAutoresLibro([FromBody] Libros _libros)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_libros.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_libros.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _libros.ActualRute);
                        if (permisos != null && permisos.Read)
                        {
                            List<Autores> autores = iAutoresLibrosService.getAutoresLibro(_libros.IdLibro);
                            if (autores.Count > 0)
                            {
                                _rp.Datos = autores;
                                _rp.Resultado = 1;
                                return Ok(_rp);
                            }
                            else
                            {
                                _rp.Mensaje = "No se pudo obtener los autores del libro.";
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
        [Route("GetGenerosLibro")]
        // SysBiblioteca/API/Libros/GetGenerosLibro
        // Método que obtiene los géneros literarios del libro seleccionado
        public IActionResult GetGenerosLibro([FromBody] Libros _libros)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_libros.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_libros.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _libros.ActualRute);
                        if (permisos != null && permisos.Read)
                        {
                            List<GenerosLiterarios> generos = iGenerosLibrosService.getGenerosLibro(_libros.IdLibro);
                            if (generos.Count > 0)
                            {
                                _rp.Datos = generos;
                                _rp.Resultado = 1;
                                return Ok(_rp);
                            }
                            else
                            {
                                _rp.Mensaje = "No se pudo obtener los géneros literarios del libro.";
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
        [Route("UpdateLibro")]
        // SysBiblioteca/API/Libros/UpdateLibro
        // Método que modifica Libro del sistema
        public IActionResult UpdateLibro([FromBody] LibrosDTO_ADD _libro)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_libro.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_libro.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _libro.ActualRute);
                        if (permisos != null && permisos.Update)
                        {
                            Libros oldBook = iLibrosService.getById(_libro.IdLibro);
                            if (oldBook != null) 
                            {
                                iLibrosService.Update(oldBook, new Libros
                                {
                                    FotoLibro = _libro.FotoLibro,
                                    Libro = _libro.Libro,
                                    Version = _libro.Version,
                                    ISBN = _libro.ISBN,
                                    AnioPublicacion = _libro.AnioPublicacion,
                                    Descripcion = _libro.Descripcion,
                                    Cantidad = _libro.Cantidad,
                                    IdEditorial = _libro.IdEditorial,
                                    UsuarioModificacion = user.Usuario,
                                    FechaModificacion = DateTime.Now
                                });

                                _rp.Resultado = 1;
                                _rp.Mensaje = "Datos del libro actualizados.";
                                return Ok(_rp);
                            }
                            else
                            {
                                _rp.Mensaje = "No se pudo encontrar el libro seleccionado, intente nuevamente.";
                                return Ok(_rp);
                            }
                        }
                        else
                        {
                            _rp.Mensaje = "El usuario no tiene permisos de edici&oacute;n en ésta pantalla.";
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
        [Route("AddAutor")]
        // SysBiblioteca/API/Libros/AddAutor
        // Método que agrega un autor al libro
        public IActionResult AddAutor([FromBody] AutoresLibros _libro)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_libro.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_libro.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _libro.ActualRute);
                        if (permisos != null && permisos.Create)
                        {
                            iAutoresLibrosService.Create(new AutoresLibros
                            {
                                IdLibro = _libro.IdLibro,
                                IdAutor = _libro.IdAutor
                            });

                            _rp.Resultado = 1;
                            _rp.Mensaje = "Se agreg&oacute; correctamente el autor al libro";
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
        [Route("RemoveAutor")]
        // SysBiblioteca/API/Libros/RemoveAutor
        // Método que elimina un autor del libro
        public IActionResult RemoveAutor([FromBody] AutoresLibros _libro)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_libro.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_libro.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _libro.ActualRute);
                        if (permisos != null && permisos.Delete)
                        {
                            AutoresLibros autorLibro = iAutoresLibrosService.getByAutorLibro(_libro.IdLibro, _libro.IdAutor);
                            if (autorLibro != null)
                            {
                                iAutoresLibrosService.Delete(autorLibro);

                                _rp.Resultado = 1;
                                _rp.Mensaje = "Se elimin&oacute; correctamente el autor del libro";
                                return Ok(_rp);
                            }
                            else
                            {
                                _rp.Mensaje = "No se pudo encontrar el autor seleccionado, intente nuevamente.";
                                return Ok(_rp);
                            }
                        }
                        else
                        {
                            _rp.Mensaje = "El usuario no tiene permisos de eliminaci&oacute;n en esta pantalla.";
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
        [Route("AddGender")]
        // SysBiblioteca/API/Libros/AddGender
        // Método que agrega un género al libro
        public IActionResult AddGender([FromBody] GenerosLibros _libro)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_libro.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_libro.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _libro.ActualRute);
                        if (permisos != null && permisos.Create)
                        {
                            iGenerosLibrosService.Create(new GenerosLibros
                            {
                                IdLibro = _libro.IdLibro,
                                IdGenero = _libro.IdGenero
                            });

                            _rp.Resultado = 1;
                            _rp.Mensaje = "Se agreg&oacute; correctamente el g&eacute;nero al libro";
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
        [Route("RemoveGender")]
        // SysBiblioteca/API/Libros/RemoveGender
        // Método que elimina un autor del libro
        public IActionResult RemoveGender([FromBody] GenerosLibros _libro)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_libro.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_libro.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _libro.ActualRute);
                        if (permisos != null && permisos.Delete)
                        {
                            GenerosLibros generoLibro = iGenerosLibrosService.getByGenderLibro(_libro.IdLibro, _libro.IdGenero);
                            if (generoLibro != null)
                            {
                                iGenerosLibrosService.Delete(generoLibro);

                                _rp.Resultado = 1;
                                _rp.Mensaje = "Se elimin&oacute; correctamente el g&eacute;nero del libro";
                                return Ok(_rp);
                            }
                            else
                            {
                                _rp.Mensaje = "No se pudo encontrar el autor seleccionado, intente nuevamente.";
                                return Ok(_rp);
                            }
                        }
                        else
                        {
                            _rp.Mensaje = "El usuario no tiene permisos de eliminaci&oacute;n en esta pantalla.";
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
        [Route("ActivateBook")]
        // SysBiblioteca/API/Libros/ActivateBook
        // Método que activa un libro en el sistema
        public IActionResult ActivateBook([FromBody] Libros _libro)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_libro.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_libro.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _libro.ActualRute);
                        if (permisos != null && permisos.Update)
                        {
                            Libros libro = iLibrosService.getById(_libro.IdLibro);
                            if (libro != null)
                            {
                                iLibrosService.Activate(_libro.IdLibro);

                                _rp.Resultado = 1;
                                _rp.Mensaje = "Se activ&oacute; correctamente el libro";
                                return Ok(_rp);
                            }
                            else
                            {
                                _rp.Mensaje = "No se pudo encontrar el libro seleccionado, intente nuevamente.";
                                return Ok(_rp);
                            }
                        }
                        else
                        {
                            _rp.Mensaje = "El usuario no tiene permisos de edici&oacute;n en esta pantalla.";
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
        [Route("DeactivateBook")]
        // SysBiblioteca/API/Libros/DeactivateBook
        // Método que desactiva un libro en el sistema
        public IActionResult DeactivateBook([FromBody] Libros _libro)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_libro.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_libro.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _libro.ActualRute);
                        if (permisos != null && permisos.Update)
                        {
                            Libros libro = iLibrosService.getById(_libro.IdLibro);
                            if (libro != null)
                            {
                                iLibrosService.deactivate(_libro.IdLibro);

                                _rp.Resultado = 1;
                                _rp.Mensaje = "Se desactiv&oacute; correctamente el libro";
                                return Ok(_rp);
                            }
                            else
                            {
                                _rp.Mensaje = "No se pudo encontrar el libro seleccionado, intente nuevamente.";
                                return Ok(_rp);
                            }
                        }
                        else
                        {
                            _rp.Mensaje = "El usuario no tiene permisos de edici&oacute;n en esta pantalla.";
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

        private string getGenerosName(List<GenerosLiterarios> generosLiterarios)
        {
            String generos = String.Empty;
            foreach (var genero in generosLiterarios)
            {
                generos += genero.Genero + ", ";
            }
            return generos;
        }

        private string getAutoresName(List<Autores> autoresLibros)
        {
            String autores = String.Empty;
            foreach (var autor in autoresLibros)
            {
                autores += autor.Autor + ", ";
            }
            return autores;
        }

        #endregion

    }
}