var pagina = "Inicio";
$(document).ready(function () {
    pgInicio.inicializar();
});


var pgInicio = {
    inicializar: function () {
        var min = 1; var max = 4;
        var number = Math.floor(Math.random() * (max - min + 1) + min);
        $("#imgBannerRotativo").attr("src", SITEROOT + "/images/bannersHome/" + number + ".jpg?ver=1.0.0");

        $('#clock').countdown('2018/06/14 10:00:00', function (event) {
            $(this).html(event.strftime('%D días %H:%M:%S'));
        });
    }
}