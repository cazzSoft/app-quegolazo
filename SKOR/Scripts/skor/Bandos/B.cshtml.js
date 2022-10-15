$(document).ready(function () {
    loginB.inicializar();
});

var loginB = {
    
    cod:"",

    inicializar: function () {
        loginB.cod = "QGL"; //tomar del url
        login.queHacerValidado = loginB.enviar;
        loginB.cod = $("#txtCodigoBando").val();

        //$("#btnLoginB").click(function () {
        //    login.queHacerValidado = loginB.enviar;
        //    myApp.popup('.popup-login'); 
        //}


        
       
    },

    enviar: function () {

        //alert("hola");
        var h = SITEROOT + "/bando/unirse/" + loginB.cod;
        location.href = h;

    }


};

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

