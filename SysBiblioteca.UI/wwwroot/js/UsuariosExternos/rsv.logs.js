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