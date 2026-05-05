//Carga las plataformas activas
function loadActivas() {
    var pkg = {
        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', ''),
        AppToken: localStorage.getItem("AppToken")
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Aplicaciones/GetActivas',
        contentType: "Application/json",
        data: JSON.stringify(pkg),
        success: function (data) {
            if (data.resultado == 1) {
                if ($.fn.dataTable.isDataTable('#tableActivas')) {
                    $('#tableActivas').DataTable().clear().destroy();
                }
                $("#tActivas").html("");
                $.each(data.datos, function () {
                    var html = "";
                    var fechaCreacion = new Date(this.fechaCreacion);
                    var fechaModificacion = new Date(this.fechaModificacion);
                    html += "<tr>";
                    html += "   <td>" + this.nombreAplicacion + "</td>";
                    html += "   <td>" + (this.urlAplicacion != null && this.urlAplicacion != "" ? "<a href='" + this.urlAplicacion + "' target='_blank' class='text-dark-75 text-hover-primary'>" + this.urlAplicacion + "</a>" : "N/D") + "</td>";
                    html += "   <td>" + (this.usuarioCreacion != null ? this.usuarioCreacion : "N/D") + "</td>";
                    html += "   <td>" + (this.fechaCreacion != null ? fechaCreacion.toLocaleDateString() : "N/D") + "</td>";
                    html += "   <td>" + (this.usuarioModificacion != null ? this.usuarioModificacion : "N/D") + "</td>";
                    html += "   <td>" + (this.fechaModificacion != null ? fechaModificacion.toLocaleDateString() : "N/D") + "</td>";
                    html += "   <td width='175'>";
                    html += "       <center>";
                    html += "           <button type='button' title='Edición de Plataformas' class='btn btn-success' onclick='getApp(" + this.idAplicacion + ", true)' data-toggle='modal' data-target='#staticPlataformas'><i class='fas fa-edit'></i></button>";
                    html += "           <button type='button' title='Desactivar' class='btn btn-danger' onclick='deactivateApp(" + this.idAplicacion + ")'><i class='fas fa-times'></i></button>";
                    html += "       </center>";
                    html += "   </td>";
                    html += "</tr>";
                    $("#tActivas").append(html);
                });
                paginate('tableActivas');
            }
            else {
                if ($.fn.dataTable.isDataTable('#tableActivas')) {
                    $('#tableActivas').DataTable().clear().destroy();
                }
                $("#tActivas").html(null);
                var html = "";
                html += "<tr>";
                html += "   <td colspan='8'><center class='text-danger font-weight-bolder'>" + data.mensaje + "</center></td>";
                html += "</tr>";
                $("#tActivas").append(html);
            }
        }
    });
}

//Carga las plataformas inactivas
function loadInactivas() {
    var pkg = {
        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', ''),
        AppToken: localStorage.getItem("AppToken")
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Aplicaciones/GetInactivas',
        contentType: "Application/json",
        data: JSON.stringify(pkg),
        success: function (data) {
            if (data.resultado == 1) {
                if ($.fn.dataTable.isDataTable('#tableInactivas')) {
                    $('#tableInactivas').DataTable().clear().destroy();
                }
                $("#tInactivas").html("");
                $.each(data.datos, function () {
                    var html = "";
                    var fechaCreacion = new Date(this.fechaCreacion);
                    var fechaModificacion = new Date(this.fechaModificacion);

                    html += "<tr>";
                    html += "   <td>" + this.nombreAplicacion + "</td>";
                    html += "   <td>" + (this.urlAplicacion != null && this.urlAplicacion != "" ? this.urlAplicacion : "N/D") + "</td>";
                    html += "   <td>" + (this.usuarioCreacion != null ? this.usuarioCreacion : "N/D") + "</td>";
                    html += "   <td>" + (this.fechaCreacion != null ? fechaCreacion.toLocaleDateString() : "N/D") + "</td>";
                    html += "   <td>" + (this.usuarioModificacion != null ? this.usuarioModificacion : "N/D") + "</td>";
                    html += "   <td>" + (this.fechaModificacion != null ? fechaModificacion.toLocaleDateString() : "N/D") + "</td>";
                    html += "   <td width='175'>";
                    html += "       <center>";
                    html += "           <button type='button' title='Activar' class='btn btn-success' onclick='activateApp(" + this.idAplicacion + ")'><i class='fas fa-times'></i></button>";
                    html += "       </center>";
                    html += "   </td>";
                    html += "</tr>";
                    $("#tInactivas").append(html);
                });
                paginate('tableInactivas');
            }
            else {
                if ($.fn.dataTable.isDataTable('#tableInactivas')) {
                    $('#tableInactivas').DataTable().clear().destroy();
                }
                $("#tInactivas").html(null);
                var html = "";
                html += "<tr>";
                html += "   <td colspan='8'><center class='text-danger font-weight-bolder'>" + data.mensaje + "</center></td>";
                html += "</tr>";
                $("#tInactivas").append(html);
            }
        }
    });
}

//Obtiene la información de la plataforma para mostrarla en pantalla
function getApp(IdAplicacion) {
    var Obj = {
        IdAplicacion: parseInt(IdAplicacion),

        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', ''),
        AppToken: localStorage.getItem("AppToken")
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Aplicaciones/GetAplicacion',
        contentType: "Application/json",
        data: JSON.stringify(Obj),
        success: function (data) {
            if (data.resultado == 1) {
                resetForm(false);

                $("#IdAplicacion").val(data.datos.idAplicacion);
                $("#NombreAplicacion").val(data.datos.nombreAplicacion);
                $("#URLAplicacion").val(data.datos.urlAplicacion);
                $("#TokenAplicacion").val(data.datos.tokenAplicacion);
            }
            else {
                Swal.fire({
                    title: 'Error',
                    icon: "error",
                    html: data.mensaje,
                    timer: 2000,
                    timerProgressBar: true,
                    didOpen: () => {
                        Swal.showLoading();
                    },
                }).then(function () {
                    $('#btnCancel').click();
                });
            }
        },
        error: function (data) {
            Swal.fire({
                title: 'Error',
                icon: "error",
                html: data.mensaje,
                timer: 2000,
                timerProgressBar: true,
                didOpen: () => {
                    Swal.showLoading();
                },
            }).then(function () {
                $('#btnCancel').click();
            });
        }
    });
}