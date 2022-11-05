
var url = window.location.protocol + '//' + window.location.host;
$(document).ready(function () {
    
    var countDDate = new Date(),
    endday = new Date(), days = 17;
    countDDate.setDate(countDDate.getDate() - (countDDate.getDay() + 2) % 7);
    endday.setDate(countDDate.getDate() + days);
    endday.getTime();
    countDiv = document.getElementById('countdown');


    var x = setInterval(function () {
        var now = new Date().getTime();
        var daterest = endday - now;
        var days = Math.floor(daterest / (1000 * 60 * 60 * 24)),
            hours = Math.floor((daterest % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60)),
            minutes = Math.floor((daterest % (1000 * 60 * 60)) / (1000 * 60)),
            seconds = Math.floor((daterest % (1000 * 60)) / 1000);
        if (days == 0) {
            $('.class_').remove();
        } else {
            countDiv.innerHTML = days + " diás " + hours + ":" + minutes + ":" + seconds + " ";
        }
        
    }, 1000);

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
                    console.log(url);
                    location.href = url+"/Index/Cartilla/85";
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