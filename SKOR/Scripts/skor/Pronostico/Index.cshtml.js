/// <reference path="MiPronostico.js" />

$(document).ready(function () {
    
    pgPronostico.inicializar();
    
});



var pgPronostico = {

    indexPronosticoActual: 0,
    idPronosticoActual: 0,
    indiceGrupoActual: 0,
    idCartilla: 0,


    inicializar: function () {
        //click del 
        var idCartillaUsuario = $("#txtIdCartillaUsuario").val();
        pgPronostico.idCartilla = $("#txtIdCartilla").val();
        
        pgPronostico.binds();

        CartillaUsuario.cargar(idCartillaUsuario,pgPronostico.refrescar)

    },

    binds: function () {
        //click del popup
        $("#POP_btnActualiza").bind("click", function (e) {
            e.preventDefault();
            var s1 = $("#POP_txtScore1").val();
            var s2 = $("#POP_txtScore2").val();
            var numClasif = $('input[name=rdEq]:checked').val();
            if (numClasif == undefined || numClasif == null ) {numClasif =0}

            if (pgPronostico.verificaGanador()==true) {
                CartillaUsuario.guardarPronostico(pgPronostico.indexPronosticoActual, s1, s2, numClasif, pgPronostico.cerrar);  //OJO indice vs id
            }
        });


        $("#POP_btnSiguiente").bind("click", function (e) {
            e.preventDefault();
            var s1 = $("#POP_txtScore1").val();
            var s2 = $("#POP_txtScore2").val();
            var numClasif = $('input[name=rdEq]:checked').val();
            if (numClasif == undefined || numClasif == null) { numClasif = 0 }

            if (pgPronostico.verificaGanador() == true) {
                CartillaUsuario.guardarPronostico(pgPronostico.indexPronosticoActual, s1, s2, numClasif, pgPronostico.siguiente);  //OJO indice vs id
            }
        });

        //Sellar
        $("#btnSellar").bind("click", function (e) {
            e.preventDefault();           
            CartillaUsuario.sellar(pgPronostico.salir);  //OJO indice vs id
        }); 

        //botones mas y menos

        $(".qntyplus").bind("click", function (e) {
            e.preventDefault();

            var txt = $(this).data("field");
            var qt = $("#" + txt).val();
            $($("#" + txt)).val(qt - (- 1));
            pgPronostico.ganador();

        });

        $(".qntyminus").bind("click", function (e) {
            e.preventDefault();
            var txt = $(this).data("field");
            var qt = $("#" + txt).val();
            if (qt > 0) {
                $($("#" + txt)).val(qt - 1);
                pgPronostico.ganador();
            }

        });

        $("#btnGrupoSiguiente").bind("click", function (e) {
            e.preventDefault();
            

            var gs = CartillaUsuario.grupos;
            var ig = pgPronostico.indiceGrupoActual;

            
            if (gs.length -1 > ig ) {
                pgPronostico.indiceGrupoActual = ig + 1;
                pgPronostico.refrescar();

                pgPronostico.setBotones();
            }            
        });

        $("#btnGrupoAnterior").bind("click", function (e) {
            e.preventDefault();
            

            var gs = CartillaUsuario.grupos;
            var ig = pgPronostico.indiceGrupoActual;

            
            if ( ig > 0 ) {
                pgPronostico.indiceGrupoActual = ig - 1;
                pgPronostico.refrescar();

                pgPronostico.setBotones();

            }            
        });

    },

    setBotones: function () {
        var gs = CartillaUsuario.grupos;
        var ig = pgPronostico.indiceGrupoActual;

         //escondo boton Sig

            if (gs.length - 1 > ig ) {
                //$("#btnGrupoSiguiente").show();                
                $("#btnGrupoSiguiente").removeAttr("disabled");       
            }
            else {
                //$("#btnGrupoSiguiente").hide();
                $("#btnGrupoSiguiente").attr("disabled", "disabled");
        }



        //escondo boton Ant
        if (ig  > 0) {
            //$("#btnGrupoAnterior").show();
            $("#btnGrupoAnterior").removeAttr("disabled");       
        }
        else {
            //$("#btnGrupoAnterior").hide();
            $("#btnGrupoAnterior").attr("disabled", "disabled");
        }



    },


    //se debe seleccionar un ganador, en caso que aplique
    verificaGanador: function (ind) {        
        pr = CartillaUsuario.miCartilla.MisPronosticos[pgPronostico.indexPronosticoActual];

        if (pr.esClasificacion == true) {
            if ($("#rdEq1").prop("checked") == false && $("#rdEq2").prop("checked") == false) {

                myApp.alert("Si pronosticas empate, debes seleccionar quien gana los penales <br> Haz click sobre el equipo que gana", '¡Qué Golazo!');
                return false;
            } else {
                return true;
            }           
        } else {
            return true;
        }

    },

    ganador: function () {

        $("#rdEq1").prop("checked", false);
        $("#rdEq2").prop("checked", false);

        if ($("#POP_txtScore1").val() > $("#POP_txtScore2").val()) { $("#rdEq1").prop("checked", true); }
        if ($("#POP_txtScore1").val() < $("#POP_txtScore2").val()) { $("#rdEq2").prop("checked", true);}
    },

    cerrar: function () {
        //alert('cerrar');
        pgPronostico.refrescar();
        myApp.closeModal('.popup-pronostico');

        //por si acaso:
        pgPronostico.setBotones();


        var pr = CartillaUsuario.miCartilla.MisPronosticos[pgPronostico.indexPronosticoActual];
        if (pr.idPartido == 124) {

            var txteq = ""
            if (pr.idEquipoClasifica == 1) {
                txteq = pr.equipoPr1;
            } else {
                txteq = pr.equipoPr2;
            }

            myApp.alert(txteq + " Campeón Mundial! <br> <br> felicitaciones por completar tu pronóstico. <br> No olvides sellarlo para participar", "¡Qué Golazo!");
        }


    },

    siguiente: function () {
        
        pgPronostico.refrescar();

        if (pgPronostico.indexPronosticoActual < CartillaUsuario.miCartilla.MisPronosticos.length -1) {            
            pgPronostico.setPronosticoActual(pgPronostico.indexPronosticoActual + 1);
        } else {
            pgPronostico.cerrar();           

        }

    },

    salir: function () {
        location.href = SITEROOT + "/MiCartilla/Index/" + CartillaUsuario.idCartillaUsuario;
        //alert('redir a Micartilla');
    },


    refrescar: function () {

        var ps = CartillaUsuario.miCartilla.MisPronosticos;

        $("#divPron").html(pgPronostico.creaTablaPronosticos(ps));

        pgPronostico.setBotones();

        //bind click, de cada boton        
        $(".abrePronostico").bind("click", function (e) {
            e.preventDefault();
            var idP = $(this).data("idp");
            pgPronostico.setPronosticoActual(idP);  //OJO indice vs id
        });

    },

    setPronosticoActual: function (ind) {
        
        pgPronostico.indexPronosticoActual = ind;
        pr = CartillaUsuario.miCartilla.MisPronosticos[ind];
      
        //seteo el popup
        var fecha = Fecha.getDate(pr.fecha);

        $("#POP_lblNombrePartido").html(pr.descripcion);
        $("#spnFechaPartidoPronositico").html(fecha);
        $("#spnUbicacionPartidoPronositico").html(pr.ubicacion);
        
        $("#rdEq1").prop("checked", false);
        $("#rdEq2").prop("checked", false);

        if (pr.idEquipoClasifica == 1) { $("#rdEq1").prop('checked', true);}
        if (pr.idEquipoClasifica == 2) { $("#rdEq2").prop('checked', true); }

        if (pr.esClasificacion == true) {
            $("#spnEmpate").show();
            /*$("#rdEq1").show();
            $("#rdEq2").show();*/
            $("#rdbEquipo1").show();
            $("#rdbEquipo2").show();
            $("#POP_lblEquipo1").hide();
            $("#POP_lblEquipo2").hide();
            $("#trEncabezadoPronostico").show();
            
        } else {
            $("#spnEmpate").hide();
            /*$("#rdEq1").hide();
            $("#rdEq2").hide();*/
            $("#rdbEquipo1").hide();
            $("#rdbEquipo2").hide();
            $("#POP_lblEquipo1").show();
            $("#POP_lblEquipo2").show();
            $("#trEncabezadoPronostico").hide();

            $("#rdEq1").prop('checked', false);
            $("#rdEq2").prop('checked', false);
        }


        var bandera1 = pgPronostico.getBandera(pr.idEquipoPr1);
        var txtScore1;
        if (pr.scoreP1 == null || pr.scoreP1 == -1) { txtScore1 = "0" } else { txtScore1 = pr.scoreP1 }
        $("#POP_imgBandera1").attr("src", bandera1);
        $("#POP_lblEquipo1").html(pr.equipoPr1);
        $("#POP_lblEquipo1Radio").html(pr.equipoPr1);
        $("#POP_txtScore1").val(txtScore1);

        var bandera2 = pgPronostico.getBandera(pr.idEquipoPr2);
        var txtScore2;
        if (pr.scoreP2 == null || pr.scoreP2 == -1) { txtScore2 = "0" } else { txtScore2 = pr.scoreP2 }

        $("#POP_imgBandera2").attr("src", bandera2);
        $("#POP_lblEquipo2").html(pr.equipoPr2);
        $("#POP_lblEquipo2Radio").html(pr.equipoPr2);
        $("#POP_txtScore2").val(txtScore2);        


        $("#imgBanner2").attr("src", SITEROOT + "/images/campannas/banners/b2/" + pgPronostico.idCartilla + ".jpg");
               
        //recien acá levantar el popup?

    },




    creaTablaPronosticos: function (ps) {

        var html = "";

        var grupoActual = "";
        grupoActual = CartillaUsuario.grupos[pgPronostico.indiceGrupoActual]

        if (CartillaUsuario.grupos.length >1) {
            $("#lblGrupo").html(grupoActual);
        } else {
            $("#lblGrupo").html("");
            $("#btnGrupoSiguiente").hide();
            $("#btnGrupoAnterior").hide();
        }
        

        for (var i = 0; i < ps.length; i++) {
            var p = ps[i];

            //filtrado por grupo
            if (p.grupo == grupoActual) {

                //no muestro los no definidos aun:
                if (p.idEquipoPr1 != 1 && p.idEquipoPr2 != 1) {
                    html = html + pgPronostico.creaTablaPronostico(i, p);
                }

            }


            
        }

        return html;

    },


    creaTablaPronostico: function (indice, pr) {

        var clasif1 = false;
        var clasif2 = false;

        if (pr.esClasificacion == true){
            if (pr.idEquipoClasifica == 1) { clasif1 = true };
            if (pr.idEquipoClasifica == 2) { clasif2 = true };
        }
        var table = '';

        table += '<div class="row"><div class="col-auto"><div class="data-table card">';

        table += '<table style="width:100%;">';
        table += '<tbody>';
        table += pgPronostico.getRowPron(pr.idEquipoPr1, pr.equipoPr1, pr.scoreP1,clasif1, true);
        table += pgPronostico.getRowPron(pr.idEquipoPr2, pr.equipoPr2, pr.scoreP2, clasif2, false);
        table += pgPronostico.getRowBtn(indice, pr.scoreP1);
        table += '</tbody>';
        table += '</table>';

        table += '</div></div></div>';

        return table

    },


    getRowPron: function (id, equipo, score, check, primera) {
        var txtScore;
        if (score == null || score == -1) { txtScore = "-" }  else { txtScore = score }
        
        var row = '';
        row += '<tr>';
        row += '<td style="width:25px"><img class="logoEquipo" src="' + pgPronostico.getBandera(id) + '"/></td>';
        row += '<td style="width:50%">' + equipo ;

        if (check == true) {
            row += '<i class="icon icon-form-checkbox"> <img src="' + SITEROOT + '/images/skor/check.png" /> </i>';
        }

        row +=  '</td>';

        
        row += '<td><strong>' + txtScore + '</strong></td>';
        if (primera) {
            row += '<td rowspan="2">';
            //<i class="fa fa-info-circle"></i>
            row += '<a href="#" data-popup=".popup-pronostico" class="open-popup shopfav pull-left"></a>';
            row += '</td>';
        }
        row += '</tr>';

        return row;
    },

    getRowBtn: function (indice, score) {
        var btnValue = '';
        if (score == null || score == -1)
            btnValue = 'Pronosticar';
        else
            btnValue = 'Editar';

        var row = '';
        row += '<tr>';
        row += '<td colspan="4">';
        row += '<a href="#" data-idp="' + indice + '" data-popup=".popup-pronostico" class="abrePronostico open-popup shopfav pull-left  col button button-fill color-blue">';
        row += btnValue;
        row += '</a>';
        row += '</td>';
        row += '</tr>';

        return row;
    },

    

    getBandera: function (id) {
        
        var urlBandera = SITEROOT + "/images/banderas/" + id + ".png";
        return urlBandera;

    }




}