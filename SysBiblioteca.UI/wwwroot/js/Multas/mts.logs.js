//Carga las multas pendientes
function loadMultasPendientes() {
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
                if ($.fn.dataTable.isDataTable('#tablePendientes')) {
                    $('#tablePendientes').DataTable().clear().destroy();
                }
                $("#tPendientes").html(null);
                $.each(data.datos, function () {
                    var html = "";
                    html += "<tr onclick='getMulta(" + this.idMulta + ")' data-toggle='modal' data-target='#staticFine'>";
                    html += '   <td scope="row"><center><img class="img-fluid" style="max-height: 50px;" src="' + this.fotoLibro + '"></center></th>';
                    html += "   <td>" + this.libro + "</td>";
                    html += "   <td>" + this.usuario + "</td>";
                    html += "   <td><center>" + this.diasRetraso + "</center></td>";
                    html += "   <td><center>$" + this.monto + "</center></td>";
                    html += "   <td class='text-center font-weight-bolder'>" + getBadge(this.estado) + "</td>";
                    html += "</tr>";
                    $("#tPendientes").append(html);
                });
                paginate('tablePendientes');
            }
            else {
                if ($.fn.dataTable.isDataTable('#tablePendientes')) {
                    $('#tablePendientes').DataTable().clear().destroy();
                }
                $("#tPendientes").html(null);
                var html = "";
                html += "<tr>";
                html += "   <td colspan='6'><center class='text-danger font-weight-bolder'>" + data.mensaje + "</center></td>";
                html += "</tr>";
                $("#tPendientes").append(html);
            }
        }
    });
}

//Carga las multas pagadas
function loadMultasPagadas() {
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
                if ($.fn.dataTable.isDataTable('#tableFinalizados')) {
                    $('#tableFinalizados').DataTable().clear().destroy();
                }
                $("#tFinalizados").html(null);
                $.each(data.datos, function () {
                    var html = "";
                    html += "<tr>";
                    html += '   <td scope="row"><center><img class="img-fluid" style="max-height: 50px;" src="' + this.fotoLibro + '"></center></th>';
                    html += "   <td>" + this.libro + "</td>";
                    html += "   <td>" + this.usuario + "</td>";
                    html += "   <td><center>" + this.diasRetraso + "</center></td>";
                    html += "   <td><center>$" + this.monto + "</center></td>";
                    html += "   <td>" + this.usuarioValidacion + "</td>";
                    html += "   <td>" + this.fechaValidacion + "</td>";
                    html += "</tr>";
                    $("#tFinalizados").append(html);
                });
                paginate('tableFinalizados');
            }
            else {
                if ($.fn.dataTable.isDataTable('#tableFinalizados')) {
                    $('#tableFinalizados').DataTable().clear().destroy();
                }
                $("#tFinalizados").html(null);
                var html = "";
                html += "<tr>";
                html += "   <td colspan='7'><center class='text-danger font-weight-bolder'>" + data.mensaje + "</center></td>";
                html += "</tr>";
                $("#tFinalizados").append(html);
            }
        }
    });
}

function getBadge(estado) {
    var badge = "";

    switch (estado) {
        case "Pagada":
            badge = '<span class="badge bg-success text-white">' + estado + '</span>'
            break;
        case "No Pagada":
            badge = '<span class="badge bg-danger text-white">' + estado + '</span>'
            break;
        case "Pendiente":
            badge = '<span class="badge bg-warning text-black">' + estado + '</span>'
            break;
    }

    return badge;
}