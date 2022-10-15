using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Skor.Models;

namespace Skor.Controllers
{
    public class BandoController : Controller
    {
        // GET: Bando
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MisBandos()
        {
            vUsuarios.usuario usuarioActual;
            List<UsuariosBandos> bandosUsuario;
            string nombreBando = "";
            //Controlando que este logueado
            try
            {
                usuarioActual = vUsuarios.web.TraeUsuarioRegistrado();
                if (usuarioActual.id == 0) //No se ha logueado
                    return RedirectToAction("Index", "Inicio");

                nombreBando = Convert.ToString(Url.RequestContext.RouteData.Values["id"] ?? "");
                using (var baseSk = new skorEntities())
                {
                    bandosUsuario = (from ub in baseSk.UsuariosBandos.Include("Bandos") where ub.idUsuario == usuarioActual.id select ub).ToList();
                }
                ViewBag.bandosUsuario = bandosUsuario;
                ViewBag.idUsuario = usuarioActual.id;
                ViewBag.nombreBando = nombreBando;
            }
            catch (Exception e)
            {
                ViewBag.error = e.Message;
            }

            return View("MisBandos");
        }

        public ActionResult Ver()
        {
            Bandos elBando;
            UsuariosBandos uBandos;
            vUsuarios.usuario usuarioActual;
            try
            {
                int idBando = Convert.ToInt32(Url.RequestContext.RouteData.Values["id"] ?? "");
                usuarioActual = vUsuarios.web.TraeUsuarioRegistrado();
                //quiero recibir otro parametro n=1
                //y  pasar esa n a la vista
                int segundo = Convert.ToInt32(Url.RequestContext.RouteData.Values["n"] ?? 0);//se debe pasar un &n=1

                using (var baseSk = new Models.skorEntities())
                {
                    elBando = (from b in baseSk.Bandos.Include("USR_usuarios").Include("USR_usuarios.MIS_Personas") where b.id == idBando select b).FirstOrDefault();
                    uBandos = (from bn in baseSk.UsuariosBandos where bn.idUsuario == usuarioActual.id && bn.idBando == idBando select bn).FirstOrDefault();
                }
                ViewBag.nuevo = segundo;
                ViewBag.bando = elBando;
                ViewBag.usuarioBando = uBandos;
            }
            catch (Exception e)
            {
                ViewBag.error = e.Message;
            }

            return View("Ver");

        }

        public JsonResult GetMisMiembros(int idBando)
        {
            Resultado res;
            List<Object> users;
            try
            {
                List<UsuariosBandos> usuarios;


                using (var baseSk = new Models.skorEntities())
                {
                    users = new List<Object>();
                    usuarios = (from b in baseSk.UsuariosBandos.Include("USR_usuarios").Include("USR_usuarios.MIS_Personas") where b.idBando == idBando select b).OrderBy(ub => ub.USR_usuarios.ranking).Take(50).ToList();

                    foreach (var user in usuarios)
                    {
                        users.Add(new { nombre = user.USR_usuarios.MIS_Personas.nombre1, apellido = user.USR_usuarios.MIS_Personas.apellido1 });
                    }
                    res = new Resultado(true, users);
                }

            }
            catch (Exception e)
            {
                res = new Resultado(false, e.Message);
            }
            return Json(res);
        }

        public ActionResult Unirse()
        {

            int idu;
            vUsuarios.usuario usuarioActual;
            UsuariosBandos userBando;
            Bandos bando;

            string codBando = Convert.ToString(Url.RequestContext.RouteData.Values["id"] ?? "A");

            try
            {
                usuarioActual = vUsuarios.web.TraeUsuarioRegistrado();

                idu = Convert.ToInt32(usuarioActual.id);

                if (idu > 0)
                {
                    using (var baseSk = new Models.skorEntities())
                    {
                        //si es usuario, lo uno al grupo

                        bando = (from ba in baseSk.Bandos where ba.codigo == codBando select ba).FirstOrDefault();

                        if (bando != null)
                        {
                            //todo: Pasar a objeto una vez probado para usar con jsoncontrolller
                            //bando.unirse(idu)
                            UsuariosBandos bu = new UsuariosBandos();
                            bu.idBando = bando.id;
                            bu.idUsuario = idu;
                            userBando = (from ub in baseSk.UsuariosBandos where ub.idBando == bando.id && ub.idUsuario == idu select ub).FirstOrDefault();
                            if (bando.esAbierto == true)  //todo: reemplazar con esAceptado
                            {
                                bu.estaAceptado = true;
                            }

                            //controlar que ya no exista un registro con ese idUsuario + idBando
                            if (userBando == null)
                            {
                                baseSk.UsuariosBandos.Add(bu);
                                baseSk.SaveChanges();
                            }


                        }
                        else
                        {
                            return RedirectToAction("Index", "Inicio");
                        }

                    } //using

                    return RedirectToAction("ver", "Bando", new { id = bando.id, n = 1 });

                } //usr reg

                else
                {
                    return RedirectToAction("B", "Login", new { id = codBando });
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Inicio");
            }


        }

        public JsonResult UnirseJ(string COD)
        {
            int idu;
            vUsuarios.usuario usuarioActual;
            //todo: verificar que el usuario registrado es el dueño
            Resultado ret;

            try
            {
                usuarioActual = vUsuarios.web.TraeUsuarioRegistrado();
                idu = Convert.ToInt32(usuarioActual.id);

                Bandos bando;

                if (idu > 0)
                {
                    using (var baseSk = new Models.skorEntities())
                    {


                        bando = (from ba in baseSk.Bandos where ba.codigo == COD select ba).FirstOrDefault();
                        //bando.unirse(idu)
                    }
                    ret = new Resultado(true, "unido");

                }
                else
                {
                    ret = new Resultado(false, "usuario?");
                }

            }
            catch (Exception e)
            {
                ret = new Resultado(false, e.Message);
            }

            return Json(ret);


        }

        public JsonResult crearNuevo(string nombre)
        {
            Resultado ret;

            int idu;
            vUsuarios.usuario usuarioActual;
            UsuariosBandos userBando;
            Bandos bando;

            try
            {
                usuarioActual = vUsuarios.web.TraeUsuarioRegistrado();

                idu = Convert.ToInt32(usuarioActual.id);

                if (idu > 0)
                {
                    using (var baseSk = new Models.skorEntities())
                    {

                        bando = new Bandos();

                        bando.nombre = nombre;
                        bando.esAbierto = true;
                        bando.idAdmin = idu;
                        bando.idTipo = 1;  //todo : definir tipos  
                        bando.codigo = "";
                        bando.condiciones = "";
                        bando.descripcion = "";
                        bando.posicion = 0;
                        bando.premios = "";
                        bando.puntos = 0;                        

                        baseSk.Bandos.Add(bando);

                        baseSk.SaveChanges();

                        //ya con el ID genero un código unico
                        bando.codigo = bandoEquipo.GeneraCodigo(bando.nombre, bando.id );
                        //todo: pasar todo esto al objeto bando
                        baseSk.SaveChanges();


                        //Lo uno al grupo al usuario

                        UsuariosBandos bu = new UsuariosBandos();
                        bu.idBando = bando.id;
                        bu.idUsuario = idu;
                        bu.estaAceptado = true;

                        baseSk.UsuariosBandos.Add(bu);
                        baseSk.SaveChanges();

                    } //using

                    ret = new Resultado(true, bando.id);

                } //hay usuario

                else
                {
                    ret = new Resultado(false, "Usuario no registrado", 101);
                }

            }
            catch (Exception e)
            {
                ret = new Resultado(false, e.Message);
            }

            return Json(ret);

        }

        public JsonResult SalirBando(int idB) {
            vUsuarios.usuario user;
            UsuariosBandos ubs;
            Resultado res;
            try
            {
                user = vUsuarios.web.TraeUsuarioRegistrado();
                if (user.id > 0)
                {
                    using (var baseSk = new skorEntities())
                    {
                        ubs = baseSk.UsuariosBandos.FirstOrDefault(ub => ub.idUsuario == user.id && ub.idBando == idB);
                        baseSk.Entry(ubs).State = System.Data.Entity.EntityState.Deleted;
                        baseSk.SaveChanges();
                    }
                    res = new Resultado(true, true);
                }
                else
                {
                    res = new Resultado(false, "No existe usuario logueado.");
                }

            }
            catch (Exception e) {
                res = new Resultado(false,e.Message);
            }
            return Json(res);
        }

    }

    

}
