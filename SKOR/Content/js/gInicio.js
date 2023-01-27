
var url = window.location.protocol + '//' + window.location.host;
$(document).ready(function () {
    
    simplyCountdown('#cuenta', {
        year: 2023, // required
        month: 11, // required
        day: 20, // required
        hours: 0, // Default is 0 [0-23] integer
        minutes: 0, // Default is 0 [0-59] integer
        seconds: 0, // Default is 0 [0-59] integer
        words: { //words displayed into the countdown
            days: 'Día',
            hours: 'Hora',
            minutes: 'Minuto',
            seconds: 'Segundo',
            pluralLetter: 's'
        },
        plural: true, //use plurals
        inline: false, //set to true to get an inline basic countdown like : 24 days, 4 hours, 2 minutes, 5 seconds
        inlineClass: 'simply-countdown-inline', //inline css span class in case of inline = true
        // in case of inline set to false
        enableUtc: true, //Use UTC as default
        onEnd: function () {
            document.getElementById('portada').classList.add('oculta');
            $('.class_').remove();
            return;
        }, //Callback on countdown end, put your own function here
        refresh: 1000, // default refresh every 1s
        sectionClass: 'simply-section', //section css class
        amountClass: 'simply-amount', // amount css class
        wordClass: 'simply-word', // word css class
        zeroPad: false,
        countUp: false
    });

});



//funcion para kuegos publicos
function juego_public(id) {
    validate_juego(id, '.popup-page');
}

//funcion para juegos privado
function juego_private(id) {
    validate_juego(id, '.popup-code');
}


//validar si el juego ha sido pagado
function validate_juego(id,Pop) {
    var FrmData = {
        valor: '4',
        idjuego: id
    };

    $.ajax({
        url: "/Juego/JuegosPagos_ValidarJuego",
        method: "POST",
        async: false,
        data: FrmData,
        dataType: "json",
        success: function (data) {

            if (data.code == 200) {
               
                if (data.meta.resultado == 'SI') {//el juego ya esta pagado
                    //redireccionar
                    location.href = url + `/Juego/MisCartillas/?idjuego=${id}`;
                    //location.href = url + `/Juego/Carti`;
                } else if (data.meta.resultado == 'NO') {
                    boton_pagos(id);
                    myApp.popup(`${Pop}`);
                    clientTransactionId_ = id;
                    if (Pop == '.popup-code') {
                        //seteamos valores del juego 
                        $('#id_juego').val(id);
                    }
                }
            }
        },

        error: function (data) {
            console.log(data);
        },
    });
}


//metodo de pago
function boton_pagos(idj) {
    var clientTransactionId_ = idj+11;
    $('#pp-button').html(" ");
    payphone.Button({

        //token obtenido desde la consola de developer
        token: "dNIXKR-p2CSC-LduEtb-apgojyYEnpwsABABlJqaD1UxVfrW7Te3VZcTwBuSCcWSfnj7Dpu2pccUEg1Q1r-OAWvp4rDjtdSKxhtWGYA-FbK98FVNCprXiyYUBGqDAosQ7PCOhwvVvEmXmDchNT9bVgDtzQ1hdZqZ0vUmqhrf3SwcnAHrEuX40OQavpgREOjAqNQA_3VDVF-tC2JMDbfRC7CdKs_ZcVbkUnH35f6wSSlkIG_hICpANDTFWFjtrt-GCXX98Q6mctqmmft9NsWgGA4_nejjeFXnaBlhjGbW9B71yF2S6b5uKdtleT3OzqXwzBDkDAmUR1fiQs7a0_0boHmHj2Y",

        //PARÁMETROS DE CONFIGURACIÓN
        btnHorizontal: false,
        btnCard: true,
        context: "",

        createOrder: function (actions) {

            //Se ingresan los datos de la transaccion ej. monto, impuestos, etc
            return actions.prepare({

                amount: 400,
                amountWithoutTax: 400,
                currency: "USD",
                clientTransactionId: clientTransactionId_// @vUsuarios.web.TraeUsuarioRegistrado().idPersona
            });

        },
        onComplete: function (model, actions) {

            //Se confirma el pago realizado
            actions.confirm({
                id: model.id,
                clientTxId: model.clientTxId
            }).then(function (value) {

                //EN ESTA SECCIÓN SE RECIBE LA RESPUESTA Y SE MUESTRA AL USUARIO

                if (value.transactionStatus == "Approved") {
                   /* alert("Pago " + value.transactionId + " recibido, estado " + value.transactionStatus);*/
                    comfirmar_pago(value.transactionId, clientTransactionId_-11);
                }
            }).catch(function (err) {
                console.log(err);
            });

        }
    }).render("#pp-button");
}


//confirm pago
function comfirmar_pago(transactionId,idj) {
    var FrmData = {
        idjuego: idj,
        clientTransactionId: transactionId,
    };

    $.ajax({
        url: "/Juego/JuegosPagos_Ingreso",
        method: "POST",
        async: false,
        data: FrmData,
        dataType: "json",
        success: function (data) {

            if (data.code == 200) {

                if (data.meta.resultado == 'SI') {//ingresado
                    //redireccionar
                    location.href = url + `/Juego/MisCartillas/?idjuego=${idj}`;
                } else if (data.meta.resultado == 'NO') {
                    alert('Intente nuevamente. Algo salio mal..');
                }
            }
        },

        error: function (data) {
            console.log(data);
        },
    });
}
//validar codigo de juegos privados 
function validate_code() {
    var FrmData = {
        idjuego: $('#id_juego').val(),
        codigo: $('#code_juego').val(),
    };

    $.ajax({
        url: "/Juego/Juegos_ValidarJuegoCodigo",
        method: "POST",
        async: false,
        data: FrmData,
        dataType: "json",
        success: function (data) {

            if (data.code == 200) {
               
                if (data.meta.resultado == 'SI') {//el codigo si coinside con el juego
                    //redireccionar
                    //location.href = url + `/Juego/MisCartillas/?idjuego=${id}`;
                    myApp.popup(`.popup-page`);
                    $('.info').addClass('d-none');
                    clientTransactionId_ = $('#id_juego').val();
                } else if (data.meta.resultado == 'NO') {
                    $('.info').html('Código invalido');
                    $('.info').removeClass('d-none');
                }
            }
        },

        error: function (data) {
            console.log(data);
        },
    });
}