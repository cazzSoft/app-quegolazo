using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Skor.Controllers
{
    public class PronosticoController : Controller
    {
        //Viene desde el js de informacionCartilla al dar click en participar
        public ActionResult Index()
        {
            int idCartillaUsuario;
            Models.CartillasUsuario cu;

            vUsuarios.usuario elUser;

            try
            {
                elUser = vUsuarios.web.TraeUsuarioRegistrado();
                idCartillaUsuario = Convert.ToInt32(Url.RequestContext.RouteData.Values["id"] ?? "0");

                using (var baseSk = new Models.skorEntities())
                {
                    cu = (from carU in baseSk.CartillasUsuario where carU.id == idCartillaUsuario select carU).FirstOrDefault();
                    //todo: traer 1 solo join? etapa 3
                }

                if (cu.estaSellada == true )
                {
                    //No debería estar acá, lo llevo a VER.
                    return RedirectToAction("Index", "MiCartilla", new { id = idCartillaUsuario });
                }
                if (cu.idUsuario != elUser.id)
                {
                    //trampeando o deslogueado, se va al home
                    return RedirectToAction("Index", "Inicio", new { id = idCartillaUsuario });
                }
                
                else
                {
                    ViewBag.cartilla = Util.General.getCartilla(cu.idCartilla??0);
                    ViewBag.idCartillaUsuario = idCartillaUsuario;

                    
                }

            }
            catch (Exception e) {
                ViewBag.error = e.Message;
            }

            return View();
        }
    }
}