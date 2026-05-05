$("#btnSaveAssign").click(function () {
    if ($("#MenuRaiz").val() > 0 && $("#Menu").val() >= 0 && $("#MenuHijo").val() >= 0) {
        var Obj = {
            Menu: {
                IdMenu: parseInt($("#MenuHijo").val()),
                IdSubParent: parseInt($("#Menu").val()),
                IdParent: parseInt($("#MenuRaiz").val())
            },
            Create: $("#Create").prop('checked'),
            Read: $("#Read").prop('checked'),
            Update: $("#Update").prop('checked'),
            Delete: $("#Delete").prop('checked'),
            IdRol: parseInt($("#IdRolPermiso").val()),

            Token: localStorage.getItem("UserToken"),
            ActualRute: window.location.hash.replace('#', ''),
            AppToken: localStorage.getItem("AppToken")
        };
        var api = localStorage.getItem('apiURL');
        $("#frmAsiganciones").addClass('was-validated');

        $.ajax({
            type: 'POST',
            url: api + 'Permisos/AddMenuToRol',
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
                        renderRootMenu();
                        resetFormulario();
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
        $("#frmAsiganciones").addClass('was-validated');
    }
});

$("#btnEditAssign").click(function () {
    var Obj = {
        Create: $("#Create").prop('checked'),
        Read: $("#Read").prop('checked'),
        Update: $("#Update").prop('checked'),
        Delete: $("#Delete").prop('checked'),
        IdLinkRolMenu: parseInt($("#IdLinkRolMenuPais").val()),

        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', ''),
        AppToken: localStorage.getItem("AppToken")
    };
    var api = localStorage.getItem('apiURL');
    $("#frmAsiganciones").addClass('was-validated');

    $.ajax({
        type: 'POST',
        url: api + 'Permisos/PutMenu',
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
                    renderRootMenu();
                    resetFormulario();
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
});

function removeMenu(id) {
    Swal.fire({
        title: 'Validacion',
        text: "¿En verdad desea desasignar este menú de este rol en este país?",
        icon: 'warning',
        showCancelButton: true,

        cancelButtonColor: '#181C32',
        confirmButtonColor: '#FF3D60',

        cancelButtonText: "Cancelar",
        confirmButtonText: "Desasignar",
    }).then((result) => {
        if (result.isConfirmed) {
            var Obj = {
                IdMenu: id,
                IdRol: parseInt($("#IdRolPermiso").val()),

                Token: localStorage.getItem("UserToken"),
                ActualRute: window.location.hash.replace('#', ''),
                AppToken: localStorage.getItem("AppToken")
            };
            var api = localStorage.getItem('apiURL');

            $.ajax({
                type: 'POST',
                url: api + 'Permisos/RemoveMenuToRol',
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
                            renderRootMenu();
                            resetFormulario();
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
    });
}