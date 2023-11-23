function loadSecciones() {
    var pkg = {
        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', '')
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Inventario/GetSecciones',
        contentType: "Application/json",
        data: JSON.stringify(pkg),
        success: function (data) {
            if (data.resultado == 1) {
                if ($.fn.dataTable.isDataTable('#tableSecciones')) {
                    $('#tableSecciones').DataTable().clear().destroy();
                }
                $("#tSecciones").html("");
                $.each(data.datos, function () {
                    var html = "";
                    html += "<tr>";
                    html += "   <td>" + this.seccion + "</td>";
                    html += "   <td>" + this.usuarioCreacion + "</td>";
                    html += "   <td>" + this.fechaCreacion + "</td>";
                    html += "   <td>" + (this.usuarioModificacion != null ? this.usuarioModificacion : "N/A") + "</td>";
                    html += "   <td>" + (this.fechaModificacion != null ? this.fechaModificacion : "N/A") + "</td>";
                    html += "   <td width='175'>";
                    html += "       <center>";
                    html += "           <button type='button' title='Editar' class='btn btn-primary' onclick='getSeccion(" + this.idSeccion + ", true)' data-toggle='modal' data-target='#staticSeccion'><i class='fas fa-edit'></i></button>";
                    html += "           <button type='button' title='Ver Estanterias' class='btn btn-success' onclick='getSeccion(" + this.idSeccion + ", false)' data-toggle='modal' data-target='#staticEstanterias'><i class='fas fa-list'></i></button>";
                    html += "           <button type='button' title='Eliminar' class='btn btn-danger' onclick='deleteSeccion(" + this.idUsuario + ")'><i class='fas fa-times'></i></button>";
                    html += "       </center>";
                    html += "   </td>";
                    html += "</tr>";
                    $("#tSecciones").append(html);
                });
                paginate('tableSecciones');
            }
            else {
                if ($.fn.dataTable.isDataTable('#tableSecciones')) {
                    $('#tableSecciones').DataTable().clear().destroy();
                }
                $("#tSecciones").html(null);
                var html = "";
                html += "<tr>";
                html += "   <td colspan='2'><center class='text-danger font-weight-bolder'>" + data.mensaje + "</center></td>";
                html += "</tr>";
                $("#tSecciones").append(html);
            }
        }
    });
}

function loadEstanterias(IdSeccion) {
    var pkg = {
        IdSeccion: IdSeccion,

        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', '')
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Inventario/GetEstanterias',
        contentType: "Application/json",
        data: JSON.stringify(pkg),
        success: function (data) {
            if (data.resultado == 1) {
                if ($.fn.dataTable.isDataTable('#tableEstanterias')) {
                    $('#tableEstanterias').DataTable().clear().destroy();
                }
                $("#tEstanterias").html("");
                $.each(data.datos, function () {
                    var html = "";
                    html += "<tr>";
                    html += "   <td>" + this.estanteria + "</td>";
                    html += "   <td>" + this.usuarioCreacion + "</td>";
                    html += "   <td>" + this.fechaCreacion + "</td>";
                    html += "   <td>" + (this.usuarioModificacion != null ? this.usuarioModificacion : "N/A") + "</td>";
                    html += "   <td>" + (this.fechaModificacion != null ? this.fechaModificacion : "N/A") + "</td>";
                    html += "   <td width='175'>";
                    html += "       <center>";
                    html += "           <button type='button' title='Editar' class='btn btn-primary' onclick='getEstanteria(" + this.idEstanteria + ", true)' data-toggle='modal' data-target='#staticNiveles'><i class='fas fa-edit'></i></button>";
                    html += "           <button type='button' title='Ver Niveles' class='btn btn-success' onclick='getEstanteria(" + this.idEstanteria + ", false)' data-toggle='modal' data-target='#staticNiveles'><i class='fas fa-list'></i></button>";
                    html += "           <button type='button' title='Eliminar' class='btn btn-danger' onclick='deleteEstanteria(" + this.idEstanteria + ")'><i class='fas fa-times'></i></button>";
                    html += "       </center>";
                    html += "   </td>";
                    html += "</tr>";
                    $("#tEstanterias").append(html);
                });
                paginate('tableEstanterias');
            }
            else {
                if ($.fn.dataTable.isDataTable('#tableEstanterias')) {
                    $('#tableEstanterias').DataTable().clear().destroy();
                }
                $("#tEstanterias").html(null);

                var html = "";
                html += "<tr>";
                html += "   <td colspan='2'><center class='text-danger font-weight-bolder'>" + data.mensaje + "</center></td>";
                html += "</tr>";
                $("#tEstanterias").append(html);
            }
        }
    });
}

function loadNiveles(IdEstanteria) {
    var pkg = {
        IdEstanteria: IdEstanteria,

        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', '')
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Inventario/GetNiveles',
        contentType: "Application/json",
        data: JSON.stringify(pkg),
        success: function (data) {
            if (data.resultado == 1) {
                if ($.fn.dataTable.isDataTable('#tableNiveles')) {
                    $('#tableNiveles').DataTable().clear().destroy();
                }
                $("#tNiveles").html("");

                $.each(data.datos, function () {
                    var html = "";
                    html += "<tr>";
                    html += "   <td>" + this.nivel + "</td>";
                    html += "   <td>" + this.usuarioCreacion + "</td>";
                    html += "   <td>" + this.fechaCreacion + "</td>";
                    html += "   <td>" + (this.usuarioModificacion != null ? this.usuarioModificacion : "N/A") + "</td>";
                    html += "   <td>" + (this.fechaModificacion != null ? this.fechaModificacion : "N/A") + "</td>";
                    html += "   <td width='175'>";
                    html += "       <center>";
                    html += "           <button type='button' title='Editar' class='btn btn-primary' onclick='getNivel(" + this.idNivel + ")' data-toggle='modal' data-target='#staticNivel'><i class='fas fa-edit'></i></button>";
                    html += "           <button type='button' title='Eliminar' class='btn btn-danger' onclick='deleteNivel(" + this.idNivel + ")'><i class='fas fa-times'></i></button>";
                    html += "       </center>";
                    html += "   </td>";
                    html += "</tr>";
                    $("#tNiveles").append(html);
                });
                paginate('tableNiveles');
            }
            else {
                if ($.fn.dataTable.isDataTable('#tableNiveles')) {
                    $('#tableNiveles').DataTable().clear().destroy();
                }
                $("#tNiveles").html(null);

                var html = "";
                html += "<tr>";
                html += "   <td colspan='2'><center class='text-danger font-weight-bolder'>" + data.mensaje + "</center></td>";
                html += "</tr>";
                $("#tNiveles").append(html);
            }
        }
    });
}