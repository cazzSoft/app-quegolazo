using Skor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Skor.Controllers
{
    public class InformacionCartillaController : Controller
    {
        // GET: Cartilla
        public ActionResult Index()
        {
            int idCartilla;

            try {
                idCartilla = Convert.ToInt32(Url.RequestContext.RouteData.Values["id"] ?? "0");
                ViewBag.cartilla = Util.General.getCartilla(idCartilla);
            }
            catch (Exception e) {
                ViewBag.error = e.Message;
            }

            return View();
        }
    }
}