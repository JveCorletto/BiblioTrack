$('#btnSave').click(function () {
    if ($('#Rol').val().trim() != "") {
        var Obj = {
            Rol: $('#Rol').val(),

            Token: localStorage.getItem("UserToken"),
            ActualRute: window.location.hash.replace('#', ''),
            AppToken: localStorage.getItem("AppToken")
        };
        var api = localStorage.getItem('apiURL');

        $.ajax({
            type: 'POST',
            url: api + 'Roles/Create',
            contentType: "Application/json",
            data: JSON.stringify(Obj),
            success: function (data) {
                if (data.resultado == 1) {
                    Swal.fire({
                        title: 'Exito',
                        icon: "success",
                        html: data.message,
                        timer: 2000,
                        timerProgressBar: true,
                        didOpen: () => {
                            Swal.showLoading();
                        },
                    }).then(function () {
                        $('#btnCancel').click();
                        loadActivos();
                    });
                }
                else {
                    Swal.fire({
                        title: 'Error',
                        icon: "warning",
                        html: data.mensaje,
                        timer: 2000,
                        timerProgressBar: true,
                        didOpen: () => {
                            Swal.showLoading();
                        },
                    });
                }
            }
        });
    }
    else {
        Swal.fire({
            title: 'Validacion',
            icon: "warning",
            html: 'Para continuar, debe de brindar un nombre de Rol.',
            timer: 1000,
            timerProgressBar: true,
            didOpen: () => {
                Swal.showLoading();
            },
        });
    }
});

$('#btnEdit').click(function () {
    if ($('#Rol').val().trim() != "" && $('#IdRol').val() > 0) {
        var Obj = {
            IdRol: parseInt($('#IdRol').val()),
            Rol: $('#Rol').val(),

            Token: localStorage.getItem("UserToken"),
            ActualRute: window.location.hash.replace('#', ''),
            AppToken: localStorage.getItem("AppToken")
        };
        var api = localStorage.getItem('apiURL');

        $.ajax({
            type: 'POST',
            url: api + 'Roles/Update',
            contentType: "Application/json",
            data: JSON.stringify(Obj),
            success: function (data) {
                if (data.resultado == 1) {
                    Swal.fire({
                        title: 'Exito',
                        icon: "success",
                        html: data.message,
                        timer: 1000,
                        timerProgressBar: true,
                        didOpen: () => {
                            Swal.showLoading();
                        },
                    }).then(function () {
                        $('#btnCancel').click();
                        loadActivos();
                    });;
                }
                else {
                    Swal.fire({
                        title: 'Error',
                        icon: "warning",
                        html: data.message,
                        timer: 1000,
                        timerProgressBar: true,
                        didOpen: () => {
                            Swal.showLoading();
                        },
                    });
                }
            }
        });
    }
    else {
        Swal.fire({
            title: 'Validacion',
            icon: "warning",
            html: 'Para continuar, debe de brindar un nombre de Rol.',
            timer: 1000,
            timerProgressBar: true,
            didOpen: () => {
                Swal.showLoading();
            },
        });
    }
});

function getRol(IdRol, action) {
    var Obj = {
        IdRol: IdRol,

        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', ''),
        AppToken: localStorage.getItem("AppToken")
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Roles/GetById',
        contentType: "Application/json",
        data: JSON.stringify(Obj),
        success: function (data) {
            if (data.resultado == 1) {
                if (action) {
                    resetForm();

                    $("#tituloModal").text("Datos del Rol");

                    $('#IdRol').val(data.datos.idRol);
                    $('#Rol').val(data.datos.rol).attr("disabled", true);

                    $('#btnSave').hide();
                    $('#editionMode').show();
                }
                else {
                    $("#IdRolPermiso").val(data.datos.idRol);
                    $("#NombreRol").val(data.datos.rol);

                    loadPlataformas();
                    resetFormulario();
                }
            }
            else {
                Swal.fire({
                    title: 'Error',
                    icon: "warning",
                    html: data.mensaje,
                    timer: 1000,
                    timerProgressBar: true,
                    didOpen: () => {
                        Swal.showLoading();
                    },
                }).then(function () {
                    if (action) {
                        $('#btnCancel').click();
                    }
                    else {
                        $('#btnCancelRol').click();
                    }
                });
            }
        }
    });
}

function deactivateRol(IdRol) {
    Swal.fire({
        title: 'Validacion',
        text: "¿En verdad desea desactivar este rol?",
        icon: 'warning',
        showCancelButton: true,

        cancelButtonColor: '#181C32',
        confirmButtonColor: '#FF3D60',

        cancelButtonText: "Cancelar",
        confirmButtonText: "Desactivar",
    }).then((result) => {
        if (result.isConfirmed) {
            var Obj = {
                IdRol: IdRol,

                Token: localStorage.getItem("UserToken"),
                ActualRute: window.location.hash.replace('#', ''),
                AppToken: localStorage.getItem("AppToken")
            };
            var api = localStorage.getItem('apiURL');

            $.ajax({
                type: 'POST',
                url: api + 'Roles/Deactivate',
                contentType: "Application/json",
                data: JSON.stringify(Obj),
                success: function (data) {
                    if (data.resultado == 1) {
                        Swal.fire({
                            title: 'Exito',
                            icon: "success",
                            html: data.mensaje,
                            timer: 1000,
                            timerProgressBar: true,
                            didOpen: () => {
                                Swal.showLoading();
                            },
                        }).then(function () {
                            loadActivos();
                            loadInactivos();
                        });
                    }
                    else {
                        Swal.fire({
                            title: 'Error',
                            icon: "warning",
                            html: data.mensaje,
                            timer: 1000,
                            timerProgressBar: true,
                            didOpen: () => {
                                Swal.showLoading();
                            },
                        });
                    }
                }
            });
        }
    })
}

function activateRol(IdRol) {
    Swal.fire({
        title: 'Validacion',
        text: "¿En verdad desea activar este rol?",
        icon: 'warning',
        showCancelButton: true,

        cancelButtonColor: '#181C32',
        confirmButtonColor: '#08D1AD',

        cancelButtonText: "Cancelar",
        confirmButtonText: "Activar",
    }).then((result) => {
        if (result.isConfirmed) {
            var Obj = {
                IdRol: IdRol,

                Token: localStorage.getItem("UserToken"),
                ActualRute: window.location.hash.replace('#', ''),
                AppToken: localStorage.getItem("AppToken")
            };
            var api = localStorage.getItem('apiURL');

            $.ajax({
                type: 'POST',
                url: api + 'Roles/Activate',
                contentType: "Application/json",
                data: JSON.stringify(Obj),
                success: function (data) {
                    if (data.resultado == 1) {
                        Swal.fire({
                            title: 'Exito',
                            icon: "success",
                            html: data.mensaje,
                            timer: 1000,
                            timerProgressBar: true,
                            didOpen: () => {
                                Swal.showLoading();
                            },
                        }).then(function () {
                            loadActivos();
                            loadInactivos();
                        });;
                    }
                    else {
                        Swal.fire({
                            title: 'Error',
                            icon: "warning",
                            html: data.mensaje,
                            timer: 1000,
                            timerProgressBar: true,
                            didOpen: () => {
                                Swal.showLoading();
                            },
                        });
                    }
                }
            });
        }
    })
}