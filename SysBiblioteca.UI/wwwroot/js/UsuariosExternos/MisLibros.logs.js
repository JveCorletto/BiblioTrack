function loadMyBooks() {
    var pkg = {
        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', '')
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Prestamos/GetMyBooks',
        contentType: "Application/json",
        data: JSON.stringify(pkg),
        success: function (data) {
            if (data.resultado == 1) {
                if ($.fn.dataTable.isDataTable('#tableActivos')) {
                    $('#tableActivos').DataTable().clear().destroy();
                }
                $("#tMyBooks").html(null);

                $.each(data.datos, function () {
                    var html = "";
                    html += "<tr>";
                    html += '   <td><center><img class="img-fluid" style="max-height: 50px;" src="' + this.libro.fotoLibro + '"></center></th>';
                    html += "   <td>" + this.libro.libro + "</td>";
                    html += "   <td><center>" + this.autores + "</td>";
                    html += "   <td><center>" + this.diasPrestamo + "</center></td>";
                    html += "   <td><center>" + this.fechaPrestamo + "</td>";
                    html += "   <td class='text-center font-weight-bolder'>" + getStatusBadge(this.estado) + "</td>"; // Utiliza la función para obtener el badge
                    html += "</tr>";
                    $("#tMyBooks").append(html);
                });

                paginate('tableActivos');

                // Alerta si hay libros demorados
                var librosDemorados = $('#tableActivos tbody tr[data-estado="Demorado"]').length;
                if (librosDemorados > 0) {
                    $('#cantidadConDemora').text(librosDemorados);
                    $('#alertaConDemora').show();
                }
            }
            else {
                if ($.fn.dataTable.isDataTable('#tableActivos')) {
                    $('#tableActivos').DataTable().clear().destroy();
                }
                $("#tMyBooks").html(null);
                var html = "";
                html += "<tr>";
                html += "   <td colspan='5'><center class='text-danger font-weight-bolder'>" + data.mensaje + "</center></td>";
                html += "</tr>";
                $("#tMyBooks").append(html);
            }
        }
    });
}

// Función para obtener el badge según el estado devuelto por la API
function getStatusBadge(estado) {
    if (estado === 'A tiempo') {
        return '<span class="badge bg-success text-white">A tiempo</span>';
    } else if (estado === 'Demorado') {
        return '<span class="badge bg-danger text-white">Demorado</span>';
    } else if (estado === 'Pendiente') {
        return '<span class="badge bg-warning text-white">Pendiente</span>';
    } else {
        return '<span class="badge bg-secondary text-white">' + estado + '</span>';
    }
}
