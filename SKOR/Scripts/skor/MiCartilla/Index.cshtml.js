$(document).ready(function () {

    
    pgMiCartilla.inicializar();

});


var pgMiCartilla = {

    indiceGrupoActual: 0,

    inicializar: function () {

        var idCartillaUsuario = $("#txtIdCartillaUsuario").val();

        pgMiCartilla.binds();
        CartillaUsuario.cargar(idCartillaUsuario, pgMiCartilla.refrescar);

    },

    binds: function () {

        $("#btnShareP").bind("click", function (e) {
            e.preventDefault();
            pgMiCartilla.comparte();
        }); 

        $("#lnkComparte").bind("click", function (e) {
            e.preventDefault();
            pgMiCartilla.comparte();
        }); 

        
        $("#btnGrupoSiguiente").bind("click", function (e) {
            e.preventDefault();


            var gs = CartillaUsuario.grupos;
            var ig = pgMiCartilla.indiceGrupoActual;


            if (gs.length - 1 > ig) {
                pgMiCartilla.indiceGrupoActual = ig + 1;
                pgMiCartilla.refrescar();

                pgMiCartilla.setBotones();
            }
        });

        $("#btnGrupoAnterior").bind("click", function (e) {
            e.preventDefault();


            var gs = CartillaUsuario.grupos;
            var ig = pgMiCartilla.indiceGrupoActual;


            if (ig > 0) {
                pgMiCartilla.indiceGrupoActual = ig - 1;
                pgMiCartilla.refrescar();

                pgMiCartilla.setBotones();

            }
        });

    },

    setBotones: function () {
        var gs = CartillaUsuario.grupos;
        var ig = pgMiCartilla.indiceGrupoActual;

        //escondo boton Sig

        if (gs.length - 1 > ig) {
            //$("#btnGrupoSiguiente").show();                
            $("#btnGrupoSiguiente").removeAttr("disabled");
        }
        else {
            //$("#btnGrupoSiguiente").hide();
            $("#btnGrupoSiguiente").attr("disabled", "disabled");
        }



        //escondo boton Ant
        if (ig > 0) {
            //$("#btnGrupoAnterior").show();
            $("#btnGrupoAnterior").removeAttr("disabled");
        }
        else {
            //$("#btnGrupoAnterior").hide();
            $("#btnGrupoAnterior").attr("disabled", "disabled");
        }



    },


    refrescar: function () {

        var ps = CartillaUsuario.miCartilla.MisPronosticos;

        $("#divPronC").html(pgMiCartilla.creaTablaPronosticos(ps));

        pgMiCartilla.setBotones();

    },

    comparte: function () {

        
        var ps = CartillaUsuario.miCartilla.MisPronosticos;

        var pr = ps[ps.length -1];
        if (pr.idPartido == 124) {


            var txteq = ""
            if (pr.idEquipoClasifica == 1) {
                txteq = pr.equipoPr1;
            } else {
                txteq = pr.equipoPr2;
            }

            var txt = "Mi pronóstico para el mundial es que la final será: ";
            txt = txt + pr.equipoPr1 + " VS " + pr.equipoPr2;
            txt = txt + " y el campéon mundial será: " + txteq;
            txt = txt + " . Haz tu pronóstico también en ¡Qué Golazo! ";
          
            //myApp.alert(txt, "¡Qué Golazo!");

            FB.ui({
                method: 'share',                
                quote: txt,
                href: 'http://www.quegolazo.com.ec/',
            }, function (response) { });

        }




    },


        //TODO: Reutilizar (1 funcion) la de editarpron


        creaTablaPronosticos: function (ps) {

            var html = "";

            var grupoActual = "";
            grupoActual = CartillaUsuario.grupos[pgMiCartilla.indiceGrupoActual]

            if (CartillaUsuario.grupos.length > 1) {
                $("#lblGrupo").html(grupoActual);
            } else {
                $("#lblGrupo").html("");                
                $("#btnGrupoSiguiente").hide();
                $("#btnGrupoAnterior").hide();

            }


            for (var i = 0; i < ps.length; i++) {
                var p = ps[i];

                if (p.grupo == grupoActual) {

                    //no muestro los no definidos aun:
                    if (p.idEquipoPr1 != 1 && p.idEquipoPr2 != 1) {
                        html = html + pgMiCartilla.creaTablaPronostico(i, p);
                    }

                }

                

            }

            return html;

        },


        creaTablaPronostico: function (indice, pr) {

            var table = '';

            table += '<div class="row"><div class="col-auto"><div class="data-table card">';

            table += '<table style="width:100%;">';

            if (indice==0) {
                table += '<thead><tr><th></th><th></th><th>Pron.</th><th>Real</th><th>Puntos</th></tr></thead>'
            }

            table += '<tbody>';
            table += pgMiCartilla.getRowPron(pr.idEquipoPr1, pr.equipoPr1, pr.equipo1,  pr.scoreP1, pr.score1, pr.puntos, true);
            table += pgMiCartilla.getRowPron(pr.idEquipoPr2, pr.equipoPr2, pr.equipo2, pr.scoreP2, pr.score2, 0, false);

            table += '</tbody>';
            table += '</table>';

            table += '</div></div></div>';

            return table

        },


        getRowPron: function (id, equipo, equipoReal, score, real, puntos, primera) {
            var txtScore;
            if (score == null || score == -1) { txtScore = "-" } else { txtScore = score }
            var txtReal;
            if (real == null || real == -1) { txtReal = "-" } else { txtReal = real }
            var txtPuntos;
            if (puntos == null || puntos == -1) { txtPuntos = "-" } else { txtPuntos = puntos }
            

            var row = '';
            row += '<tr>';
            row += '<td style="width:25px"><img class="logoEquipo" src="' + pgMiCartilla.getBandera(id) + '"/></td>';

            if (equipo == equipoReal) {
                row += '<td style="width:40%">' + equipo + '</td>';
            } else {

                if (equipoReal == null) {
                    row += '<td style="width:40%">' + equipo + '( ? ) </td>';
                } else {
                    row += '<td style="width:40%"><del>' + equipo + '</del>(' + equipoReal + ')</td>';
                }
                
            }
            
            row += '<td style="text-align:center; width:15%"><strong>' + txtScore + '</strong></td>';
            row += '<td style="text-align:center; width:15%"><strong>' + txtReal + '</strong></td>';
            if (primera) {
                row += '<strong><td rowspan="2" style="font-size:12px; font-weight:bold;text-align:center; width:15%;">';
                row += txtPuntos;
                row += '</strong></td>';
            }
            row += '</tr>';

            return row;
        },

      
    

        getBandera: function (id) {

            var urlBandera = SITEROOT + "/images/banderas/" + id + ".png";
            return urlBandera;

        }





    }
