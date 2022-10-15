$(document).ready(function () {
    pgVerBando.inicializar();
});

var pgVerBando = {

    inicializar: function () {

        
        pgVerBando.popupUnido();
        pgVerBando.cargarMiembrosBando();
        $("#btnSalirEquipo").click(function () {
            var idB = $(this).data("id");
            var name = $(this).data("nombre");
            pgVerBando.salirBando(idB,name);
        });
    },

    popupUnido: function () {
        var ban = $("#txtNombreBando").val();
        var unir = $("#txtUnionBando").val();
        
        if (parseInt(unir) == 1) {
            var txt = "Te has unido al equipo:" + ban;
            txt = txt + ".<br>  Compite contra tus amigos, colegas y más. "
            txt = txt + "<br> Participa en los diferentes juegos y suma puntos para tu equipo."
            myApp.alert(txt, "Felicitaciones, ¡Qué Golazo!");
        }
    },
    cargarMiembrosBando: function () {

        var idB = $('#txtIdBando').val();

        htclibjs.Ajax({
            url: "/Bando/GetMisMiembros",
            data: JSON.stringify({ idBando: idB }),
            success: function (r) {
                var posiciones;
                var html = '';
                var contador = 0;

                $("#ulTablaMiembros").html("");
                usuarios = r.data;
                html = '<li class="table_row"><div class="table_section">Nombre</div></li>';
                $.each(usuarios, function (index, usuario) {
                    contador += 1;
                    html += '<li class="table_row">';
                    html += '<div class="table_section">' + usuario.nombre + ' ' + usuario.apellido + '</div>';
                    html += '</li>';
                });
                $("#ulTablaMiembros").html(html);

                $("#ulTablaMiembros").data("mostrar", 0);

                //posiciones del mundial
                pgVerBando.cargarPuestosBando();

            }
        });
    },

    cargarPuestosBando: function () {

        var idB = $('#txtIdBando').val();
        var idC = 12; //MUNDIAL por ahora        

        htclibjs.Ajax({
            url: "/Posiciones/getPosicionesBandos",
            data: JSON.stringify({ idCartilla: idC, idBando: idB }),
            success: function (r) {
                $("#tbodyPosicionesBando").html(r.data);                
            }
        });
    },

    salirBando: function (idBando, nombre) {
        myApp.confirm("Seguro de salir de " + nombre + "?", "Confirmación", function () {
            myApp.showPreloader();
            htclibjs.Ajax({
                url: "/Bando/SalirBando",
                data: JSON.stringify({ idB: idBando }),
                success: function (r) {
                    myApp.hidePreloader();
                    myApp.alert("Ha salido del grupo " + nombre, "Qué Golazo!", function () {
                        location.href = SITEROOT;
                    });
                }
            });
        }, null);

    },   

};