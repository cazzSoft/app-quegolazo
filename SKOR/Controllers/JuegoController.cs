using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Skor.Controllers
{
    public class JuegoController : Controller
    {
        // GET: Juego
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult JuegosPagos_Ingreso()
        { //nombre de la ruta

            var meta = new Models.JUEGOS.Clases.cResultado() {
                idregistro = 0,
                resultado = "NO",
                resultado_detalle = "Error",
            };
            int code = 500;
            var data = new object();

            var form = Request.Form;
            int IDPERSONA = 0;
            int IDJUEGO = 0;
            int IDUSUARIO = 0;
            decimal VALOR = 0;
            var item = new Models.JUEGOS.Clases.cJuegoPagos();
            try
			{

                if (form.Get("idpersona") == null || form.Get("idjuego") == null || form.Get("idusuario") == null || form.Get("valor") == null)
                {

                } else {

                    IDPERSONA = int.Parse( form.Get("idpersona").ToString() );
                    IDJUEGO = int.Parse( form.Get("idjuego").ToString() );
                    IDUSUARIO = int.Parse( form.Get("idusuario").ToString() );
                    VALOR = decimal.Parse( form.Get("valor").ToString() );

                    item = new Models.JUEGOS.Clases.cJuegoPagos()
                    {
                        idPersona =  IDPERSONA,
                        idJuego = IDJUEGO,
                        idUsuario = IDUSUARIO,
                        Valor = VALOR,
                    };
                    meta = new Models.JUEGOS.Metodos.mJuegoPagos().JuegoPagoIngreso(_item: item);
                    data = item;
                    code = 200;
                }  

                
			}
			catch (Exception ex)
			{
                meta.resultado_detalle = ex.Message;
			}

            return Json(data: new { data = data, code = code, meta = meta }  , new JsonRequestBehavior( )  );
            //return Json(data: new { data = 2 , code = code , meta = new { idregistro = idregistro, resultado = resultado, resultado_detalle = resultado_detalle } }  , new JsonRequestBehavior( )  );
        }
    }
}