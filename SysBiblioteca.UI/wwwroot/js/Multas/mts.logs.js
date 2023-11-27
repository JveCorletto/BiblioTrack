//Carga las multas activas
function loadActivos() {
    var pkg = {
        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', '')
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Multas/GetMultas',
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
                    html += "   <td>" + this.datosMulta.Codigo_de_multa + "</td>";
                    html += "   <td>" + this.datosMulta.Codigo_de_prestamo + "</td>";
                    html += "   <td>" + this.estado.estado + "</td>";
                    html += "   <td>" + this.datosMulta.pago_fisico + "</td>";
                    html += "   <td>" + this.datosMulta.fecha_validacion + "</td>";
                    html += "   <td>" + this.datosMulta.comprobante_pago + "</td>";
                    html += "   <td width='175'>";
                    html += "       <center>";
                    html += "           <button type='button' title='Adjuntar Comprobante' class='btn btn-primary' onclick='getEmpleado(" + this.idUsuario + ", true)' data-toggle='modal' data-target='#staticEmpleado'><i class='fas fa-edit'></i></button>";
                    html += "           <button type='button' title='Visualizar Comprobante' class='btn btn-danger' onclick='deactivateEmpleado(" + this.idUsuario + ")'><i class='fas fa-times'></i></button>";
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

//Carga las multas inactivas
function loadInactivos() {
    var pkg = {
        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', '')
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Seguridad/GetEmpleadosInactivos',
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
                    html += "   <td>" + this.datosMulta.Codigo_de_multa + "</td>";
                    html += "   <td>" + this.datosMulta.Codigo_de_prestamo + "</td>";
                    html += "   <td>" + this.estado.estado + "</td>";
                    html += "   <td>" + this.datosMulta.pago_fisico + "</td>";
                    html += "   <td>" + this.datosMulta.fecha_validacion + "</td>";
                    html += "   <td>" + this.datosMulta.comprobante_pago + "</td>";
                    html += "   <td width='175'>";
                    html += "       <center>";
                    html += "           <button type='button' title='Activar' class='btn btn-success' onclick='activatePago(" + this.idMulta + ")'><i class='fas fa-check'></i></button>";
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