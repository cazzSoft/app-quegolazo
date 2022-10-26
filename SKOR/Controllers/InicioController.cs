using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Skor.Models;

namespace Skor.Controllers
{
    public class InicioController : Controller
    {
        // GET: Inicio
        public ActionResult Index()
        {            
            List<Cartillas> cartillas, cartillasCerradas, cartillasAbiertas;
            List<SP_PosicionesGenerales_Result> posiciones;
            vUsuarios.usuario elUser;
            
            try
            {
                elUser = vUsuarios.web.TraeUsuarioRegistrado();
                using (var baseSk = new Models.skorEntities())
                {
                    cartillas = (from camp in baseSk.Cartillas where camp.estaTerminada == false && camp.estaActiva == true select camp).ToList();
                    cartillasCerradas = cartillas.Where(camp => camp.estaCerrada == true).OrderByDescending(c => c.id).ToList();
                    cartillasAbiertas = cartillas.Where(camp => camp.estaCerrada == false).OrderByDescending(c => c.id).ToList();
                    posiciones = baseSk.SP_PosicionesGenerales((int)elUser.id, 15).ToList();
                    if (elUser.id > 0) {//el usuario de vusuario no trae completo
                        ViewBag.usuario = (from usr in baseSk.USR_usuarios.Include("MIS_Personas").Include("Rankings") where usr.id == elUser.id select usr).FirstOrDefault();
                    }
                }

                ViewBag.cartillasAbiertas = cartillasAbiertas;
                ViewBag.cartillasCerradas = cartillasCerradas;
                ViewBag.posiciones = posiciones;

                var listaJuegos = new Models.JUEGOS.Metodos.mJuego().JuegosPublicadosLista();
                ViewBag.ListaJuegosPublicados = listaJuegos;

            }
            catch (Exception e)
            {
                ViewBag.error = e.Message;
            }

            return View();
        }
        [HttpPost]
        public JsonResult JuegosPublicados() { //nombre de la ruta
            var lista = new Models.JUEGOS.Metodos.mJuego().JuegosPublicadosLista();
            return Json(data: lista);
        }

        public ActionResult Ejemplo() {
            //validaciones
            return View();
        }

    }
}