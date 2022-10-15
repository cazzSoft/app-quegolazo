$(document).ready(function () {
    pgClose.inicializar();
});

var pgClose = {
    inicializar: function () {
        $("#btnCerrarCartillas").click(function () {
            pgClose.cerrarCartillas();
        });
    },

    cerrarCartillas: function () {
        htclibjs.Ajax({
            url: "/General/CerrarCartillas",
            data: null,
            success: function (r) {
                if (r.data == true) {
                    alert("Cartillas cerradas");
                }
            }
        });
    }
}