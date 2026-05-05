//Carga las plataformas para poder designar los permisos
function loadPlataformas() {
    var pkg = {
        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', ''),
        AppToken: localStorage.getItem("AppToken")
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Aplicaciones/GetActivas',
        contentType: "Application/json",
        data: JSON.stringify(pkg),
        success: function (data) {
            if (data.resultado == 1) {
                $("#IdPlataforma").html(null);
                var html = "";
                html += "<option selected>Elija una Aplicación</option>";
                $.each(data.datos, function () {
                    html += "<option value='" + this.idAplicacion + "'>" + this.nombreAplicacion + "</option>";
                });
                $("#IdPlataforma").html(html);
                $('#formPermisos').hide();
            }
        }
    });
}

//Genera una vista previa de como se vería la barra lateral con los menus Raiz
function renderRootMenu() {
    var Obj = {
        Menu: {
            IdAplicacion: parseInt($("#IdPlataforma").val())
        },
        IdRol: parseInt($("#IdRolPermiso").val()),

        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', ''),
        AppToken: localStorage.getItem("AppToken")
    };
    var api = localStorage.getItem('apiURL');

    var html = "";
    $("#rootMenu").html(null);

    $.ajax({
        type: 'POST',
        url: api + 'Permisos/GetRoots',
        contentType: "Application/json",
        data: JSON.stringify(Obj),
        success: function (data) {
            if (data.resultado == 1) {
                $.each(data.datos, function () {
                    html += '<a class="list-group-item list-group-item-action clearfix align-items-center font-weight-bold">' + this.menu.nombre;
                    if (this.hasSons) {
                        html += '   <i class="btn btn-primary material-icons float-right" onclick="renderMenus(' + this.menu.idMenu + ');" style="font-size: small;" title="Listar">dehaze</i>';
                    }
                    else {
                        html += '   <i class="btn btn-danger material-icons float-right" onclick="removeMenu(' + this.menu.idMenu + ');" style="font-size: small;" title="Eliminar">delete</i>';
                        html += '   <i class="btn btn-info material-icons float-right" onclick="getMenu(' + this.menu.idMenu + ');" style="font-size: small;" title="Editar">edit</i>';
                    }
                    html += '</a>';
                    $("#rootMenu").html(html);

                    $("#navegacion").html(null);
                    var html2 = "";
                    html2 += '<li class="breadcrumb-item active" aria-current="page">Menú Raíz</li>';
                    $("#navegacion").html(html2);
                });
            }
            else {
                Swal.fire({
                    title: 'Mensaje',
                    icon: "info",
                    html: data.mensaje,
                    timer: 2000,
                    timerProgressBar: true,
                    didOpen: () => {
                        Swal.showLoading();
                    },
                });
            }
        },
        error: function () {
            Swal.fire({
                title: 'Error',
                icon: "warning",
                html: "Algo salió mal y el menú asignado no pudo ser cargado, actualiza para volver a intentarlo.",
                timer: 2000,
                timerProgressBar: true,
                didOpen: () => {
                    Swal.showLoading();
                },
            });
        }
    });
}

//Genera una vista previa de como se vería la barra lateral con los menus Padre
function renderMenus(id) {
    if (id > 0) {
        localStorage.setItem("IdParent", id);
    }

    var Obj = {
        Menu: {
            IdAplicacion: parseInt($("#IdPlataforma").val())
        },
        IdRol: parseInt($("#IdRolPermiso").val()),
        IdMenu: parseInt(localStorage.getItem("IdParent")),

        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', ''),
        AppToken: localStorage.getItem("AppToken")
    };
    var api = localStorage.getItem('apiURL');

    var html = "";
    $("#rootMenu").html(null);

    $.ajax({
        type: 'POST',
        url: api + 'Permisos/GetMenus',
        contentType: "Application/json",
        data: JSON.stringify(Obj),
        success: function (data) {
            if (data.resultado == 1) {
                $.each(data.datos, function () {
                    html += '<a class="list-group-item list-group-item-action clearfix align-items-center font-weight-bold">' + this.menu.nombre;
                    if (this.hasSons) {
                        html += '   <i class="btn btn-primary material-icons float-right" onclick="renderNietos(' + this.menu.idMenu + ');" style="font-size: small;" title="Listar">dehaze</i>';
                    }
                    else {
                        html += '   <i class="btn btn-danger material-icons float-right" onclick="removeMenu(' + this.menu.idMenu + ');" style="font-size: small;" title="Eliminar">delete</i>';
                        html += '   <i class="btn btn-info material-icons float-right" onclick="getMenu(' + this.menu.idMenu + ');" style="font-size: small;" title="Editar">edit</i>';
                    }
                    html += '</a>';
                    $("#rootMenu").html(html);

                    $("#navegacion").html(null);
                    var html2 = "";
                    html2 += '<li class="breadcrumb-item"><a style="color:#3699ff;" onclick="renderRootMenu();">Menú Raíz</a></li>';
                    html2 += '<li class="breadcrumb-item active" aria-current="page">Menú</li>';
                    $("#navegacion").html(html2);
                });
            }
            else {
                Swal.fire({
                    title: 'Mensaje',
                    icon: "info",
                    html: data.mensaje,
                    timer: 2000,
                    timerProgressBar: true,
                    didOpen: () => {
                        Swal.showLoading();
                    },
                });
            }
        },
        error: function () {
            Swal.fire({
                title: 'Error',
                icon: "warning",
                html: "Algo salió mal y el menú asignado no pudo ser cargado, actualiza para volver a intentarlo.",
                timer: 2000,
                timerProgressBar: true,
                didOpen: () => {
                    Swal.showLoading();
                },
            });
        }
    });
}

//Genera una vista previa de como se vería la barra lateral con los menus Hijos
function renderNietos(id) {
    if (id > 0) {
        localStorage.setItem("IdSubParent", id);
    }

    var Obj = {
        Menu: {
            IdPlataforma: parseInt($("#IdPlataforma").val())
        },
        IdRol: parseInt($("#IdRolPermiso").val()),
        IdMenu: parseInt(localStorage.getItem("IdSubParent")),

        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', ''),
        AppToken: localStorage.getItem("AppToken")
    };
    var api = localStorage.getItem('apiURL');

    var html = "";
    $("#rootMenu").html(null);

    $.ajax({
        type: 'POST',
        url: api + 'Permisos/GetSons',
        contentType: "Application/json",
        data: JSON.stringify(Obj),
        success: function (data) {
            if (data.resultado == 1) {
                $.each(data.datos, function () {
                    html += '<a class="list-group-item list-group-item-action clearfix align-items-center font-weight-bold">' + this.menu.nombre;
                    if (this.hasSons) {
                        html += '   <i class="btn btn-primary material-icons float-right" onclick="showNietos(' + this.menu.idMenu + ');" style="font-size: small;" title="Listar">dehaze</i>';
                    }
                    else {
                        html += '   <i class="btn btn-danger material-icons float-right" onclick="removeMenu(' + this.menu.idMenu + ');" style="font-size: small;" title="Eliminar">delete</i>';
                        html += '   <i class="btn btn-info material-icons float-right" onclick="getMenu(' + this.menu.idMenu + ');" style="font-size: small;" title="Editar">edit</i>';
                    }
                    html += '</a>';
                    $("#rootMenu").html(html);

                    $("#navegacion").html(null);
                    var html2 = "";
                    html2 += '<li class="breadcrumb-item"><a style="color:#3699ff;" onclick="renderRootMenu();">Menú Raíz</a></li>';
                    html2 += '<li class="breadcrumb-item"><a style="color:#3699ff;" onclick="renderMenus();">Menús</a></li>';
                    html2 += '<li class="breadcrumb-item active" aria-current="page">Menú hijos</li>';
                    $("#navegacion").html(html2);
                });
            }
            else {
                Swal.fire({
                    title: 'Mensaje',
                    icon: "info",
                    html: data.mensaje,
                    timer: 2000,
                    timerProgressBar: true,
                    didOpen: () => {
                        Swal.showLoading();
                    },
                });
            }
        },
        error: function () {
            Swal.fire({
                title: 'Error',
                icon: "warning",
                html: "Algo salió mal y el menú asignado no pudo ser cargado, actualiza para volver a intentarlo.",
                timer: 2000,
                timerProgressBar: true,
                didOpen: () => {
                    Swal.showLoading();
                },
            });
        }
    });
}

//Obtiene la info del menu para poder mostralo en el formulario y poder editarlo
function getMenu(id) {
    if (id > 0) {
        var Obj = {
            IdMenu: id,
            IdRol: parseInt($("#IdRolPermiso").val()),

            Token: localStorage.getItem("UserToken"),
            ActualRute: window.location.hash.replace('#', ''),
            AppToken: localStorage.getItem("AppToken")
        };
        var api = localStorage.getItem('apiURL');

        $.ajax({
            type: 'POST',
            url: api + 'Permisos/GetSelectedMenu',
            contentType: "Application/json",
            data: JSON.stringify(Obj),
            success: function (data) {
                if (data.resultado = 1) {
                    resetFormulario();
                    $("#dataMenu").show(); $("#btnEditAssign").show(); $("#btnSaveAssign").hide(); $("#btnCancelAssign").show();
                    $("#Padres").hide(); $("#Hijos").hide(); $("#Nietos").hide();
                    $("#IdLinkRolMenuPais").val(data.datos.idLinkRolMenuPais);
                    $("#nameMenu").val(data.datos.menu.nombre);

                    $('#Create').prop('checked', data.datos.create).change();
                    $('#Read').prop('checked', data.datos.read).change();
                    $('#Update').prop('checked', data.datos.update).change();
                    $('#Delete').prop('checked', data.datos.delete).change();
                    localStorage.setItem("IdMenuSeleccionado", id);
                }
                else {
                    Swal.fire({
                        title: 'Error',
                        icon: "warning",
                        html: data.mensaje,
                        timer: 1000,
                        timerProgressBar: true,
                        didOpen: () => {
                            Swal.showLoading();
                        },
                    });
                }
            }
        });
    }
}



//Llenado de ComboBox Menús Raíz
function ShowRoots() {
    var Obj = {
        IdAplicacion: parseInt($("#IdPlataforma").val()),

        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', ''),
        AppToken: localStorage.getItem("AppToken")
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Menus/GetRoots',
        contentType: "Application/json",
        data: JSON.stringify(Obj),
        success: function (data) {
            $("#MenuRaiz").html(null);
            $('#Menu').html(null);
            $('#MenuHijo').html(null);
            $("#frmAsiganciones").removeClass('was-validated');

            if (data.resultado == 1) {
                var html = "";
                html += "<option value>Seleccione un Menú Raíz</option>";
                $.each(data.datos, function () {
                    html += "<option value='" + this.idMenu + "'>" + this.nombre + "</option>";
                });
                $("#MenuRaiz").html(html);
            }
            else {
                var html = "";
                html += "<option value='0'>" + data.mensaje + "</option>";
                $("#MenuRaiz").html(html);
                $("#Menu").html(html);
                $("#MenuHijo").html(html);
            }
        }
    });
}

//Llenado de ComboBox Menús Padres
function GetMenus() {
    if ($("#MenuRaiz").val() > 0) {
        $("#btnCancel").show();
        var Obj = {
            IdParent: parseInt($("#MenuRaiz").val()),

            Token: localStorage.getItem("UserToken"),
            ActualRute: window.location.hash.replace('#', ''),
            AppToken: localStorage.getItem("AppToken")
        };
        var api = localStorage.getItem('apiURL');

        $.ajax({
            type: 'POST',
            url: api + 'Menus/GetMenus',
            contentType: "Application/json",
            data: JSON.stringify(Obj),
            success: function (data) {
                $("#Menu").html(null);
                $("#MenuHijo").html(null);
                $("#frmAsiganciones").removeClass('was-validated');

                if (data.resultado == 1) {
                    var html = "";
                    html += "<option value>Elija un Menú</option>";
                    $.each(data.datos, function () {
                        html += "<option value='" + this.idMenu + "'>" + this.nombre + "</option>";
                    });
                    $("#Menu").html(html);
                }
                else {
                    var html = "";
                    html += "<option value='0'>" + data.mensaje + "</option>";
                    $("#Menu").html(html);
                    $("#MenuHijo").html(html);
                }
            }
        });
    }
    else {
        $("#Menu").html(null);
        var html = "";
        html += "<option value>Sin datos qué mostrar</option>";
        $("#Menu").html(html);
        $("#MenuHijo").html(html);
        $("#btnCancel").hide();
    }
}

//Llenado de ComboBox Menús Hijos
function GetSons() {
    if ($("#Menu").val() > 0) {
        var Obj = {
            IdParent: parseInt($("#MenuRaiz").val()),
            IdSubParent: parseInt($("#Menu").val()),

            Token: localStorage.getItem("UserToken"),
            ActualRute: window.location.hash.replace('#', ''),
            AppToken: localStorage.getItem("AppToken")
        };
        var api = localStorage.getItem('apiURL');

        $.ajax({
            type: 'POST',
            url: api + 'Menus/GetSons',
            contentType: "Application/json",
            data: JSON.stringify(Obj),
            success: function (data) {
                $("#MenuHijo").html(null);
                $("#frmAsiganciones").removeClass('was-validated');

                if (data.resultado == 1) {
                    var html = "";
                    html += "<option value>Elija un Menú</option>";
                    $.each(data.datos, function () {
                        html += "<option value='" + this.idMenu + "'>" + this.nombre + "</option>";
                    });
                    $("#MenuHijo").html(html);
                }
                else {
                    var html = "";
                    html += "<option value='0'>" + data.mensaje + "</option>";
                    $("#MenuHijo").html(html);
                }
            }
        });
    }
    else {
        $("#MenuHijo").html(null);
        var html = "";
        html += "<option value>Sin datos qué mostrar</option>";
        $("#MenuHijo").html(html).attr("disabled", true);
    }
}