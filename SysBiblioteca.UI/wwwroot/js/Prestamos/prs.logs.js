//Carga los prestamos activos
function loadPrestamos() {
    var pkg = {
        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', '')
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Prestamos/GetActivePrestamos',
        contentType: "Application/json",
        data: JSON.stringify(pkg),
        success: function (data) {
            if (data.resultado == 1) {
                if ($.fn.dataTable.isDataTable('#tablePrestamos')) {
                    $('#tablePrestamos').DataTable().clear().destroy();
                }
                $("#tPrestamos").html("");
                $.each(data.datos, function () {
                    var html = "";
                    html += "<tr onclick='getPrestamo(" + this.idPrestamo + ")' data-toggle='modal' data-target='#staticEmpleado' class='" + getclass(this.estado) + "'>";
                    html += '   <td scope="row"><center><img class="img-fluid" style="max-height: 100px;" src="' + this.libro.fotoLibro + '"></center></th>';
                    html += "   <td>" + this.libro.libro + "</td>";
                    html += "   <td><center>" + this.diasPrestamo + "</center></td>";
                    html += "   <td>" + this.fechaPrestamo + "</td>";
                    html += "   <td>" + this.usuario + "</td>";
                    html += "   <td class='font-weight-bolder'>" + this.estado + "</td>";
                    html += "</tr>";
                    $("#tPrestamos").append(html);
                });
                paginate('tablePrestamos');
            }
            else {
                if ($.fn.dataTable.isDataTable('#tablePrestamos')) {
                    $('#tablePrestamos').DataTable().clear().destroy();
                }
                $("#tPrestamos").html(null);
                var html = "";
                html += "<tr>";
                html += "   <td colspan='9'><center class='text-danger font-weight-bolder'>" + data.mensaje + "</center></td>";
                html += "</tr>";
                $("#tPrestamos").append(html);
            }
        }
    });
}

function getclass(estado) {
    var badge = "";

    switch (estado) {
        case "A tiempo":
            badge = 'table-success'
            break;
        case "Demorado":
            badge = 'table-danger'
            break;
        case "No Entregado":
            badge = 'table-warning'
            break;
    }

    return badge;
}