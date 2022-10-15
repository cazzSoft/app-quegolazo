/// <reference path="../Login.js" />

$(document).ready(function () {
    var idCartilla = $("#txtIdCartilla").val();
    pgInformacionCartilla.inicializar(idCartilla);
});

var pgInformacionCartilla = {

    idCartilla: 0,

    inicializar: function (idCar_) {
        //click del 
        this.idCartilla = idCar_;
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
                data: JSON.stringify({ idcartilla: pgInformacionCartilla.idCartilla }),
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