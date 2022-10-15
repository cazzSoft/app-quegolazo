using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace SkorAdmin.Controllers
{
    public class GeneralController : Controller
    {
        // GET: General
        public ActionResult Index()
        {
            return View();
        }

        

        public ActionResult Close()
        {
            try {
                if (Models.Util.General.getUsuario() == null)
                {//solo controlo login
                    return RedirectToAction("Index", "Login");
                }
           }
            catch (Exception e) {
                ViewBag.error = e.Message;
            }

            return View("Close");
        }


        [EnableCors(origins: "*", headers: "*", methods: "post")]
        public JsonResult CerrarCartillas()
        {
            object res;

            try
            {
                using (var baseSk = new SkorAdmin.Models.skorEntities())
                {
                    baseSk.SP_CerrarCartillas();
                }
                res = new { exitoso = true, data = true };
            }
            catch (Exception e)
            {
                res = new { exitoso = false, data = e.Message };
            }

            return Json(res);
        }

    }
}