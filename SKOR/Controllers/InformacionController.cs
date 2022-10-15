using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Skor.Controllers
{
    public class InformacionController : Controller
    {
        // GET: Informacion
        public ActionResult Patrocinios()
        {
            return View();
        }
    }
}