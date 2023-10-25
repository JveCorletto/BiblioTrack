function verificarBase() {
    var url = window.location.pathname;
    var hash = window.location.hash;
  
    if (hash == '') {
        window.location.href = "/Budgeter#" + url;
    }
    else {
        if (typeof validateVista !== 'undefined' && jQuery.isFunction(validateVista)) validateVista();
    }
}

window.onload = verificarBase