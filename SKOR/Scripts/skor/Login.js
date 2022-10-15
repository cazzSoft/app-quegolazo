$(document).ready(function () {
    login.inicializar();
});

var login = {

    queHacerValidado: function () {
        pagina
        if (typeof pagina !== 'undefined' && pagina == 'Inicio') {
            location.reload();
        }
    },  //funcion a la cual llamar una vez validado el usuario

    inicializar: function () {
        $("#btnLogin").click(function () {
            Usuario.validar();
        });

        $("#btnRegistrar").click(function () {
            Usuario.registrar();
        });

        $("#hrfSalir").click(function () {
            Usuario.logout();
        });

        $("#btnRecobrarClave").click(function () {
            Usuario.recuperarClave();
        });

        $('#chkSeePassoword').click(function () {
            $(this).is(':checked') ? $('#txtClaveUsuario').showPassword() : $('#txtClaveUsuario').hidePassword();
        });

        $('#chkSeePasswordRegistro').click(function () {
            $(this).is(':checked') ? $('#txtClaveRegistro').showPassword() : $('#txtClaveRegistro').hidePassword();
        });

        $('#chkSeePasswordCambioClave').click(function () {
            $(this).is(':checked') ? $('#txtClaveActual').showPassword() : $('#txtClaveActual').hidePassword();
            $(this).is(':checked') ? $('#txtClaveNueva').showPassword() : $('#txtClaveNueva').hidePassword();
            $(this).is(':checked') ? $('#txtClaveNueva2').showPassword() : $('#txtClaveNueva2').hidePassword();
        });

        

        $('#btnCambiarClave').click(function () {
            Usuario.cambiaPwd();
        });
        
    },


};

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

var Usuario = {

    id: function () {
        var elId = $("#txtIdUsuario").val();
        return parseInt(elId);
    },
    //no usado
    setUsuario: function (idUser) {
        localStorage.setItem("idUsuario", idUser);
    },
    validar: function () {
        var nu = $('#txtNombreUsuario').val();
        var pwd = $('#txtClaveUsuario').val();
        $('#msgLogin').html('');
        myApp.showPreloader();
        htclibjs.Ajax({
            url: "/Login/ValidarUsuario",
            data: JSON.stringify({ nameUser: nu, clave: pwd }),
            success: function (r) {
                if (r.codigo == 20) { Usuario.usuarioValido(r.data) }
                else if (r.codigo == 30) { Usuario.usuarioNOValido() }
                else if (r.codigo == 40) { Usuario.cambiarPwd() }
                else if (r.codigo == 50) { Usuario.usuarioExpirado() }
                myApp.hidePreloader();
            }
        });

    },

    registrar: function () {
        var nombre_ = $("#txtNombreRegistro").val();
        var apellido_ = $("#txtApellidoRegistro").val();
        var correo_ = $("#txtCorreoRegistro").val();
        var clave_ = $("#txtClaveRegistro").val();
        var telefono_ = $("#txtTelefonoRegistro").val();


        if ($.trim(nombre_) == "" || $.trim(apellido_) == "" || $.trim(correo_) == "" || $.trim(clave_) == "") {
            myApp.alert('Ingrese los campos requeridos','Registro');
            return;
        }
        //if ($.trim(telefono_) != "" && telefono_.length != 10) {
        //    myApp.alert('Ingresa un número de teléfono de 10 dígitos.','Registro');
        //    return;
        //}

        var persona = { nombre: nombre_, apellido: apellido_, email: correo_, clave: clave_, telefono : telefono_ };

        myApp.showPreloader();

        htclibjs.Ajax({
            url: "/Login/registrarUsuario",
            data: JSON.stringify({ usuario: persona }),
            success: function (r) {
                if (r.exitoso == true) {
                    myApp.alert('Tu cuenta fue creada con éxito!', '¡Qué Golazo!');
                    //alert('Tu cuenta fue creada con éxito!');
                    myApp.closeModal('.popup-signup');
                    Usuario.usuarioValido(r.data);
                }
                myApp.hidePreloader();
            }
        });
    },

    usuarioValido: function (id) {
        $("#txtIdUsuario").val(id);//para que este logueado en JS
        $('#msgLogin').html('Usuario correcto');
        $("#imgLogoUser").attr("src", SITEROOT + "/images/skor/user_login.png");
        myApp.closeModal('.popup-login');


        //ejecuto lo que la pagina en que estoy me pida..
        try {
            login.queHacerValidado();
        } catch (e) {

        }

    },

    usuarioNOValido: function () {
        $('#msgLogin').html('Usuario no válido');
    },

    cambiarPwd: function () {
        myApp.popup('.popup-claveNueva');
    },

    usuarioExpirado: function () {
        /*$('#spnMensajeLogin').html('Usuario expirado');
        $('#divMensajeLogin').removeClass();
        $('#divMensajeLogin').addClass('alert alert-warning');
        $('#divMensajeLogin').show();*/
        $('#msgLogin').html('El usuario tiene la clave expirada');
    },

    cambioNOValido: function (razon) {
        alert('cambioNOValido');
        myApp.alert(razon, 'Ingreso');
        $('#pMessageNewClave').html('Cambio no válido '+ razon);
    },

    cambiaPwd: function () {

        var actual = $("#txtClaveActual").val();
        var nueva = $("#txtClaveNueva").val();        
        var nueva2 = $("#txtClaveNueva2").val();        

        if (nueva == nueva2) {
            myApp.showPreloader();
            htclibjs.Ajax({
                url: "/Login/cambiaPWD",
                data: JSON.stringify({ ua: actual, nu: nueva }),
                success: function (r) {
                    myApp.hidePreloader();
                    if (r.codigo == 20) {
                        Usuario.usuarioValido();
                        myApp.closeModal('.popup');
                        $('.popup-overlay').removeClass('modal-overlay-visible');
                    }
                    else if (r.codigo == 30) { Usuario.cambioNOValido(r.data); }
                }
            });
        } else {
            $('#pMessageNewClave').html('Repetición de clave incorrecta');
        }
        

    },

    recuperarClave: function () {
        var email = $("#txtCorreoRecobrar").val();
        if ($.trim(email) == "") {
            myApp.alert('Ingrese un correo', 'Recuperar clave');
            return;
        }
        myApp.showPreloader();
        htclibjs.Ajax({
            url: "/Login/recuperarUsuario",
            data: JSON.stringify({ correo:email }),
            success: function (r) {
                myApp.hidePreloader();
                myApp.closeModal('.popup');
                $('.popup-overlay').removeClass('modal-overlay-visible');
                myApp.alert('Se ha enviado un correo con la clave de recuperación.', 'Recuperar clave');
            }
        });
    },

    logout: function () {
        myApp.showPreloader();
        htclibjs.Ajax({
            url: "/Login/logOut",
            data: null,
            success: function (r) {
                myApp.hidePreloader();
                $("#txtIdUsuario").val("0");
                location.href = SITEROOT + "/";
            }
        });
    }
    


}



