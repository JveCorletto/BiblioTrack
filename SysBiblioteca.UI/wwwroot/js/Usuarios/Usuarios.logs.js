//Carga los empleados activos
function loadActivos() {
    var pkg = {
        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', '')
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Seguridad/GetUsuarios',
        contentType: "Application/json",
        data: JSON.stringify(pkg),
        success: function (data) {
            if (data.resultado == 1) {
                if ($.fn.dataTable.isDataTable('#tableActivos')) {
                    $('#tableActivos').DataTable().clear().destroy();
                }
                $("#tActivos").html("");
                $.each(data.datos, function () {
                    var html = "";
                    html += "<tr>";
                    html += "   <td>" + this.datosPersonales.dui + "</td>";
                    html += "   <td>" + this.datosPersonales.nombres + "</td>";
                    html += "   <td>" + this.datosPersonales.apellidos + "</td>";
                    html += "   <td>" + this.rol.rol + "</td>";
                    html += "   <td>" + this.datosPersonales.genero.genero + "</td>";
                    html += "   <td>" + this.datosPersonales.correo + "</td>";
                    html += "   <td>" + this.datosPersonales.telefono + "</td>";
                    html += "   <td width='175'>";
                    html += "       <center>";
                    html += "           <button type='button' title='Edici�n de Usuario' class='btn btn-primary' onclick='getUsuario(" + this.idUsuario + ", true)' data-toggle='modal' data-target='#staticUsuario'><i class='fas fa-edit'></i></button>";
                    html += "           <button type='button' title='Desactivar' class='btn btn-danger' onclick='deactivateUsuario(" + this.idUsuario + ")'><i class='fas fa-times'></i></button>";
                    html += "       </center>";
                    html += "   </td>";
                    html += "</tr>";
                    $("#tActivos").append(html);
                });
                paginate('tableActivos');
            }
            else {
                if ($.fn.dataTable.isDataTable('#tableActivos')) {
                    $('#tableActivos').DataTable().clear().destroy();
                }
                $("#tActivos").html(null);
                var html = "";
                html += "<tr>";
                html += "   <td colspan='9'><center class='text-danger font-weight-bolder'>" + data.mensaje + "</center></td>";
                html += "</tr>";
                $("#tActivos").append(html);
            }
        }
    });
}

//Carga los empleados inactivos
function loadInactivos() {
    var pkg = {
        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', '')
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Seguridad/GetUsuariosInactivos',
        contentType: "Application/json",
        data: JSON.stringify(pkg),
        success: function (data) {
            if (data.resultado == 1) {
                if ($.fn.dataTable.isDataTable('#tableInactivos')) {
                    $('#tableInactivos').DataTable().clear().destroy();
                }
                $("#tInactivos").html("");
                $.each(data.datos, function () {
                    var html = "";
                    html += "<tr>";
                    html += "   <td>" + this.datosPersonales.dui + "</td>";
                    html += "   <td>" + this.datosPersonales.nombres + "</td>";
                    html += "   <td>" + this.datosPersonales.apellidos + "</td>";
                    html += "   <td>" + this.rol.rol + "</td>";
                    html += "   <td>" + this.datosPersonales.genero.genero + "</td>";
                    html += "   <td>" + this.datosPersonales.correo + "</td>";
                    html += "   <td>" + this.datosPersonales.telefono + "</td>";
                    html += "   <td width='175'>";
                    html += "       <center>";
                    html += "           <button type='button' title='Activar' class='btn btn-success' onclick='activateUsuario(" + this.idUsuario + ")'><i class='fas fa-check'></i></button>";
                    html += "       </center>";
                    html += "   </td>";
                    html += "</tr>";
                    $("#tInactivos").append(html);
                });
                paginate('tableInactivos');
            }
            else {
                if ($.fn.dataTable.isDataTable('#Inactivos')) {
                    $('#Inactivos').DataTable().clear().destroy();
                }
                $("#tInactivos").html(null);
                var html = "";
                html += "<tr>";
                html += "   <td colspan='9'><center class='text-danger font-weight-bolder'>" + data.mensaje + "</center></td>";
                html += "</tr>";
                $("#tInactivos").append(html);
            }
        }
    });
}

//Carga los Generos
function loadGeneros(IdGenero, action) {
    var Obj = {
        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', '')
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Catalogos/GetGeneros',
        contentType: "Application/json",
        data: JSON.stringify(Obj),
        success: function (data) {
            $("#Genero").html(null);

            if (data.resultado == 1) {
                if (action) {
                    $("#Genero").html(null);
                    var html = "";
                    html += "<option value>Elija un G�nero</option>";
                    $.each(data.datos, function () {
                        html += "<option value='" + this.idGenero + "'>" + this.genero + "</option>";
                    });
                    $("#Genero").html(html);

                    if (IdGenero > 0) {
                        $("#Genero").val('' + IdGenero + '').change();
                    }
                    else {
                        $("#Genero").val('0').change();
                    }
                }
                else {
                    var html = "";
                    html += "<option value>Elija un G�nero</option>";
                    $.each(data.datos, function () {
                        html += "<option value='" + this.idGenero + "'>" + this.genero + "</option>";
                    });
                    $("#Genero").html(html);
                }
            }
            else {
                var html = "";
                html += "<option value='0'>" + data.mensaje + "</option>";
                $("#Genero").html(html);
            }
        }
    });
}

//Carga los Roles
function loadRoles(IdRol, action) {
    var Obj = {
        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', '')
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Catalogos/GetRoles',
        contentType: "Application/json",
        data: JSON.stringify(Obj),
        success: function (data) {
            $("#Rol").html(null);

            if (data.resultado == 1) {
                if (action) {
                    $("#Rol").html(null);
                    var html = "";
                    html += "<option value>Elija un Rol</option>";
                    $.each(data.datos, function () {
                        html += "<option value='" + this.idRol + "'>" + this.rol + "</option>";
                    });
                    $("#Rol").html(html);

                    if (IdRol > 0) {
                        $("#Rol").val('' + IdRol + '').change();
                    }
                    else {
                        $("#Rol").val('0').change();
                    }
                }
                else {
                    var html = "";
                    html += "<option value>Elija un Rol</option>";
                    $.each(data.datos, function () {
                        html += "<option value='" + this.idRol + "'>" + this.rol + "</option>";
                    });
                    $("#Rol").html(html);
                }
            }
            else {
                var html = "";
                html += "<option value='0'>" + data.mensaje + "</option>";
                $("#Rol").html(html);
            }
        }
    });
}