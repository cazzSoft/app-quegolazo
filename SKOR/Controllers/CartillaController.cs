using Skor.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Skor.Controllers
{
    public class CartillaController : Controller
    {
        // GET: Cartilla
        public ActionResult Index(int id=0, int idj=0)
        {
            int idCartilla, idUsuario;
            vUsuarios.usuario elUser;
            SP_RevisaCartilla_Result cartillaResult;

            try {
                idCartilla = Convert.ToInt32(Url.RequestContext.RouteData.Values["id"] ?? "0");
            
                elUser = vUsuarios.web.TraeUsuarioRegistrado();
                if (elUser == null)
                {
                    idUsuario = 0;
                }
                else {
                    idUsuario = (int)elUser.id;
                }

                int idJuego = idj;
                using (var baseSk = new skorEntities()) {
                    cartillaResult = baseSk.SP_RevisaCartilla(idCartilla, idUsuario, idJuego).FirstOrDefault();
                }
                
                if (!(cartillaResult.estaCerrada ?? false))//estaAbierta
                {
                    if (cartillaResult.idCartillaUsuario == null)
                    {
                        //abierta y no estoy registrado
                        return RedirectToAction("Index", "InformacionCartilla", new { id = idCartilla ,idj= idj });
                        //return RedirectToAction("Index", "InformacionCartilla", new { id = idCartilla });

                    }
                    else {
                        //abierta y estoy jugando
                        if (cartillaResult.estaSellada == true)
                        {
                            //sellada, voy a pagina de ver
                            return RedirectToAction("Index", "MiCartilla", new { id = cartillaResult.idCartillaUsuario });

                        }
                        else
                        {
                            //a jugar
                            return RedirectToAction("Index", "Pronostico", new { id = cartillaResult.idCartillaUsuario });
                        }
                        
                    }
                }
                else {
                    //cerrada, voy a posiciones
                    return RedirectToAction("Index", "Posiciones", new { id = idCartilla });
                }

            }
            catch (Exception e) {
                ViewBag.error = e.Message;
            }
            
            return View();
        }





        // crear controlador cartillaUsuario?? si se desrganiza este, ok

        public JsonResult GetMiCartillaUsuario(int idcu)
        {
            int idu;
            vUsuarios.usuario usuarioActual;
            //todo: verificar que el usuario registrado es el dueño
            Resultado ret;
            MiCartillaUsuario cartilla;

            try {
                usuarioActual = vUsuarios.web.TraeUsuarioRegistrado();
                
                using (var baseSk = new Models.skorEntities()) {
                    cartilla = new MiCartillaUsuario(idcu);
                }
                ret = new Resultado(true,cartilla);
            }
            catch (Exception e) {
                ret = new Resultado(false,e.Message);
            }

            return Json(ret);


        }


        public JsonResult GetCartilla(int idCartilla)
        {
            int idu;
            vUsuarios.usuario usuarioActual;
            //todo: verificar que el usuario registrado es el dueño
            Resultado ret;
            Cartillas cartilla;

            try
            {
                usuarioActual = vUsuarios.web.TraeUsuarioRegistrado();

                using (var baseSk = new Models.skorEntities())
                {
                    cartilla = (from cr in baseSk.Cartillas where cr.id == idCartilla select cr).FirstOrDefault();
                }
                ret = new Resultado(true, cartilla);
            }
            catch (Exception e)
            {
                ret = new Resultado(false, e.Message);
            }

            return Json(ret);


        }


        public JsonResult Participar(int idCartilla, int idJuego)
        {
            vUsuarios.usuario usuarioActual;
            Resultado ret;
            CartillasUsuario cu;
            CartillasUsuario cuVigente;

            try
            {
                usuarioActual = vUsuarios.web.TraeUsuarioRegistrado();


                using (var baseSk = new Models.skorEntities())
                {
                    //busco si este usr ya tiene esa cartilla
                    cuVigente = (from cr in baseSk.CartillasUsuario where cr.idCartilla == idCartilla && cr.idUsuario == usuarioActual.id  select cr).FirstOrDefault();

                    //DESCOMENTAR ACA: sacando la cartilla de a CU?
                    //if (cuVigente.id <0 )   //.cartilla.estaCerrada)
                    //{
                    //    ret = new Resultado(true, "Esta cartilla está cerrada, ya no se puede participar.",101);
                    //    return Json(ret); // puedo hacer aca? o anidamso el if??
                    //}

                    if (cuVigente == null)  //si no existe la creo
                    {
                        //todo: Cartilla.Participar:
                        cu = new CartillasUsuario();
                        cu.idCartilla = idCartilla;
                        cu.idUsuario = Convert.ToInt32(usuarioActual.id);
                        cu.fechaCreacion = DateTime.Today;

                        baseSk.CartillasUsuario.Add(cu);
                        baseSk.SaveChanges();

                        /*SqlParameter parameter1 = new SqlParameter("@idCartillaUsuario", cu.id);
                        baseSk.Database.ExecuteSqlCommand("exec SP_PROC_CreaPronosticos @idCartillaUsuario", parameter1);*/
                        baseSk.SP_PROC_CreaPronosticos(cu.id);
                        var resultado =  new Models.JUEGOS.Metodos.mCartillasUsuario().CartillaUsuario_ActualizarIdJuego(cu.id, idJuego);
                        if (resultado.idregistro > 0)
                        {
                        }
                        else { 
                        
                        }

                        ret = new Resultado(true, cu.id);
                    }
                    else
                    {
                        ret = new Resultado(true, cuVigente.id);
                    }
                }
            }
            catch (Exception e)
            {
                ret = new Resultado(false, e.Message);
            }

            return Json(ret);


        }


        public ActionResult Buscar() {

            return View("Buscar");
        }

        public ActionResult Terminadas()
        {
            List<Cartillas> lasCartillas;

            try
            {
                using (var baseSk = new skorEntities())
                {
                    lasCartillas = (from cr in baseSk.Cartillas where cr.estaTerminada == true select cr).OrderByDescending(c => c.id).ToList();
                }
                ViewBag.cartillas = lasCartillas;
            }
            catch (Exception e)
            {
                ViewBag.error = e.Message;
            }

            return View("Terminadas");
        }
    }




}