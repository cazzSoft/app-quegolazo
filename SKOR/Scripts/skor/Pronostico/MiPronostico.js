
//cargar la cartilla completa




function posicion(id) {
    this.id = id;
    this.puntos = 0;
    this.GD = 0;
    this.posicion = 0;
    this.gana = 0;
    this.goles = 0;
}


var CartillaUsuario = {

    idCartillaUsuario: 0,
    miCartilla: {},

    grupos: ["A1", "B"],

    cargar: function (idCartillaUsuario, queHacer) {

        CartillaUsuario.idCartillaUsuario = idCartillaUsuario;


        htclibjs.Ajax({
            url: "/Cartilla/GetMiCartillaUsuario",
            data: JSON.stringify({ idcu: idCartillaUsuario }),
            success: function (r) {
                console.log(r);
                if (r.exitoso == true) {
                    CartillaUsuario.miCartilla = r.data;
                    CartillaUsuario.llenaGrupos();
                    queHacer();
                    console.log(CartillaUsuario.miCartilla.MisAvancesfase);
                }
            }
        });


    },

    llenaGrupos: function () {

        //limpio
        CartillaUsuario.grupos = [];

        var mp = CartillaUsuario.miCartilla.MisPronosticos;

        var existe= false;

        for (var i = 0; i < mp.length; i++) {
            pr = CartillaUsuario.miCartilla.MisPronosticos[i];

            //busco que no exista            
            existe=false;
            for (var j = 0; j < CartillaUsuario.grupos.length; j++) {
                if (CartillaUsuario.grupos[j] == pr.grupo  ) {
                    existe =true;    
                    break;
                }
            }

            if (existe == false  ) {CartillaUsuario.grupos.push(pr.grupo);}            
        }

    },

    guardarPronostico: function (ind,s1,s2, numClasifica, queHacer) {        

        pr = CartillaUsuario.miCartilla.MisPronosticos[ind];

        pr.scoreP1 = s1;
        pr.scoreP2 = s2;
        pr.idEquipoClasifica = numClasifica;

        //guardo:
        myApp.showPreloader();

        htclibjs.Ajax({
            url: "/MiPronostico/GuardaPronostico",
            data: JSON.stringify({ idp: pr.id, Sp1: pr.scoreP1, Sp2: pr.scoreP2, numCl:numClasifica}),
            success: function (r) {
                console.log(r);
                if (r.exitoso == true) {
                    //grupo 4 es SIN GRUPO
                    if (pr.idGrupo == 4) {
                        queHacer();
                    } else {
                        CartillaUsuario.calculaGrupo(pr.idGrupo, queHacer);
                    }
                  
                    try {
                        myApp.hidePreloader();
                    } catch (e) {
                    }
                }
            }
        });


    },

    calculaGrupo: function (idGrupo, queHacer) {
        //busca todos los pronosticos de este grupo

        //var fases = [{ idGrupo: 1, posicion: 1, numeroEquipo: 1, idPartido: 8 }, { idGrupo: 2, posicion: 1, numeroEquipo: 2, idPartido: 8 }];
        var fases = CartillaUsuario.miCartilla.MisAvancesfase;
        console.log("FASES");
        console.log(fases);

        self = CartillaUsuario;

        var todos = self.miCartilla.MisPronosticos; 
        var prsG = [];

        //Filtro los del grupo
        for (var i = 0; i < todos.length; i++) {
            if (todos[i].idGrupo == idGrupo) {
                prsG.push(todos[i]);
            }                                     
        }
        

        //aqui voy a guardar la posiciones del grupo:
        var posiciones= [];

        for (var i = 0; i < prsG.length; i++) {
            var pr = prsG[i];

            //si falta llenar, no hago nada
            if (pr.scoreP1 == null || pr.scoreP1 == -1 || pr.scoreP2 == null || pr.scoreP2 == -1) {
                noHacer = true;
                queHacer();
                return false;
            }


            //calculo ganador
            var dif = pr.scoreP1 - pr.scoreP2;

            var eq1 = self.buscaXid(pr.idEquipoPr1, posiciones);
            eq1.GD = eq1.GD + dif;
            eq1.goles = eq1.goles + parseInt(pr.scoreP1);
            if (dif == 0) { eq1.puntos += 1 } else if (dif > 0) { eq1.puntos += 3 }
            if (pr.idEquipoClasifica == 1) { eq1.gana = 1;}

            var eq2 = self.buscaXid(pr.idEquipoPr2, posiciones);
            eq2.GD = eq2.GD + (dif * -1);
            eq2.goles = eq2.goles + parseInt(pr.scoreP2);
            if (dif == 0) { eq2.puntos += 1 } else if (dif < 0) { eq2.puntos += 3 }
            if (pr.idEquipoClasifica == 2) { eq2.gana = 1; }
        }

        posiciones.sort(SortByPuntos);

        var ganadores = [];

        //BUsco a que partido va, par que se guarde.
        for (var i = 0; i < fases.length; i++) {
            if (fases[i].idGrupo== idGrupo) {
                for (var j = 0; j < posiciones.length; j++) {
                    if (fases[i].posicion==j+1) {
                        ganadores.push({ idPartido: fases[i].idPartido, numEquipo: fases[i].numeroEquipo, idEquipo:posiciones[j].id})
                    }
                }
            }


        }

        //Cruzar contra avancesFase y mandar a guardar prons al server



        if (ganadores.length > 0) {
            htclibjs.Ajax({
                url: "/MiPronostico/GuardaGrupo",
                data: JSON.stringify({ idcu: self.idCartillaUsuario, pg: ganadores }),
                success: function (r) {
                    console.log(r);
                    if (r.exitoso == true) {
                        if (r.codigo == 333) {
                            self.miCartilla.MisPronosticos = r.data;           
                            self.llenaGrupos();                            
                        }
                    }
                    queHacer();
                }
            });
        }
        else {
            queHacer();
        }

        return true;


    },

    buscaXid: function (id, arreglo) {
        for (var i = 0; i < arreglo.length; i++) {
            if (arreglo[i].id == id) {
                return arreglo[i];
            }
        }

        p = new posicion(id);        
        arreglo.push(p);
        return p;

    },
    

    sellar: function ( queHacer) {

        self = CartillaUsuario;
        var prsG = self.miCartilla.MisPronosticos

        for (var i = 0; i < prsG.length; i++) {
            var pr = prsG[i];

            //si falta llenar, no se puede sellar
            if (pr.scoreP1 == null || pr.scoreP1 == -1 || pr.scoreP2 == null || pr.scoreP2 == -1) {
                noHacer = true;
                myApp.alert('Complete todos los pronósticos para sellar', '¡Qué Golazo!');
                //alert("complete todos los pronósticos para sellar");
                return false;
            }
        }

        var texto = "<p style='text-align:justify;'>Una vez sellada, no podrá hacer cambios a sus pronósticos.</p><p style='text-align:justify;'>En caso de empate en puntos ganará quien haya sellado antes.</p>";
        myApp.confirm(texto, '¿Seguro de sellar la cartilla?',
            function () {
                myApp.showPreloader();
                htclibjs.Ajax({
                    url: "/MiPronostico/sellarCartilla",
                    data: JSON.stringify({ idcu: CartillaUsuario.idCartillaUsuario }),
                    success: function (r) {
                        console.log(r);
                        if (r.exitoso == true) {
                            queHacer();

                            try {
                                myApp.hidePreloader();
                            } catch (e) {
                            }


                        }
                    }
                });
            },
            function () {
                //nada
            }
        );

        


    }

}

function SortByPuntos(a, b) {
    
    if (a.puntos == b.puntos) {
        if (a.GD == b.GD) {
            if (a.gana == b.gana) {
                return ((a.goles > b.goles) ? -1 : ((a.goles < b.goles) ? 1 : 0));
            } else {
                return ((a.gana > b.gana) ? -1 : ((a.gana < b.gana) ? 1 : 0));
            }            
        } else {
            return ((a.GD > b.GD) ? -1 : ((a.GD < b.GD) ? 1 : 0));
        }        
    }
    else {
        return ((a.puntos > b.puntos) ? -1 : ((a.puntos < b.puntos) ? 1 : 0));
    }
    
}