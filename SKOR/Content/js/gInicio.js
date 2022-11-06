
var url = window.location.protocol + '//' + window.location.host;
$(document).ready(function () {
    
    simplyCountdown('#cuenta', {
        year: 2022, // required
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
    //validate_code(idj, codigo)
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
                console.log(id);
                if (data.meta.resultado == 'SI') {//el juego ya esta pagado
                    //redireccionar
                    var idjuego = id;
                    console.log(url + "/Juego/MisCartillas/");
                    //alert(url + "/Juego/MisCartillas" )
                    //location.href = url + "/Juego/MisCartillas";
                   
                    location.href = url + `/Juego/MisCartillas/?idjuego=${id}`;
                    //location.href = url + `/MiCartilla/index/?id=${id}`;
                    //$.redirect('/Juego/MisCartillas',  { idjuego: id, });
               
                } else if (data.meta.resultado == 'NO') {
                    myApp.popup(`${Pop}`);
                }
            }
        },

        error: function (data) {
            console.log(data);
        },
    });
}


//validar codigo de juegos privados 
function validate_code(idj,codigo) {
    $.ajax({
        url: "/cita/pacienteUpdate",
        /*method: "POST",*/
        data: FrmData,
        dataType: "json",
        success: function (data) {

            if (data.length != 0) {
                if (data) {//el juego ya esta pagado
                    return 'true';
                } else {
                    return 'false';
                }
            }
        },

        error: function (data) {
            console.log(data);
        },
    });
}