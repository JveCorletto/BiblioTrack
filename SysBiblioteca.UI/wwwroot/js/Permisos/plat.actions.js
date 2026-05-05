//Registra una nueva Aplicación
$("#btnSave").click(function () {
    if ($("#NombreAplicacion").val().trim() != "") {
        var Obj = {
            NombreAplicacion: $("#NombreAplicacion").val().trim(),
            URLAplicacion: $("#URLAplicacion").val().trim(),

            Token: localStorage.getItem("UserToken"),
            ActualRute: window.location.hash.replace('#', ''),
            AppToken: localStorage.getItem("AppToken")
        };
        var api = localStorage.getItem('apiURL');
        $("#dataAplicacion").addClass('was-validated');

        $.ajax({
            type: 'POST',
            url: api + 'Aplicaciones/SaveAplicacion',
            contentType: "Application/json",
            data: JSON.stringify(Obj),
            success: function (data) {
                if (data.resultado == 1) {
                    Swal.fire({
                        title: 'Exito',
                        icon: "success",
                        html: data.mensaje,
                        timer: 2000,
                        timerProgressBar: true,
                        didOpen: () => {
                            Swal.showLoading();
                        },
                    }).then(function () {
                        $("#NombreAplicacion").attr("disabled", true);
                        $("#URLAplicacion").attr("disabled", true);

                        $("#IdAplicacion").val(data.datos.idAplicacion);
                        $("#TokenAplicacion").val(data.datos.tokenAplicacion);

                        $('#editionMode').show();
                        $('#btnSave').hide();
                        $('#btnEdit').hide();
                        $('#btnCancel').hide();

                        loadActivas();
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
    else {
        $("#dataAplicacion").addClass('was-validated');
    }
});

//Edita los datos de una plataforma existente
$("#btnEdit").click(function () {
    if ($("#NombreAplicacion").val().trim() != "") {
        var Obj = {
            IdAplicacion: parseInt($("#IdAplicacion").val()),

            NombreAplicacion: $("#NombreAplicacion").val().trim(),
            URLAplicacion: $("#URLAplicacion").val().trim(),

            Token: localStorage.getItem("UserToken"),
            ActualRute: window.location.hash.replace('#', ''),
            AppToken: localStorage.getItem("AppToken")
        };
        var api = localStorage.getItem('apiURL');
        $("#dataAplicacion").addClass('was-validated');

        $.ajax({
            type: 'POST',
            url: api + 'Aplicaciones/UpdateAplicacion',
            contentType: "Application/json",
            data: JSON.stringify(Obj),
            success: function (data) {
                if (data.resultado == 1) {
                    Swal.fire({
                        title: 'Exito',
                        icon: "success",
                        html: data.mensaje,
                        timer: 2000,
                        timerProgressBar: true,
                        didOpen: () => {
                            Swal.showLoading();
                        },
                    }).then(function () {
                        $("#NombreAplicacion").attr("disabled", true);
                        $("#URLAplicacion").attr("disabled", true);

                        $('#editionMode').show();
                        $('#btnSave').hide();
                        $('#btnEdit').hide();
                        $('#btnCancel').hide();

                        loadActivas();
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
    else {
        $("#dataPlataforma").addClass('was-validated');
    }
});

//desactiva una Aplicación en el sistema
function deactivateApp(IdAplicacion) {
    Swal.fire({
        title: 'Validacion',
        text: "¿En verdad desea desactivar esta Aplicación?",
        icon: 'warning',
        showCancelButton: true,

        cancelButtonColor: '#181C32',
        confirmButtonColor: '#FF3D60',

        cancelButtonText: "Cancelar",
        confirmButtonText: "Desactivar",
    }).then((result) => {
        if (result.isConfirmed) {
            var Obj = {
                IdAplicacion: parseInt(IdAplicacion),

                Token: localStorage.getItem("UserToken"),
                ActualRute: window.location.hash.replace('#', ''),
                AppToken: localStorage.getItem("AppToken")
            };
            var api = localStorage.getItem('apiURL');

            $.ajax({
                type: 'POST',
                url: api + 'Aplicaciones/Deactivate',
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
                            loadActivas();
                            loadInactivas();
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

//activa una Aplicación en el sistema
function activateApp(IdAplicacion) {
    Swal.fire({
        title: 'Validacion',
        text: "¿En verdad desea activar esta Aplicación?",
        icon: 'warning',
        showCancelButton: true,

        cancelButtonColor: '#181C32',
        confirmButtonColor: '#FF3D60',

        cancelButtonText: "Cancelar",
        confirmButtonText: "Activar",
    }).then((result) => {
        if (result.isConfirmed) {
            var Obj = {
                IdAplicacion: parseInt(IdAplicacion),

                Token: localStorage.getItem("UserToken"),
                ActualRute: window.location.hash.replace('#', ''),
                AppToken: localStorage.getItem("AppToken")
            };
            var api = localStorage.getItem('apiURL');

            $.ajax({
                type: 'POST',
                url: api + 'Aplicaciones/Activate',
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
                            loadActivas();
                            loadInactivas();
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