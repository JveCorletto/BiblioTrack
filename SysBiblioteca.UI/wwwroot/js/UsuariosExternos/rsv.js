$(document).ready(function () {
    verificarBase();

    var diasInput = $('#DiasPrestamo');
    diasInput.on('input', function () {
        var valor = parseInt(diasInput.val());

        $('#validation').empty();
        if (isNaN(valor) || valor <= 0 || valor >= 31) {
            var html = "";
            html += '<div class="alert alert-danger" role="alert">';
            html += '   Ingrese un n&uacute;mero v&aacute;lido (mayor a 0 y menor a 31)';
            html += '</div>';
            $('#validation').html(html);
            diasInput.val('');
        }
    });
});

function validateVista() {
    var pkg = {
        Token: localStorage.getItem("UserToken"),
        URL: window.location.hash.replace('#', '')
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Authentication/GetMenu',
        contentType: "Application/json",
        data: JSON.stringify(pkg),
        success: function (data) {
            if (data.resultado != null) {
                if (data.resultado == 1) {
                    loadAutores();
                    loadGeneros();
                }
                else {
                    window.location = "../../../";
                }
            }
            else {
                window.location = "../../../";
            }
        }
    });
}
