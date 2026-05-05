//Carga los prestamos Pendientes de Entrega
function loadPrestamosPendientes() {
    var pkg = {
        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', '')
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Prestamos/GetPendingLoans',
        contentType: "Application/json",
        data: JSON.stringify(pkg),
        success: function (data) {
            if (data.resultado == 1) {
                if ($.fn.dataTable.isDataTable('#tablePrestamosPendientes')) {
                    $('#tablePrestamosPendientes').DataTable().clear().destroy();
                }
                $("#tPrestamosPendientes").html("");
                $.each(data.datos, function () {
                    var html = "";
                    html += "<tr onclick='getPrestamo(" + this.idPrestamo + ")' data-toggle='modal' data-target='#staticLoan'>";
                    html += '   <td scope="row"><center><img class="img-fluid" style="max-height: 50px;" src="' + this.libro.fotoLibro + '"></center></th>';
                    html += "   <td>" + this.libro.libro + "</td>";
                    html += "   <td><center>" + this.diasPrestamo + "</center></td>";
                    html += "   <td>" + this.usuario + "</td>";
                    html += "   <td class='text-center font-weight-bolder'>" + getclass(this.estado) + "</td>";
                    html += "</tr>";
                    $("#tPrestamosPendientes").append(html);
                });
                paginate('tablePrestamosPendientes');
            }
            else {
                if ($.fn.dataTable.isDataTable('#tablePrestamosPendientes')) {
                    $('#tablePrestamosPendientes').DataTable().clear().destroy();
                }
                $("#tPrestamosPendientes").html(null);
                var html = "";
                html += "<tr>";
                html += "   <td colspan='5'><center class='text-danger font-weight-bolder'>" + data.mensaje + "</center></td>";
                html += "</tr>";
                $("#tPrestamosPendientes").append(html);
            }
        }
    });
}

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
                    html += "   <td class='text-center font-weight-bolder'>" + getclass(this.estado) + "</td>";
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
                html += "   <td colspan='5'><center class='text-danger font-weight-bolder'>" + data.mensaje + "</center></td>";
                html += "</tr>";
                $("#tActivos").append(html);
            }
        }
    });
}

//Carga los Autores para la Busqueda de Libros
function loadAutores() {
    var Obj = {
        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', '')
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Libros/GetAutores',
        contentType: "Application/json",
        data: JSON.stringify(Obj),
        success: function (data) {
            if (data.resultado == 1) {
                $("#AutorSearch").html(null);

                var html = "";
                html += "<option value='0'>Elija un Autor</option>";
                $.each(data.datos, function () {
                    html += "<option value='" + this.idAutor + "'>" + this.autor + "</option>";
                });
                $("#AutorSearch").html(html);
            }
            else {
                $("#AutorSearchAutor").html(null);

                var html = "";
                html += "<option value='0'>" + data.mensaje + "</option>";
                $("#AutorSearch").html(html);
            }
        }
    });
}

//Carga los Géneros Literários para la Busqueda de Libros
function loadGeneros() {
    var Obj = {
        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', '')
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Libros/GetGeneros',
        contentType: "Application/json",
        data: JSON.stringify(Obj),
        success: function (data) {
            if (data.resultado == 1) {
                $("#GeneroSearch").html(null);

                var html = "";
                html += "<option value='0'>Elija un G&eacute;nero Literario</option>";
                $.each(data.datos, function () {
                    html += "<option value='" + this.idGenero + "'>" + this.genero + "</option>";
                });
                $("#GeneroSearch").html(html);
            }
            else {
                $("#GeneroSearch").html(null);

                var html = "";
                html += "<option value='0'>" + data.mensaje + "</option>";
                $("#GeneroSearch").html(html);
            }
        }
    });
}

//Carga los empleados activos
function loadUsuarios() {
    var pkg = {
        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', '')
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Prestamos/GetUserForLoans',
        contentType: "Application/json",
        data: JSON.stringify(pkg),
        success: function (data) {
            if (data.resultado == 1) {
                if ($.fn.dataTable.isDataTable('#tableUsuarios')) {
                    $('#tableUsuarios').DataTable().clear().destroy();
                }
                $("#tUsuarios").html(null);

                $.each(data.datos, function () {
                    var html = "";
                    html += "<tr onclick='selectUser(" + this.idUsuario + ")'>";
                    html += "   <td>" + this.datosPersonales.dui + "</td>";
                    html += "   <td>" + this.usuario + "</td>";
                    html += "   <td>" + this.datosPersonales.apellidos + ", " + this.datosPersonales.nombres + "</td>";
                    html += "   <td>" + this.datosPersonales.telefono + "</td>";
                    html += "   <td>" + this.datosPersonales.fechaNacimiento + "</td>";
                    html += "   <td>" + this.datosPersonales.genero.genero + "</td>";
                    html += "</tr>";
                    $("#tUsuarios").append(html);
                });
                paginate('tableUsuarios');
            }
            else {
                if ($.fn.dataTable.isDataTable('#tableUsuarios')) {
                    $('#tableUsuarios').DataTable().clear().destroy();
                }
                $("#tUsuarios").html(null);
                var html = "";
                html += "<tr>";
                html += "   <td colspan='6'><center class='text-danger font-weight-bolder'>" + data.mensaje + "</center></td>";
                html += "</tr>";
                $("#tUsuarios").append(html);
            }
        }
    });
}

function getclass(estado) {
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