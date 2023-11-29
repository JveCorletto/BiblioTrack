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
                    debugger;
                    var html = "";
                    html += "<tr>";
                    html += "   <td>" + this.libro.libro + "</td>";
                    html += "   <td>" + this.autor + "</td>";
                    html += "   <td>" + this.fechaPrestamo + "</td>";
                    html += "   <td>" + this.fechaDevolucion + "</td>";
                    html += "   <td>" + getStatusBadge(this.estado) + "</td>"; // Utiliza la función para obtener el badge
                    html += "</tr>";
                    $("#tMyBooks").append(html);
                });

                // Aplicar DataTable a la tabla con configuración de lenguaje y renderizado de la quinta columna
                $('#tableActivos').DataTable({
                    language: {
                        "decimal": "",
                        "emptyTable": "No hay datos disponibles en la tabla",
                        "info": "Mostrando _START_ a _END_ de _TOTAL_ registros",
                        "infoEmpty": "Mostrando 0 a 0 de 0 registros",
                        "infoFiltered": "(filtrados de un total de _MAX_ registros)",
                        "infoPostFix": "",
                        "thousands": ",",
                        "lengthMenu": "Mostrar _MENU_ registros por página",
                        "loadingRecords": "Cargando...",
                        "processing": "Procesando...",
                        "search": "Buscar:",
                        "zeroRecords": "No se encontraron registros coincidentes",
                        "paginate": {
                            "first": "Primero",
                            "last": "Último",
                            "next": "Siguiente",
                            "previous": "Anterior"
                        },
                        "aria": {
                            "sortAscending": ": activar para ordenar la columna ascendente",
                            "sortDescending": ": activar para ordenar la columna descendente"
                        }
                    },
                    order: [[3, 'desc']], // Ordenar por la cuarta columna (Fecha Devolución) de forma descendente
                });

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
