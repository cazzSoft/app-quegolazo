$(document).ready(function () {
    pgHome.inicializar();
});

var pgHome = {
    inicializar: function () {
        $(".califica").click(function () {
            var idPartido;
            var equipo1;
            var equipo2;

            $("#txtScoreEquipo1").val("0");
            $("#txtScoreEquipo2").val("0");
            idPartido = $(this).data("partido");
            equipo1 = $(this).data("equipo1");
            equipo2 = $(this).data("equipo2");
            //
            $('#txtIdPartidoGuardar').val(idPartido);
            $('#spnNombreEquipo1').html(equipo1);
            $('#spnNombreEquipo2').html(equipo2);
            //
            $('#modalPartido').modal()
        });

        $("#btnGuardarPartido").click(function () {
            pgHome.calificarPartido();
        });

        $("#btnRecalificarTodo").click(function () {
            pgHome.recalificarTodo();
        });
        
    },

    calificarPartido: function () {
        var score1, score2, idPartido;

        idPartido = $('#txtIdPartidoGuardar').val();
        score1 = $("#txtScoreEquipo1").val();
        score2 = $("#txtScoreEquipo2").val();
        htclibjs.Ajax({
            url: "/Home/CalificarPartido",
            data: JSON.stringify({ idPartido: idPartido, sc1: score1, sc2: score2}),
            success: function (r) {
                if (r.data == true) {
                    alert("Actualizado.");
                    location.reload();
                }
            }
        });
    },

    recalificarTodo: function () {

        htclibjs.Ajax({
            url: "/Home/RecalificarTodo",
            data: null,
            success: function (r) {
                if (r.data == true) {
                    alert("Recalificado.");
                    location.reload();
                }
            }
        });
    },
}