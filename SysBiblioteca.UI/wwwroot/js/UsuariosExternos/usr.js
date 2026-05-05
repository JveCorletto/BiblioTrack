$(document).ready(function () {
    verificarBase();

    $("#FechaNacimiento").datepicker({
        maxDate: '0',
        dateFormat: 'dd/mm/yy',
        selectOtherMonths: true,
        language: 'es',
    });

    $('#NewPassword').on('input', function () {
        var password = $(this).val();
        var strength = verificarFortaleza(password);
        actualizarFortaleza(strength);
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
                    loadMyData();
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

function verificarFortaleza(password) {
    var minLength = 8;
    var hasNumber = /\d/.test(password);
    var hasLowerCase = /[a-z]/.test(password);
    var hasUpperCase = /[A-Z]/.test(password);

    var strength = 0;

    if (password.length >= minLength) {
        strength += 1;
    }

    if (hasNumber) {
        strength += 1;
    }

    if (hasLowerCase) {
        strength += 1;
    }

    if (hasUpperCase) {
        strength += 1;
    }

    return strength;
}

function actualizarFortaleza(strength) {
    var strengthText;
    var strengthClass;

    switch (strength) {
        case 0:
            strengthText = "Muy D&eacute;bil";
            strengthClass = "bg-danger";
            break;
        case 1:
            strengthText = "D&eacute;bil";
            strengthClass = "bg-danger";
            break;
        case 2:
            strengthText = "Moderada";
            strengthClass = "bg-warning";
            break;
        case 3:
            strengthText = "Fuerte";
            strengthClass = "bg-success";
            break;
        case 4:
            strengthText = "Muy Fuerte";
            strengthClass = "bg-success";
            break;
        default:
            strengthText = "Desconocido";
            strengthClass = "bg-danger";
    }

    $('#strength-text').html(strengthText).attr('class', 'badge ' + strengthClass);
}

function resetForm() {
    $('#dataPassword').trigger('reset');
}