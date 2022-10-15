using Skor.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Skor.Controllers
{
    public class MiPronosticoController : Controller
    {
        // GET: MiPronostico
        public ActionResult Index()
        {
            vUsuarios.usuario elUser;
            List<CartillasUsuario> cartillas, cartillasCerradas, cartillasAbiertas;
            
            try {
                elUser = vUsuarios.web.TraeUsuarioRegistrado();
                using (var baseSk = new Models.skorEntities()) {
                    cartillas = (from camp in baseSk.CartillasUsuario.Include("Cartillas") where camp.idUsuario == elUser.id select camp).ToList();
                    cartillasCerradas = cartillas.Where(camp => camp.Cartillas.estaCerrada == true).OrderByDescending(c => c.id).ToList();
                    cartillasAbiertas = cartillas.Where(camp => camp.Cartillas.estaCerrada == false).OrderByDescending(c => c.id).ToList();
                }
                ViewBag.cartillasAbiertas = cartillasAbiertas;
                ViewBag.cartillasCerradas = cartillasCerradas;
            }
            catch (Exception e) {
                ViewBag.error = e.Message;
            }

            return View();
        }


        public JsonResult GuardaPronostico(int idp, int Sp1, int Sp2, int numCl)
        {
            vUsuarios.usuario elUser;
            Pronosticos pronostico;
            Resultado res;
            try
            {
                elUser = vUsuarios.web.TraeUsuarioRegistrado();
                if (elUser != null)
                {
                    using (var baseSk = new Models.skorEntities())
                    {
                        //&& p.idUsuario == elUser.id
                        pronostico = (from p in baseSk.Pronosticos where p.id == idp  select p).FirstOrDefault();
                        if (pronostico != null)
                        {
                            pronostico.scoreP1 = Sp1;
                            pronostico.scoreP2 = Sp2;
                            pronostico.idEquipoClasifica = numCl;

                            baseSk.SaveChanges();
                            res = new Resultado(true, "ok");
                        }
                        else {
                            res = new Resultado(false, "No existe pronóstico al actualizar");
                        }
                    }
                    
                }
                else {
                    throw new Exception("No existe usuario GuardaPronostico");
                }

                
            }
            catch (Exception e)
            {
                res = new Resultado(false, e.Message);
            }

            return Json(res);


        }


        public JsonResult sellarCartilla(int idcu)
        {


            vUsuarios.usuario elUser;
            CartillasUsuario cu;
            Resultado res;
            try
            {
                elUser = vUsuarios.web.TraeUsuarioRegistrado();
                if (elUser != null)
                {
                    using (var baseSk = new Models.skorEntities())
                    {
                        //&& p.idUsuario == elUser.id
                        cu = (from p in baseSk.CartillasUsuario where p.id == idcu && p.idUsuario == elUser.id select p).FirstOrDefault();
                        if (cu != null)
                        {

                            //todo: cu.sellar
                            cu.estaSellada = true;
                            cu.fechaSellada = DateTime.Today;
                            // cu.posicionCierre = ... 
                            baseSk.SaveChanges();


                            SqlParameter parameter1 = new SqlParameter("@idCartillaUsuario", idcu);
                            SqlParameter parameter2 = new SqlParameter("@idCartilla", cu.idCartilla);
                            baseSk.Database.ExecuteSqlCommand("exec SP_PROC_SetPosicionCierre @idCartillaUsuario, @idCartilla", parameter1, parameter2);                            

                            
                            res = new Resultado(true, "ok");
                        }
                        else
                        {
                            res = new Resultado(false, "No existe cartilla");
                        }
                    }

                }
                else
                {
                    throw new Exception("No existe usuario GuardaPronostico");
                }


            }
            catch (Exception e)
            {
                res = new Resultado(false, e.Message);
            }

            return Json(res);

        }

    public JsonResult GuardaGrupo(int idcu, List<ResultadoGrupo> pg)

        {
            vUsuarios.usuario  usuarioActual;
            Resultado ret;
            MiCartillaUsuario cu;
            bool hubo;

            try
            {
                usuarioActual = vUsuarios.web.TraeUsuarioRegistrado();
                //todo: verificar que el usuario registrado es el dueño

                using (var baseSk = new Models.skorEntities())
                {                    
                    hubo = MiCartillaUsuario.guardarGrupos(idcu,pg);

                    if (hubo)
                    {
                        cu = new MiCartillaUsuario(idcu);
                        ret=  new Resultado(true, cu.MisPronosticos, 333);
                    }
                    else
                    {
                        ret= new Resultado(true, 0);
                    }

                }

            }
            catch (Exception e)
            {
                ret = new Resultado(false, e.Message);
            }

            return Json(ret);


        }


    }
}