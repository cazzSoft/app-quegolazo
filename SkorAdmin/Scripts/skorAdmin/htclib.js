var htclibjs = {
    
    //MANDAR POR DEFAULT UN token?

    Ajax: function (args) {
        /// <summary>Función que ejecuta una petición AJAX.</summary> y maneja cosas standar
            $.ajax({
                type: args.type == undefined ? "POST" : args.type,
                url: SITEROOT + args.url,
                contentType: "application/json; charset=utf-8",
                data: args.data == undefined ? {} : args.data,
                dataType: "json",
                timeout: args.timeout == undefined ? 300000 : args.timeout,
                beforeSend: args.beforeSend == undefined ? function () { } : args.beforeSend(),
                async: args.async == undefined ? true : args.async,                
                success: function (r) { htclibjs.manejaSuccessAjax(args.success, r); },
                error: function (err) { htclibjs.manejaErrorAjax(args.error, err); }
            });
    },


    manejaSuccessAjax: function (manejador, r) {
        
        if (r.exitoso) {

            if (r.codigo == 101) {
                myApp.alert(r.data, "Qué Golazo!");
            } else {
                manejador(r);
            }

            
        } 
        else {
            console.log("Logueo de errores: " + r.data);
            console.log(r);
            myApp.hidePreloader();
            myApp.alert(r.data, "Qué Golazo!");
        }
    
    },

    manejaErrorAjax: function (manejador, err) {
        console.log("Logueo de errores: " + err);     
        console.log(err);

        var mostrar = true;

        if (err != null && "status" in err) {
            var especificacion = "";
            switch (err.status) {
                case 0:
                    if (err.statusText === "timeout") { especificacion = ""; }
                    mostrar = false;
                    break;
                case 404:
                    especificacion = "No encontrado";
                    break;
                case 500:
                    especificacion = "Problema de servidor";
                    break;

            }

            if (mostrar == true) {
                alert("Se ha detectado un error, por favor realice una captura de pantalla y envíela a sistemas.    Error al conectar con el servidor:" + especificacion + " " + err.status + " " + err.statusText);
            } else {

            }

                        
        } else {
            alert("Error no determinado al conectar con el servidor. Vuelva a intentar");
        }

        // el cliente quiere manejar el error
        if (manejador != undefined) { manejador(err); }

        //Si hay error se oculta loading por defecto,no manda error
        myApp.hidePreloader();
        

    },  

}

var Fecha = {

    getDate: function (data) {
        var fecha;

        if (this.dateIsEmpty(data)) {
            return '';
        } else {
            if (typeof data !== 'undefined') {
                fecha = new Date(parseInt(data.substr(6)));
                return this.getFecha(fecha);
            } else {
                return data;
            }
        }
    },

    getFecha: function (d) {//as Date
        var month = d.getMonth() + 1;
        var day = d.getDate();
        var output = d.getFullYear() + "/" + (month < 10 ? '0' : '') + month + '/' + (day < 10 ? '0' : '') + day;
        return output;
    },

    dateIsEmpty: function (data) {
        var cincuentayalgo = new Date(1950, 1, 2);
        var numeroFecha;
        var fecha;

        if (typeof data !== 'undefined') {
            numeroFecha = parseInt(data.substr(6));
            fecha = new Date(numeroFecha);

            if (fecha.getTime() < cincuentayalgo.getTime()) {
                return true;
            } else {
                return false;
            }
        } else {
            return true;
        }
    }

}
