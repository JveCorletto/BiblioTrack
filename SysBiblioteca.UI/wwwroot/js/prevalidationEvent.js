function verificarUrl() {
    var url = window.location.href;
    var urlCorrectaBase = "https://localhost:7271/PrestamosDevoluciones/EventHandler";
    var urlParams = new URLSearchParams(window.location.search);
    var hash = window.location.hash;

    // Verifica si hay un hash en la URL, si la estructura no es la correcta o si no hay un CodigoEjemplar proporcionado
    if (hash !== '' || !url.startsWith(urlCorrectaBase) || !urlParams.has('CodigoEjemplar') || !urlParams.get('CodigoEjemplar')) {
        window.close(); // Cierra la pestaña si la URL no es correcta, contiene un hash o no tiene CodigoEjemplar
    } else {
        // Extraer y verificar el valor de CodigoEjemplar si es necesario
        var codigoEjemplar = urlParams.get('CodigoEjemplar');

        if (typeof validateState !== 'undefined' && jQuery.isFunction(validateState)) validateState();
    }
}

// Ejecutar la verificación al cargar la página
window.onload = verificarUrl;