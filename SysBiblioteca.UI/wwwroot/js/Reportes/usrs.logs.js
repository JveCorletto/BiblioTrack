//Carga los Prestamos Entregados
function loadPrestamosActivos() {
    var pkg = {
        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', '')
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Prestamos/GetOngoingLoans',
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
                    html += '   <td scope="row"><center><img class="img-fluid" style="max-height: 50px;" src="' + this.libro.fotoLibro + '"></center></th>';
                    html += "   <td>" + this.libro.libro + "</td>";
                    html += "   <td><center>" + this.diasPrestamo + "</center></td>";
                    html += "   <td>" + this.fechaPrestamo + "</td>";
                    html += "   <td>" + this.usuario + "</td>";
                    html += "   <td>" + this.usuarioEntrego + "</td>";
                    html += "   <td class='text-center font-weight-bolder'>" + getBadge(this.estado) + "</td>";
                    html += "</tr>";
                    $("#tActivos").append(html);
                });
                paginate('tableActivos');
                $("#btnReportePrestamosActivos").attr("disabled", false);
            }
            else {
                if ($.fn.dataTable.isDataTable('#tableActivos')) {
                    $('#tableActivos').DataTable().clear().destroy();
                }
                $("#tActivos").html(null);
                var html = "";
                html += "<tr>";
                html += "   <td colspan='7'><center class='text-danger font-weight-bolder'>" + data.mensaje + "</center></td>";
                html += "</tr>";
                $("#tActivos").append(html);
                $("#btnReportePrestamosActivos").attr("disabled", true);
            }
        }
    });
}

//Carga los Prestamos Finalizados
function loadPrestamosFinalizados() {
    var pkg = {
        FechaDesde: ($("#FechaDesde").val() != "" ? $("#FechaDesde").val() : null),
        FechaHasta: ($("#FechaHasta").val() != "" ? $("#FechaHasta").val() : null),

        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', '')
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Devoluciones/GetFinishedLoans',
        contentType: "Application/json",
        data: JSON.stringify(pkg),
        success: function (data) {
            if (data.resultado == 1) {
                if ($.fn.dataTable.isDataTable('#tableHistorial')) {
                    $('#tableHistorial').DataTable().clear().destroy();
                }
                $("#tHistorial").html("");
                $.each(data.datos, function () {
                    var html = "";
                    html += '<tr>';
                    html += '   <td><center><img class="img-fluid" style="max-height: 50px;" src="' + this.libro.fotoLibro + '"></center></th>';
                    html += "   <td>" + this.libro.libro + "</td>";
                    html += "   <td>" + this.usuario + "</td>";
                    html += "   <td><center>" + this.diasPrestamo + "</center></td>";
                    html += "   <td>" + this.fechaPrestamo + "</td>";
                    html += "   <td>" + this.fechaDevolucion + "</td>";
                    html += "   <td>" + this.usuarioRecibio + "</td>";
                    html += "</tr>";
                    $("#tHistorial").append(html);
                });
                paginate('tableHistorial');
                $("#btnReportePrestamosFinalizados").attr("disabled", false);
            }
            else {
                if ($.fn.dataTable.isDataTable('#tableHistorial')) {
                    $('#tableHistorial').DataTable().clear().destroy();
                }
                $("#tHistorial").html(null);
                var html = "";
                html += "<tr>";
                html += "   <td colspan='7'><center class='text-danger font-weight-bolder'>" + data.mensaje + "</center></td>";
                html += "</tr>";
                $("#tHistorial").append(html);
                $("#btnReportePrestamosFinalizados").attr("disabled", true);
            }
        }
    });
}

function getBadge(estado) {
    var badge = "";

    switch (estado) {
        case "A tiempo":
            badge = '<span class="badge bg-success text-white">' + estado + '</span>'
            break;
        case "Demorado":
            badge = '<span class="badge bg-danger text-white">' + estado + '</span>'
            break;
        case "Pendiente":
            badge = '<span class="badge bg-warning text-black">' + estado + '</span>'
            break;
    }

    return badge;
}