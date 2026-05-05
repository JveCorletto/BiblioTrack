//Carga los empleados activos
function loadActivos() {
    var pkg = {
        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', '')
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Seguridad/GetRoles',
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
                    html += "   <td>" + this.rol + "</td>";
                    html += "   <td>" + this.fechaCreacion + "</td>";
                    html += "   <td width='175'>";
                    html += "       <center>";
                    html += "           <button type='button' title='Edición de Rol' class='btn btn-primary' onclick='getRol(" + this.idRol + ", true)' data-toggle='modal' data-target='#staticRol'><i class='fas fa-edit'></i></button>";
                    html += "           <button type='button' title='Desactivar' class='btn btn-danger' onclick='deactivateRol(" + this.idRol + ")'><i class='fas fa-times'></i></button>";
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
        url: api + 'Seguridad/GetRolesInactivos',
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
                    html += "   <td>" + this.rol + "</td>";
                    html += "   <td>" + this.fechaCreacion + "</td>";
                    html += "   <td width='175'>";
                    html += "       <center>";
                    html += "           <button type='button' title='Activar' class='btn btn-success' onclick='activateRol(" + this.idRol + ")'><i class='fas fa-check'></i></button>";
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