if (typeof signalR !== "undefined") {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/rfidHub")
        .build();

    connection.on("ReceiveTag", function (tag) {
        console.log("Mensaje recibido desde SignalR: " + tag);
        const url = `/PrestamosDevoluciones/EventHandler?CodigoEjemplar=${tag}`;

        // Tamaño de la ventana emergente
        const popupWidth = 1040;
        const popupHeight = 600;

        // Calcular la posición para centrar la ventana
        const left = (screen.width / 2) - (popupWidth / 2);
        const top = (screen.height / 2) - (popupHeight / 2);

        // Características de la ventana emergente centrada
        const windowFeatures = `width=${popupWidth},height=${popupHeight},left=${left},top=${top},resizable=no,scrollbars=yes,status=no,menubar=no,resizable=no,directories=no`;

        // Abrir la ventana emergente
        window.open(url, '_blank', windowFeatures);
    });

    connection.start().catch(function (err) {
        return console.error(err.toString());
    });
} else {
    console.error("La biblioteca de SignalR no se ha cargado.");
}