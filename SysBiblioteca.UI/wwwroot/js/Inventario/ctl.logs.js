function loadAutores() {
    var pkg = {
        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', '')
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Libros/GetAutores',
        contentType: "Application/json",
        data: JSON.stringify(pkg),
        success: function (data) {
            if (data.resultado == 1) {
                if ($.fn.dataTable.isDataTable('#tableAutores')) {
                    $('#tableAutores').DataTable().clear().destroy();
                }

                $("#tAutores").html("");
                $.each(data.datos, function () {
                    var html = "";
                    html += "<tr>";
                    html += "   <td>" + this.autor + "</td>";
                    html += "   <td>";
                    html += "       <center>";
                    html += "           <button type='button' title='Editar' class='btn btn-primary' onclick='getAutor(" + this.idAutor + ")' data-toggle='modal' data-target='#staticAutores'><i class='fas fa-edit'></i></button>";
                    html += "           <button type='button' title='Eliminar' class='btn btn-danger' onclick='deleteAutor(" + this.idAutor + ")'><i class='fas fa-times'></i></button>";
                    html += "       </center>";
                    html += "   </td>";
                    html += "</tr>";
                    $("#tAutores").append(html);
                });

                paginate('tableAutores');
            }
            else {
                if ($.fn.dataTable.isDataTable('#tableAutores')) {
                    $('#tableAutores').DataTable().clear().destroy();
                }
                $("#tAutores").html(null);
                var html = "";
                html += "<tr>";
                html += "   <td colspan='2'><center class='text-danger font-weight-bolder'>" + data.mensaje + "</center></td>";
                html += "</tr>";
                $("#tAutores").append(html);
            }
        }
    });
}

function loadEditoriales() {
    var pkg = {
        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', '')
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Libros/GetEditoriales',
        contentType: "Application/json",
        data: JSON.stringify(pkg),
        success: function (data) {
            if (data.resultado == 1) {
                if ($.fn.dataTable.isDataTable('#tableEditoriales')) {
                    $('#tableEditoriales').DataTable().clear().destroy();
                }
                $("#tEditoriales").html("");
                $.each(data.datos, function () {
                    var html = "";
                    html += "<tr>";
                    html += "   <td>" + this.editorial + "</td>";
                    html += "   <td>";
                    html += "       <center>";
                    html += "           <button type='button' title='Editar' class='btn btn-primary' onclick='getEditorial(" + this.idEditorial + ")' data-toggle='modal' data-target='#staticEditoriales'><i class='fas fa-edit'></i></button>";
                    html += "           <button type='button' title='Eliminar' class='btn btn-danger' onclick='deleteEditorial(" + this.idEditorial + ")'><i class='fas fa-times'></i></button>";
                    html += "       </center>";
                    html += "   </td>";
                    html += "</tr>";
                    $("#tEditoriales").append(html);
                });
                paginate('tableEditoriales');
            }
            else {
                if ($.fn.dataTable.isDataTable('#tableEditoriales')) {
                    $('#tableEditoriales').DataTable().clear().destroy();
                }
                $("#tEditoriales").html(null);
                var html = "";
                html += "<tr>";
                html += "   <td colspan='2'><center class='text-danger font-weight-bolder'>" + data.mensaje + "</center></td>";
                html += "</tr>";
                $("#tEditoriales").append(html);
            }
        }
    });
}

function loadGenerosLiterarios() {
    var pkg = {
        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', '')
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Libros/GetGeneros',
        contentType: "Application/json",
        data: JSON.stringify(pkg),
        success: function (data) {
            if (data.resultado == 1) {
                if ($.fn.dataTable.isDataTable('#tableGenerosLiterarios')) {
                    $('#tableGenerosLiterarios').DataTable().clear().destroy();
                }
                $("#tGenerosLiterarios").html("");
                $.each(data.datos, function () {
                    var html = "";
                    html += "<tr>";
                    html += "   <td>" + this.genero + "</td>";
                    html += "   <td>";
                    html += "       <center>";
                    html += "           <button type='button' title='Editar' class='btn btn-primary' onclick='getGenero(" + this.idGenero + ")' data-toggle='modal' data-target='#staticGenerosLiterarios'><i class='fas fa-edit'></i></button>";
                    html += "           <button type='button' title='Eliminar' class='btn btn-danger' onclick='deleteGenero(" + this.idGenero + ")'><i class='fas fa-times'></i></button>";
                    html += "       </center>";
                    html += "   </td>";
                    html += "</tr>";
                    $("#tGenerosLiterarios").append(html);
                });
                paginate('tableGenerosLiterarios');
            }
            else {
                if ($.fn.dataTable.isDataTable('#tableGenerosLiterarios')) {
                    $('#tableGenerosLiterarios').DataTable().clear().destroy();
                }
                $("#tGenerosLiterarios").html(null);
                var html = "";
                html += "<tr>";
                html += "   <td colspan='2'><center class='text-danger font-weight-bolder'>" + data.mensaje + "</center></td>";
                html += "</tr>";
                $("#tGenerosLiterarios").append(html);
            }
        }
    });
}