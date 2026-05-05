"use strict";

var KTLogin = function () {
    var _login;

    var _handleSignInForm = function () {
        var validation;

        validation = FormValidation.formValidation(
            KTUtil.getById('kt_login_signin_form'),
            {
                fields: {
                    username: {
                        validators: {
                            notEmpty: {
                                message: 'El nombre de Usuario es requerido'
                            }
                        }
                    },
                    password: {
                        validators: {
                            notEmpty: {
                                message: 'La contraseña es requerida'
                            }
                        }
                    }
                },
                plugins: {
                    trigger: new FormValidation.plugins.Trigger(),
                    submitButton: new FormValidation.plugins.SubmitButton(),
                    bootstrap: new FormValidation.plugins.Bootstrap()
                }
            }
        );

        $('#kt_login_signin_submit').on('click', function (e) {
            e.preventDefault();

            validation.validate().then(function (status) {
                if (status == 'Valid') {
                    logIn();
                } else {
                    swal.fire({
                        text: "Por favor, rellene los campos para continuar.",
                        icon: "error",
                        buttonsStyling: false,
                        confirmButtonText: "Aceptar.",
                        customClass: {
                            confirmButton: "btn font-weight-bold btn-light-primary"
                        }
                    }).then(function () {
                        KTUtil.scrollTop();
                    });
                }
            });
        });
    }

    return {
        init: function () {
            _login = $('#kt_login');
            _handleSignInForm();
        }
    };
}();

jQuery(document).ready(function () {
    KTLogin.init();
});

function togglePassword() {
    var x = document.getElementById("Contrasenia");
    var y = document.getElementById("showPsw");
    if (x.type === "password") {
        x.type = "text";
        y.checked = true;
    } else {
        x.type = "password";
        y.checked = false;
    }
}

function logIn() {
    var api = localStorage.getItem('apiURL');
    $('#kt_login_signin_submit').prop('disabled', true);

    var obj = {
        Usuario: $("#Usuario").val().trim(),
        Contrasenia: $("#Contrasenia").val().trim()
    }

    $.ajax({
        type: 'POST',
        url: api + 'Authentication/LogIn',
        contentType: 'Application/json',
        data: JSON.stringify(obj),
        success: function (data) {
            if (data.resultado == 1) {
                swal.fire({
                    text: data.mensaje,
                    icon: "success",
                    buttonsStyling: false,
                    confirmButtonText: "Continuar!",
                    customClass: {
                        confirmButton: "btn font-weight-bold btn-light-primary"
                    }
                }).then(function () {
                    KTUtil.scrollTop();

                    localStorage.setItem('UserToken', data.datos.token);
                    var logUser = {
                        Usuario: data.datos.usuario,
                        Rol: data.datos.rol
                    };

                    $.ajax({
                        type: 'POST',
                        url: '/activateSesion',
                        contentType: "Application/json",
                        data: JSON.stringify(logUser),
                        crossDomain: true,
                        success: function (data) {
                            if (data.resultado == 1) {
                                localStorage.setItem('User', logUser.Usuario);
                                window.location = "/setProfile"
                            }
                            else {
                                alertify.error(data.mensaje);
                                $("#kt_login_signin_submit").prop('disabled', false);
                            }
                        }
                    });
                });
            } else {
                swal.fire({
                    text: data.mensaje,
                    icon: "error",
                    buttonsStyling: false,
                    confirmButtonText: "Aceptar.",
                    customClass: {
                        confirmButton: "btn font-weight-bold btn-light-primary"
                    }
                }).then(function () {
                    KTUtil.scrollTop();
                });
                $('#kt_login_signin_submit').prop('disabled', false);
            }
        },
        error: function (data) {
            swal.fire({
                text: data.mensaje,
                icon: "error",
                buttonsStyling: false,
                confirmButtonText: "Aceptar.",
                customClass: {
                    confirmButton: "btn font-weight-bold btn-light-primary"
                }
            }).then(function () {
                KTUtil.scrollTop();
            });
            $('#kt_login_signin_submit').prop('disabled', false);
        }
    })
}