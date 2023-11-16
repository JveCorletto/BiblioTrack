function loadAutores(IdAutor, action) {
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
                if (action) {
                    $("#Autor").html(null);
                    var html = "";
                    html += "<option value='0'>Elija un Autor</option>";
                    $.each(data.datos, function () {
                        html += "<option value='" + this.idAutor + "'>" + this.autor + "</option>";
                    });
                    $("#Autor").html(html);


                    if (IdAutor > 0) {
                        $("#Autor").val('' + IdAutor + '').change();
                    }
                    else {
                        $("#Autor").val('0').change();
                    }
                }
                else {
                    $("#AutorSearch, #AutorSearch2, #Autor").html(null);

                    var html = "";
                    html += "<option value='0'>Elija un Autor</option>";
                    $.each(data.datos, function () {
                        html += "<option value='" + this.idAutor + "'>" + this.autor + "</option>";
                    });
                    $("#AutorSearch, #AutorSearch2, #Autor").html(html);
                }
            }
            else {
                $("#AutorSearch, #AutorSearch2, #Autor").html(null);

                var html = "";
                html += "<option value='0'>" + data.mensaje + "</option>";
                $("#AutorSearch, #AutorSearch2, #Autor").html(html);
            }
        }
    });
}

function loadGeneros(IdGenero, action) {
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
                if (action) {
                    $("#GeneroLiterario").html(null);
                    var html = "";
                    html += "<option value='0'>Elija un G&eacute;nero Literario</option>";
                    $.each(data.datos, function () {
                        html += "<option value='" + this.idGenero + "'>" + this.genero + "</option>";
                    });
                    $("#GeneroLiterario").html(html);

                    if (IdGenero > 0) {
                        $("#GeneroLiterario").val('' + IdGenero + '').change();
                    }
                    else {
                        $("#GeneroLiterario").val('0').change();
                    }
                }
                else {
                    $("#GeneroSearch, #GeneroSearch2, #GeneroLiterario").html(null);

                    var html = "";
                    html += "<option value='0'>Elija un G&eacute;nero Literario</option>";
                    $.each(data.datos, function () {
                        html += "<option value='" + this.idGenero + "'>" + this.genero + "</option>";
                    });
                    $("#GeneroSearch, #GeneroSearch2, #GeneroLiterario").html(html);
                }
            }
            else {
                $("#GeneroSearch, #GeneroSearch2, #GeneroLiterario").html(null);

                var html = "";
                html += "<option value='0'>" + data.mensaje + "</option>";
                $("#GeneroSearch, #GeneroSearch2, #GeneroLiterario").html(html);
            }
        }
    });
}

function loadEditoriales(IdEditorial) {
    var Obj = {
        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', '')
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Libros/GetEditoriales',
        contentType: "Application/json",
        data: JSON.stringify(Obj),
        success: function (data) {
            if (data.resultado == 1) {
                $("#Editorial").html(null);
                var html = "";
                html += "<option value='0'>Elija una Editorial</option>";
                $.each(data.datos, function () {
                    html += "<option value='" + this.idEditorial + "'>" + this.editorial + "</option>";
                });
                $("#Editorial").html(html);

                if (IdEditorial > 0) {
                    $("#Editorial").val('' + IdEditorial + '').change();
                }
                else {
                    $("#Editorial").val('0').change();
                }
            }
            else {
                $("#Editorial").html(null);

                var html = "";
                html += "<option value='0'>" + data.mensaje + "</option>";
                $("#Editorial").html(html);
            }
        }
    });
}