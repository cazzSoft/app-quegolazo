

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

function validate_juego(idj) {
   //validar si el juego ha sido cancelado
    console.log(idj);
    myApp.popup('.popup-code');
    
}