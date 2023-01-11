using System;
using System.Web.Mvc;

namespace Skor.Controllers
{
    public class InformacionCartillaController : Controller
    {
        // GET: Cartilla
        public ActionResult Index(int id = 0, int idj = 0)
        {
            int idCartilla;
            int idJuego;

            try
            {
                idCartilla = Convert.ToInt32(Url.RequestContext.RouteData.Values["id"] ?? "0");
                idJuego = idj;
                ViewBag.cartilla = Util.General.getCartilla(idCartilla); 
                ViewBag.idJuego = idJuego;
            }
            catch (Exception e)
            {
                ViewBag.error = e.Message;
            }

            return View();
        }
    }
}