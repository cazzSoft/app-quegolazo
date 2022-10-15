using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Skor.Controllers.Util;
using Skor.Models;
using System.Configuration;

namespace Skor.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult ValidarUsuario(string nameUser, string clave)
        {
            Resultado rs;

            try
            {

                htcLib.espacio.inicializa(ConfigurationManager.ConnectionStrings["Database"].ConnectionString, htcLib.tiposConexion.Web);
                
                vUsuarios.usuario usr = new vUsuarios.usuario();
                vUsuarios.web.validaYRegistraenBrowser(ref usr, nameUser, clave, false);

                if (usr.activo == false)
                {
                    rs = new Resultado(true, 0, 30);
                }
                else if (usr.debeCambiarClave == true)
                {
                    rs = new Resultado(true, usr.id, 40);
                }
                else if (usr.estadoPWD == vUsuarios.estadosClave.Caducado)
                {
                    rs = new Resultado(true, usr.id, 50);
                }
                else if (usr.activo)
                {
                    rs = new Resultado(true, usr.id, 20);
                }
                else //error
                {
                    rs = new Resultado(true, 0, 30);
                }
            }
            catch (Exception e)
            {
                rs = new Resultado(false, e.Message, -1);
            }
            finally {
                htcLib.espacio.cerrar();
            }
            

            return Json(rs);
        }

        public JsonResult cambiaPWD(string ua, string nu) {
            vUsuarios.usuario usr;
            Resultado rs = null;
            try {
                htcLib.espacio.inicializa(ConfigurationManager.ConnectionStrings["Database"].ConnectionString, htcLib.tiposConexion.Web);
                 usr = vUsuarios.web.TraeUsuarioRegistrado();
                if (usr.cambiaClave(nu, nu, ua))
                {
                    rs = new Resultado(true, usr.nivel, 20);
                }
                else {
                    rs = new Resultado(true, "Clave no válida", 30);
                }
            }
            catch (Exception e)
            {
                rs = new Resultado(false,e.Message, 30);
            }
            finally {
                htcLib.espacio.cerrar();
            }

            return Json(rs);
        }



        public JsonResult logOut()
        {
            Resultado res;
            try
            {
                vUsuarios.web.signOut();
                res = new Resultado(true,true);
            }
            catch (Exception e)
            {
                res = new Resultado(false, e.Message);
            }

            return Json(res);
        }



        public JsonResult registrarUsuario(UsuarioPersona usuario)
        {
            Resultado retorno;
            Correos.RegistroUsuario correo;
            string mensaje = "";
            long idAsigando = 0;
            try
            {
                htcLib.espacio.inicializa(ConfigurationManager.ConnectionStrings["Database"].ConnectionString, htcLib.tiposConexion.Web);

                if (usuario.guardar(ref mensaje, ref idAsigando))
                {
                    correo = new Correos.RegistroUsuario(usuario);
                    correo.EnviarCorreo();

                    vUsuarios.usuario usr = default(vUsuarios.usuario);
                    usr = new vUsuarios.usuario();
                    bool recuerda = false;
                    vUsuarios.web.validaYRegistraenBrowser(ref usr, usuario.email, usuario.clave, recuerda);

                    retorno = new Resultado(true, usr.id);
                }
                else
                {
                    retorno = new Resultado(false, mensaje);
                }



            }
            catch (Exception e)
            {
                retorno = new Resultado(false, e.Message);
            }
            finally
            {
                htcLib.espacio.cerrar();
            }

            return Json(retorno);
        }

        public JsonResult recuperarUsuario(string correo) {
            string clave;
            vUsuarios.usuario user;
            Correos.RecuperaClave correoRecupera;
            Resultado elResultado;

            try {
                htcLib.espacio.inicializa(ConfigurationManager.ConnectionStrings["Database"].ConnectionString, htcLib.tiposConexion.Web);

                user = new vUsuarios.usuario(correo);
                if (user.id != 0)
                {
                    clave = vUsuarios.usuario.AsignaClaveTemporal(user.id);

                    //amdamos email con la nueva clave y formato 1 golazo
                    correoRecupera = new Correos.RecuperaClave(user, correo, clave);
                    correoRecupera.EnviarCorreo();

                    elResultado = new Resultado(true, true);
                }
                else {
                    elResultado = new Resultado(false, "Correo no encontrado");
                }
                
            }
            catch (Exception e) {
                elResultado = new Resultado(false, e.Message);
            }
            finally{
                htcLib.espacio.cerrar();
            }
            

            return Json(elResultado);
        }



        public ActionResult B()
        {
            string codigoBando = Convert.ToString(Url.RequestContext.RouteData.Values["id"] ?? "");

            ViewBag.codigoBando = codigoBando;
            return View("B");
        }
    }
}