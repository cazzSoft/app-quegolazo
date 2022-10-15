$(document).ready(function () {


    pgPosicionesGenerales.inicializar();

});

var pgPosicionesGenerales = {
    mostradoBando: false,
    inicializar: function () {
        pgPosicionesGenerales.binds();
        pgPosicionesGenerales.cargarPosicionesGenerales();
        pgPosicionesGenerales.bannerRotativo();
    },
    binds: function () {
        $("#hrfVermas").click(function () {
            var hayBando = $("#slcBandosPosiciones").length > 0 ? ($("#slcBandosPosiciones").val() == 0 ? false : true): false;
            if (hayBando) {
                pgPosicionesGenerales.cargarPosicionesGeneralesBandos();
            } else {
                pgPosicionesGenerales.cargarPosicionesGenerales();
            }
        });

        $("#hrfVermasGrupos").click(function () {
            pgPosicionesGenerales.cargarPosicionesGeneralesGrupos();
        });

        $("#slcBandosPosiciones").change(function () {
            $("#ulTablaPosiciones").data("mostrar", '0');
            if ($(this).val() == 0) {
                $("#imgBannerBando").hide();
                pgPosicionesGenerales.cargarPosicionesGenerales();
            } else {
                pgPosicionesGenerales.cargarPosicionesGeneralesBandos();
            }
            
        });

        $('#tabGrupos').on('tab:show', function () {
            if (pgPosicionesGenerales.mostradoBando == false) {
                $('#ulTablaPosicionesGrupos').data('ulTablaPosicionesGrupos', '0');
                pgPosicionesGenerales.cargarPosicionesGeneralesGrupos();
                pgPosicionesGenerales.mostradoBando = true;
            }
        });

    },
    cargarPosicionesGenerales: function () {
        var cantidad = $("#ulTablaPosiciones").data("mostrar");
        var idUsuarioActual = $("#txtIdUsuario").val();

        htclibjs.Ajax({
            url: "/Posiciones/GetPosicionesGenerales",
            data: JSON.stringify({ idUsuario: idUsuarioActual, cantidadActual: cantidad }),
            success: function (r) {
                var posiciones;
                var html = '';
                var contador = 0;

                $("#ulTablaPosiciones").html("");
                posiciones = r.data;
                html = '<li class="table_row"><div class="table_section_small">Pos.</div><div class="table_section">Nombre</div><div class="table_section_small">Pts</div></li>';
                $.each(posiciones, function (index, posicion) {
                    contador += 1;
                    html += '<li class="table_row">';
                    html += '<div class="table_section_small">' + posicion.posicion + '</div>';
                    html += '<div class="table_section ' + (posicion.idUsuario == idUsuarioActual ?'myPosition':'') + '">' + posicion.nombreCompleto + ' ';
                    if (contador == 1) {
                        html += '<img style="width:24px;display:inline;" title="El rey de ¡Qué Golazo!" alt="El rey de ¡Qué Golazo!" src="' + SITEROOT + '/images/skor/corona.png" />';
                    }
                    html += '</div>';
                    html += '<div class="table_section_small">' + posicion.puntos + '</div>';
                    html += '</li>';
                });
                $("#ulTablaPosiciones").html(html);

                cantidad = cantidad + 50;
                $("#ulTablaPosiciones").data("mostrar", cantidad);
            }
        });
    },
    cargarPosicionesGeneralesBandos: function () {
        var idBando_ = $('#slcBandosPosiciones').val();
        var cantidad = $("#ulTablaPosiciones").data("mostrar");
        var idUsuarioActual = $("#txtIdUsuario").val();

        myApp.showPreloader();
        htclibjs.Ajax({
            url: "/Posiciones/GetPosicionesGeneralesBando",
            data: JSON.stringify({ idUsuario: idUsuarioActual, idBando: idBando_, cantidadActual: cantidad }),
            success: function (r) {
                var posiciones;
                var html = '';
                var contador = 0;

                $("#ulTablaPosiciones").html("");
                posiciones = r.data;

                pgPosicionesGenerales.colocarImagenBannerBando(idBando_);

                html = '<li class="table_row"><div class="table_section_5">N.</div><div class="table_section_5">Posición</div><div class="table_section_5">Nombre</div><div class="table_section_5">Puntos</div></li>';
                $.each(posiciones, function (index, posicion) {
                    contador += 1;
                    html += '<li class="table_row">';
                    html += '<div class="table_section_5">' + contador + '</div>';
                    html += '<div class="table_section_5">' + posicion.posicion + '</div>';
                    html += '<div class="table_section_5">' + posicion.nombreCompleto + '</div>';
                    html += '<div class="table_section_5 centrado">' + posicion.puntos + '</div>';
                    html += '</li>';
                });
                $("#ulTablaPosiciones").html(html);

                cantidad = cantidad + 50;
                $("#ulTablaPosiciones").data("mostrar", cantidad);
                myApp.hidePreloader();
            }
        });
    },

    cargarPosicionesGeneralesGrupos: function () {
        var cantidad = $("#ulTablaPosicionesGrupos").data("mostrar");
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        myApp.showPreloader();
        htclibjs.Ajax({
            url: "/Posiciones/GetPosicionesGeneralesGrupos",
            data: JSON.stringify({cantidadActual: cantidad }),
            success: function (r) {
                var posiciones;
                var html = '';
                var contador = 0;

                $("#ulTablaPosicionesGrupos").html("");
                posiciones = r.data;

                html = '<li class="table_row"><div class="table_section_5">Posición</div><div class="table_section_5">Grupo</div><div class="table_section_5">Puntos</div><div class="table_section_5">#Pax</div><div class="table_section_5">Promedio</div></li>';
                $.each(posiciones, function (index, posicion) {
                    contador += 1;
                    html += '<li class="table_row">';
                    html += '<div class="table_section_5">' + posicion.posicion + '</div>';
                    html += '<div class="table_section_5">' + posicion.nombre + '';
                    if (contador == 1) {
                        html += '<img style="width:24px;display:inline;" title="El rey de ¡Qué Golazo!" alt="El rey de ¡Qué Golazo!" src="' + SITEROOT + '/images/skor/corona.png" />';
                    }
                    html += '</div>';
                    html += '<div class="table_section_5 centrado">' + posicion.puntos + '</div>';
                    html += '<div class="table_section_5 centrado">' + posicion.miembros + '</div>';
                    html += '<div class="table_section_5 centrado">' + posicion.prom + '</div>';
                    
                    html += '</li>';
                });
                $("#ulTablaPosicionesGrupos").html(html);

                cantidad = cantidad + 50;
                $("#ulTablaPosicionesGrupos").data("mostrar", cantidad);
                myApp.hidePreloader();
            }
        });

    },
    bannerRotativo: function () {
        var min = 1; var max = 4;
        var number = Math.floor(Math.random() * (max - min + 1) + min);
        $("#imgBannerRotativo").attr("src", SITEROOT + "/images/bannersHome/" + number + ".jpg");
    },

    colocarImagenBannerBando: function (idbando) {
        $("#imgBannerBando").show();
        $("#imgBannerBando").attr("src", SITEROOT + "/images/campannas/bandos/" + idbando + ".jpg");
    }
};