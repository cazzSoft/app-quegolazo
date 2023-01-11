/// <reference path="../Login.js" />

$(document).ready(function () {
    var idCartilla = $("#txtIdCartilla").val();
    var idJuego = $("#txtIdJuego").val();
    pgInformacionCartilla.inicializar(idCartilla, idJuego);
});

var pgInformacionCartilla = {

    idCartilla: 0,

    inicializar: function (idCar_,idJue_) {
        //click del 
        this.idCartilla = idCar_;
        this.idJuego = idJue_;
        pgInformacionCartilla.binds();       

    },


    binds: function () {

        $("#btnParticipar").bind("click", function (e) {
            e.preventDefault();
            pgInformacionCartilla.participar();
        });

    },

    participar: function () {

        if (Usuario.id() == 0) {
            myApp.alert('Debes ingresar a tu cuenta o crear una cuenta para participar! ', '¡Qué Golazo!');
            //alert("Debes registrarte para participar!  ACA SE VA A ABRIR DIRECTO EL LOGIN, por ahora hace login en el icono arriba derecha ");
            myApp.popup('.popup-login');            
        }
        else {
            htclibjs.Ajax({
                url: "/Cartilla/Participar",
                data: JSON.stringify({ idcartilla: pgInformacionCartilla.idCartilla, idjuego: pgInformacionCartilla.idJuego }),
                success: function (r) {
                    console.log(r);
                    if (r.exitoso == true) {
                        location.href = SITEROOT + "/Pronostico/Index/" + r.data;
                    }
                }
            });

        }

        

    }
    




}