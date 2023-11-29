//Carga las multas pendientes del usuario
function loadMultasPendientes() {
    var pkg = {
        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', '')
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Multas/GetMyFines',
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
                html += "   <td colspan='5'><center class='text-danger font-weight-bolder'>" + data.mensaje + "</center></td>";
                html += "</tr>";
                $("#tPendientes").append(html);
            }
        }
    });
}

//Carga las multas pagadas del usuario
function loadMultasPagadas() {
    var pkg = {
        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', '')
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Multas/GetMyPaidFines',
        contentType: "Application/json",
        data: JSON.stringify(pkg),
        success: function (data) {
            if (data.resultado == 1) {
                if ($.fn.dataTable.isDataTable('#tablePagadas')) {
                    $('#tablePagadas').DataTable().clear().destroy();
                }
                $("#tPagadas").html(null);
                $.each(data.datos, function () {
                    var html = "";
                    html += "<tr>";
                    html += '   <td scope="row"><center><img class="img-fluid" style="max-height: 50px;" src="' + this.fotoLibro + '"></center></th>';
                    html += "   <td>" + this.libro + "</td>";
                    html += "   <td><center>" + this.diasRetraso + "</center></td>";
                    html += "   <td><center>$" + this.monto + "</center></td>";
                    html += "   <td><center>$" + this.usuarioValidacion + "</center></td>";
                    html += "   <td><center>$" + this.fechaValidacion + "</center></td>";
                    html += "</tr>";
                    $("#tPagadas").append(html);
                });
                paginate('tablePagadas');
            }
            else {
                if ($.fn.dataTable.isDataTable('#tablePagadas')) {
                    $('#tablePagadas').DataTable().clear().destroy();
                }
                $("#tPagadas").html(null);
                var html = "";
                html += "<tr>";
                html += "   <td colspan='6'><center class='text-danger font-weight-bolder'>" + data.mensaje + "</center></td>";
                html += "</tr>";
                $("#tPagadas").append(html);
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