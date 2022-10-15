$(document).ready(function () {
    pgMisBandos.inicializar();
});

var pgMisBandos = {
    

    inicializar: function () {
        $("#btnCreaBando").click(function () {
            pgMisBandos.creaBando();            
        })

        $(".btnSalirGrupo").click(function () {
            var nombre = $(this).data("nombre");
            var id = $(this).data("bando");
            pgMisBandos.salirBando(id,nombre);
        })
        
    },

    creaBando: function () {
        var nombreB = $('#txtBandoNombre').val();      

        myApp.showPreloader();

        htclibjs.Ajax({
            url: "/Bando/crearNuevo",
            data: JSON.stringify({ nombre: nombreB }),
            success: function (r) {
                myApp.hidePreloader();
                var txt = "Equipo creado, invita a tus amigos";
                myApp.alert(txt, "¡Qué Golazo!", function () { pgMisBandos.redir(r.data); });
                
            }
        });
    },

    redir: function (id) {
        myApp.showPreloader();

        location.href = SITEROOT + "/Bando/ver/" + id; 
    },
       
    salirBando: function (idBando,nombre) {
        myApp.confirm("Seguro de salir de " + nombre + "?", "Confirmación", function () {
            myApp.showPreloader();
            htclibjs.Ajax({
                url: "/Bando/SalirBando",
                data: JSON.stringify({ idB: idBando }),
                success: function (r) {
                    myApp.hidePreloader();
                    myApp.alert("Ha salido del grupo " + nombre, "¡Qué Golazo!", function () {
                        location.href = SITEROOT; 
                    });
                }
            });
        },null);
        
    },   


};