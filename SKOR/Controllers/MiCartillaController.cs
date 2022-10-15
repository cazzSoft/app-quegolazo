using Skor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Skor.Controllers
{
    public class MiCartillaController : Controller
    {
        // GET: MiCartilla
        public ActionResult Index()
        {
            int idCartillaUsuario;
            CartillasUsuario cusr;
            List<SP_PronosticosCartillaUsuario_Result> pronosticos;
            try
            {

                idCartillaUsuario = Convert.ToInt32(Url.RequestContext.RouteData.Values["id"] ?? "0");

                using (var baseSk = new Models.skorEntities())
                {
                    cusr = (from cu in baseSk.CartillasUsuario where cu.id == idCartillaUsuario select cu).FirstOrDefault();
                    pronosticos = baseSk.SP_PronosticosCartillaUsuario(idCartillaUsuario).ToList();
                }
                ViewBag.cartilla = Util.General.getCartilla((int)cusr.idCartilla);
                ViewBag.cartillaUsuario = cusr;
                ViewBag.pronosticos = pronosticos;
                
            }
            catch (Exception e)
            {
                ViewBag.error = e.Message;
            }

            return View();
        }
    }
}