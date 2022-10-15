$(document).ready(function () {


    pgPosiciones.inicializar();

});

var pgPosiciones = {

    idCartilla: function () {
        var id = $("#txtIdCartilla").val();
        return id;
    },

    inicializar: function () {
        pgPosiciones.binds();
    },

    binds: function () {
        $("#hrfVerMasPosiciones").click(function () {

            myApp.showPreloader();
            htclibjs.Ajax({
                url: "/Posiciones/getAllPosiciones",
                data: JSON.stringify({ idCartilla: pgPosiciones.idCartilla() }),
                success: function (r) {
                    console.log(r.data);
                    $("#tbodyPosiciones").html(r.data);
                    $("#hrfVerMasPosiciones").hide();
                    myApp.hidePreloader();
                }
            });
        });

        
        $("#slcBandosPosiciones").change(function () {
            var idBando_ = $(this).val();

            $("#imgBannerBando").hide();
            myApp.showPreloader();
            htclibjs.Ajax({
                url: "/Posiciones/getPosicionesBandos",
                data: JSON.stringify({ idCartilla: pgPosiciones.idCartilla(), idBando:idBando_ }),
                success: function (r) {
                    var contador = 0;
                    pgPosiciones.colocarImagenBannerBando(idBando_);
                    $(".borrable").remove();
                    $("#tbodyPosiciones").html(r.data);
                    if (idBando_ != 0) {
                        $("#theadPosiciones th:first").before('<th class="borrable"></th>');
                        $('#tbodyPosiciones').find('tr').each(function () {
                            contador += 1;
                            $(this).find('td').eq(0).before('<td class="borrable" style="font-weight:bold;">' + contador + '</td>');
                        });

                    }
                    myApp.hidePreloader();
                }
            });
        });

    },

    colocarImagenBannerBando: function (idbando) {
        $("#imgBannerBando").show();
        $("#imgBannerBando").attr("src",SITEROOT+ "/images/campannas/bandos/"+idbando+".jpg");
    }
}