using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SkorAdmin.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            List<Models.Partidos> losPartidos;
            try {
                if (Models.Util.General.getUsuario() != null)
                {
                    //traemos partidos y mostramos
                    using (var baseSk = new SkorAdmin.Models.skorEntities())
                    {
                        losPartidos = (from pt in baseSk.Partidos.Include("Equipos").Include("Equipos1") where (pt.score1 == null || pt.score2 == null) && pt.fecha <= DateTime.Today select pt).ToList();
                    }
                    ViewBag.Partidos = losPartidos;
                }
            }
            catch (Exception e) {
                ViewBag.error = e.Message;
            }
            
            
            return View();
        }

        public JsonResult CalificarPartido(int idPartido, int sc1, int sc2) {
            Models.Util.Resultado res;
            Models.Partidos par;
            try {
                using (var baseSk = new SkorAdmin.Models.skorEntities()) {
                    par = baseSk.Partidos.SingleOrDefault(p => p.id==idPartido);
                    par.score1 = sc1;
                    par.score2 = sc2;
                    baseSk.SaveChanges();

                    baseSk.SP_PROC_CalificaPorPartido(idPartido);
                }
                res = new Models.Util.Resultado(true, true);
            }
            catch (Exception e) {
                res = new Models.Util.Resultado(false,e.Message);
            }

            return Json(res);
        }

        public JsonResult RecalificarTodo() {
            Models.Util.Resultado res;
            try
            {
                using (var baseSk = new SkorAdmin.Models.skorEntities())
                {
                    baseSk.SP_PROC_RecalculaCartillas();
                }
                res = new Models.Util.Resultado(true, true);
            }
            catch (Exception e)
            {
                res = new Models.Util.Resultado(false, e.Message);
            }

            return Json(res);
        }

    }
}