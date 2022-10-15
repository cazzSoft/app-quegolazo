using Skor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Skor.Controllers
{
    public class PosicionesController : Controller
    {
        // GET: Posiciones
        public ActionResult Index()
        {
            int idCartilla;
            vUsuarios.usuario elUser;
            SP_RevisaCartilla_Result revisaCartillaUsuario;
            
            try {
                idCartilla = Convert.ToInt32(Url.RequestContext.RouteData.Values["id"] ?? "0");
                elUser = vUsuarios.web.TraeUsuarioRegistrado();
                using (var baseSk = new Models.skorEntities()) {
                    ViewBag.filasPosiciones = getFilasPosiciones(baseSk, idCartilla, (int)elUser.id, 40);
                    revisaCartillaUsuario = baseSk.SP_RevisaCartilla(idCartilla, (int)elUser.id).FirstOrDefault();
                    ViewBag.bandos = baseSk.SP_BandosUsuario((int)elUser.id).ToList();
                }
                ViewBag.idCartillaUsuario = revisaCartillaUsuario.idCartillaUsuario;
                ViewBag.cartilla = Util.General.getCartilla(idCartilla);
                ViewBag.idCurrentUser = (int)elUser.id;
            }
            catch (Exception e) {
                ViewBag.error = e.Message;
            }
            
            
            return View();
        }


        public JsonResult getAllPosiciones(int idCartilla)
        {
            vUsuarios.usuario elUser;
            Resultado res;
            string filas;

            try
            {
                elUser = vUsuarios.web.TraeUsuarioRegistrado();
                using (var baseSk = new Models.skorEntities())
                {
                    filas = getFilasPosiciones(baseSk, idCartilla, (int)elUser.id, 40000);
                    res = new Resultado(true, filas);
                }
            }
            catch (Exception e)
            {
                res = new Resultado(false, e.Message);
            }


            return Json(res);
        }

        public JsonResult getPosicionesBandos(int idCartilla, int idBando)
        {
            vUsuarios.usuario elUser;
            Resultado res;
            string filas;

            try
            {
                elUser = vUsuarios.web.TraeUsuarioRegistrado();
                using (var baseSk = new Models.skorEntities())
                {
                    if (idBando == 0)
                    {
                        filas = getFilasPosiciones(baseSk, idCartilla, (int)elUser.id, 40);
                    }
                    else {
                        filas = getFilasPosicionesBandos(baseSk, idCartilla, (int)elUser.id, idBando);
                    }
                    
                    res = new Resultado(true, filas);
                }
            }
            catch (Exception e)
            {
                res = new Resultado(false, e.Message);
            }

            return Json(res);
        }

        public string getFilasPosiciones(Models.skorEntities baseSk,int idCartilla, int userId, int cuantos) {
            string fila, laFila ="";
            List<SP_PosicionesCartilla_Result> posiciones;
            int contador=0;

            posiciones = baseSk.SP_PosicionesCartilla(idCartilla, userId, cuantos).ToList();
            fila = Util.General.LeerPlantilla("Plantillas/Posicion.html");

            foreach (SP_PosicionesCartilla_Result resultado in posiciones) {
                contador += 1;
                laFila += fila.Replace("{{posicionFinal}}", Convert.ToString(resultado.posicionFinal)??"-")
                    .Replace("{{urlMiCartilla}}", Url.Action("Index", "MiCartilla", new { id = resultado.idCartillaUsuario }))
                    .Replace("{{nombreCompleto}}", resultado.nombreCompleto)
                    .Replace("{{puntos}}",Convert.ToString(resultado.puntos)??"-")
                    .Replace("{{ranking}}", Convert.ToString(resultado.ranking) ?? "-")
                    .Replace("{{claseElMismo}}", (resultado.idUsuario == userId ? "myPosition" : ""));
            }

            return laFila;
        }

        public string getFilasPosicionesBandos(Models.skorEntities baseSk, int idCartilla, int userId, int idBando)
        {
            string fila, laFila = "";
            List<SP_PosicionesCartillaBando_Result> posiciones;

            posiciones = baseSk.SP_PosicionesCartillaBando(idCartilla, userId, idBando).ToList();
            fila = Util.General.LeerPlantilla("Plantillas/Posicion.html");

            foreach (SP_PosicionesCartillaBando_Result resultado in posiciones)
            {
                laFila += fila.Replace("{{posicionFinal}}", Convert.ToString(resultado.posicionFinal) ?? "-")
                    .Replace("{{urlMiCartilla}}", Url.Action("Index", "MiCartilla", new { id = resultado.idCartillaUsuario }))
                    .Replace("{{nombreCompleto}}", resultado.nombreCompleto)
                    .Replace("{{puntos}}", Convert.ToString(resultado.puntos) ?? "-")
                    .Replace("{{ranking}}", Convert.ToString(resultado.ranking) ?? "-")
                    .Replace("{{claseElMismo}}", (resultado.idUsuario == userId ? "myPosition" : ""));
            }

            return laFila;
        }


        #region PosicionesGenerales
        //Vista
        public ActionResult Generales()
        {
            vUsuarios.usuario elUser;

            try
            {
                elUser = vUsuarios.web.TraeUsuarioRegistrado();
                using (var baseSk = new skorEntities()) {
                    ViewBag.bandos = baseSk.SP_BandosUsuario((int)elUser.id).ToList();
                }
                    
            }
            catch (Exception e)
            {
                ViewBag.error = e.Message;
            }

            return View("Generales");
        }


        //Json para traer las posiciones
        public JsonResult GetPosicionesGenerales(int idUsuario, int cantidadActual)
        {
            Resultado res;
            int cuanto;
            List<SP_PosicionesGenerales_Result> posiciones;
            try
            {
                cuanto = cantidadActual + 50;
                using (var baseSk = new skorEntities())
                {
                    posiciones = baseSk.SP_PosicionesGenerales(idUsuario, cuanto).ToList();
                }

                res = new Resultado(true, posiciones);
            }
            catch (Exception e)
            {
                res = new Resultado(false, e.Message);
            }
            return Json(res);
        }

        public JsonResult GetPosicionesGeneralesBando(int idUsuario,  int idBando, int cantidadActual)
        {
            Resultado res;
            int cuanto;
            List<SP_PosicionesGeneralesBando_Result> posiciones;
            try
            {
                cuanto = cantidadActual + 50;
                using (var baseSk = new skorEntities())
                {
                    posiciones = baseSk.SP_PosicionesGeneralesBando(idUsuario, cuanto, idBando).ToList();
                }

                res = new Resultado(true, posiciones);
            }
            catch (Exception e)
            {
                res = new Resultado(false, e.Message);
            }
            return Json(res);
        }


        public JsonResult GetPosicionesGeneralesGrupos(int cantidadActual) {
            int cuanto;
            List<SP_PosicionesBandos_Result> posiciones;
            Resultado res;

            try {
                cuanto = cantidadActual + 50;
                using (var baseSk = new skorEntities())
                {
                    posiciones = baseSk.SP_PosicionesBandos(cuanto).ToList();
                }
                res = new Resultado(true,posiciones);
            }
            catch (Exception e ) {
                res = new Resultado(false, e.Message);
            }
            
                
            return Json(res);
        }
        #endregion

    }
}