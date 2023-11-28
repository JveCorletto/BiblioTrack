//Carga las multas activas
function loadMActivos() {
    var pkg = {
        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', '')
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Multas/GetMultasNoPagadas',
        contentType: "Application/json",
        data: JSON.stringify(pkg),
        success: function (data) {
            if (data.resultado == 1) {
                if ($.fn.dataTable.isDataTable('#tablePActivos')) {
                    $('#tablePActivos').DataTable().clear().destroy();
                }
                $("#tpActivos").html("");
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
                    html += "           <button type='button' title='Adjuntar Comprobante' class='btn btn-primary' onclick='getMulta(" + this.idMulta + ", true)' data-toggle='modal' data-target='#staticEmpleado'><i class='fas fa-edit'></i></button>";
                    html += "           <button type='button' title='Visualizar Comprobante' class='btn btn-danger' onclick='deactivateMulta(" + this.idMulta + ")'><i class='fas fa-times'></i></button>";
                    html += "       </center>";
                    html += "   </td>";
                    html += "</tr>";
                    $("#tpActivos").append(html);
                });
                paginate('tablePActivos');
            }
            else {
                if ($.fn.dataTable.isDataTable('#tablePActivos')) {
                    $('#tablePActivos').DataTable().clear().destroy();
                }
                $("#tpActivos").html(null);
                var html = "";
                html += "<tr>";
                html += "   <td colspan='9'><center class='text-danger font-weight-bolder'>" + data.mensaje + "</center></td>";
                html += "</tr>";
                $("#tpActivos").append(html);
            }
        }
    });
}

//Carga las multas inactivas
function loadMInactivos() {
    var pkg = {
        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', '')
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Multas/GetMultasPagadas',
        contentType: "Application/json",
        data: JSON.stringify(pkg),
        success: function (data) {
            if (data.resultado == 1) {
                if ($.fn.dataTable.isDataTable('#tablePInactivos')) {
                    $('#tablePInactivos').DataTable().clear().destroy();
                }
                $("#tpInactivos").html("");
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
                    $("#tpInactivos").append(html);
                });
                paginate('tablePInactivos');
            }
            else {
                if ($.fn.dataTable.isDataTable('#Inactivos')) {
                    $('#Inactivos').DataTable().clear().destroy();
                }
                $("#tpInactivos").html(null);
                var html = "";
                html += "<tr>";
                html += "   <td colspan='9'><center class='text-danger font-weight-bolder'>" + data.mensaje + "</center></td>";
                html += "</tr>";
                $("#tpInactivos").append(html);
            }
        }
    });
}

//Carga las multas pendientes
function loadMPendientes() {
    var pkg = {
        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', '')
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Multas/GetMultasPendientes',
        contentType: "Application/json",
        data: JSON.stringify(pkg),
        success: function (data) {
            if (data.resultado == 1) {
                if ($.fn.dataTable.isDataTable('#tablePPendientes')) {
                    $('#tablePPendientes').DataTable().clear().destroy();
                }
                $("#tpPendientes").html("");
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
                    $("#tpPendientes").append(html);
                });
                paginate('tablePPendientes');
            }
            else {
                if ($.fn.dataTable.isDataTable('#Pendientes')) {
                    $('#Pendientes').DataTable().clear().destroy();
                }
                $("#tpPendientes").html(null);
                var html = "";
                html += "<tr>";
                html += "   <td colspan='9'><center class='text-danger font-weight-bolder'>" + data.mensaje + "</center></td>";
                html += "</tr>";
                $("#tpPendientes").append(html);
            }
        }
    });
}