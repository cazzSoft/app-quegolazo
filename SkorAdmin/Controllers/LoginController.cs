using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SkorAdmin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {

            string email, clave;

            if (Request.Form.Count > 0)
            {
                email = Request.Form["txtUsuario"].Trim();
                clave = Request.Form["txtPassword"].Trim();


                    if (email == "iviteri" && clave == "iviteri")
                    {
                        Models.Util.General.setUsuario(new Models.Util.UsuarioSkor(email,clave));
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.error = "Usuario no encontrado";
                    }
                
            }
            
            return View();
        }
    }
}