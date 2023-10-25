//Carga los roles activos
function loadActivos() {
    var pkg = {
        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', ''),
        AppToken: localStorage.getItem("AppToken")
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Roles/Get',
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
                    html += "   <td>" + this.estado.estado + "</td>";
                    html += "   <td width='175'>";
                    html += "       <center>";
                    html += "           <button type='button' title='Edición de Rol' class='btn btn-success' onclick='getRol(" + this.idRol + ", true)' data-toggle='modal' data-target='#staticNewRol'><i class='fas fa-edit'></i></button>";
                    html += "           <button type='button' title='Asignación de Permisos' class='btn btn-primary' onclick='getRol(" + this.idRol + ", false)' data-toggle='modal' data-target='#staticPermisions'><i class='fas fa-tasks'></i></button>";
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
                html += "   <td colspan='3'><center class='text-danger font-weight-bolder'>" + data.mensaje + "</center></td>";
                html += "</tr>";
                $("#tActivos").append(html);
            }
        }
    });
}

//Carga los roles inactivos
function loadInactivos() {
    var Obj = {
        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', ''),
        AppToken: localStorage.getItem("AppToken")
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Roles/GetInactivos',
        contentType: "Application/json",
        data: JSON.stringify(Obj),
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
                html += "   <td colspan='2'><center class='text-danger font-weight-bolder'>" + data.mensaje + "</center></td>";
                html += "</tr>";
                $("#tInactivos").append(html);
            }
        }
    });
}