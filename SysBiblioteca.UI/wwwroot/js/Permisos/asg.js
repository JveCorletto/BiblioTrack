function resetFormulario() {
    $('#Create').prop('checked', false).change();
    document.getElementById('Create').disabled = false;
    $('#Read').prop('checked', false).change();
    document.getElementById('Read').disabled = false;
    $('#Update').prop('checked', false).change();
    document.getElementById('Update').disabled = false;
    $('#Delete').prop('checked', false).change();
    document.getElementById('Delete').disabled = false;

    $("#frmAsiganciones").removeClass('was-validated');
    $('#Padres').show(); $('#Hijos').show(); $('#Nietos').show();
    $('#dataMenu').hide(); $('#btnEditAssign').hide(); $("#btnSaveAssign").show(); $("#btnCancelAssign").hide(); $("#btnTabs").hide();
}

function validateParms() {
    if ($('#IdPlataforma').val() > 0) {
        $('#formPermisos').show();

        renderRootMenu();
        ShowRoots();
    }
    else {
        $('#formPermisos').hide();
    }
}

function changeCreate() {
    if ($("#Create").prop('checked')) {
        $("#lbCreate").text('Sí');
    }
    else {
        $("#lbCreate").text('No');
    }
}

function changeRead() {
    if ($("#Read").prop('checked')) {
        $("#lbRead").text('Sí');
    }
    else {
        $("#lbRead").text('No');
    }
}

function changeUpdate() {
    if ($("#Update").prop('checked')) {
        $("#lbUpdate").text('Sí');
    }
    else {
        $("#lbUpdate").text('No');
    }
}

function changeDelete() {
    if ($("#Delete").prop('checked')) {
        $("#lbDelete").text('Sí');
    }
    else {
        $("#lbDelete").text('No');
    }
}