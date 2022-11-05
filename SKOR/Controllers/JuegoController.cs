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

		//public ActionResult CartillasDelJuego 
		#region Juegos_Pagados
		[HttpPost]
		public JsonResult JuegosPagos_Ingreso()
		{ //nombre de la ruta

			var meta = new Models.JUEGOS.Clases.cResultado()
			{
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
				var USUARIO = vUsuarios.web.TraeUsuarioRegistrado();
				if (USUARIO == null || form.Get("idjuego") == null || form.Get("valor") == null)
				{

				}
				else
				{

					IDPERSONA = Convert.ToInt32(USUARIO.idPersona); //int.Parse( form.Get("idpersona").ToString() );
					IDJUEGO = int.Parse(form.Get("idjuego").ToString());
					IDUSUARIO = Convert.ToInt32(USUARIO.id); //int.Parse( form.Get("idusuario").ToString() );
					VALOR = decimal.Parse(form.Get("valor").ToString()); //VALOR PAGADO

					item = new Models.JUEGOS.Clases.cJuegoPagos()
					{
						idPersona = IDPERSONA,
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

			return Json(data: new { data = data, code = code, meta = meta }  /*new JsonRequestBehavior( )*/  );
			//return Json(data: new { data = 2 , code = code , meta = new { idregistro = idregistro, resultado = resultado, resultado_detalle = resultado_detalle } }  , new JsonRequestBehavior( )  );
		}

		[HttpPost]
		public JsonResult JuegosPagos_ValidarJuego()
		{ //nombre de la ruta

			var meta = new Models.JUEGOS.Clases.cResultado()
			{
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
				var USUARIO = vUsuarios.web.TraeUsuarioRegistrado();
				if (USUARIO == null || form.Get("idjuego") == null || form.Get("valor") == null)
				{

				}
				else
				{

					IDPERSONA = Convert.ToInt32(USUARIO.idPersona); //int.Parse( form.Get("idpersona").ToString() );
					IDJUEGO = int.Parse(form.Get("idjuego").ToString());
					IDUSUARIO = Convert.ToInt32(USUARIO.id); //int.Parse( form.Get("idusuario").ToString() );
					VALOR = decimal.Parse(form.Get("valor").ToString()); //VALOR PAGADO


					meta = new Models.JUEGOS.Metodos.mJuegoPagos().JuegoPagoValidarPago(idPersona: IDPERSONA, idJuego: IDJUEGO);
					data = item;
					code = 200;
				}


			}
			catch (Exception ex)
			{
				meta.resultado_detalle = ex.Message;
			}

			return Json(data: new { data = data, code = code, meta = meta }  /*new JsonRequestBehavior( )*/  );
			//return Json(data: new { data = 2 , code = code , meta = new { idregistro = idregistro, resultado = resultado, resultado_detalle = resultado_detalle } }  , new JsonRequestBehavior( )  );
		}

		[HttpPost]
		public JsonResult JuegosPagadosLista_xIdPersona()
		{

			var meta = new Models.JUEGOS.Clases.cResultado()
			{
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
			 
			var item = new Models.JUEGOS.Clases.cJuegoPagos();
			try
			{
				var USUARIO = vUsuarios.web.TraeUsuarioRegistrado();
				if (USUARIO == null || form.Get("idjuego") == null || form.Get("valor") == null)
				{
					code = 204;
				}
				else
				{
					IDPERSONA = Convert.ToInt32(USUARIO.idPersona); //int.Parse( form.Get("idpersona").ToString() );
					IDJUEGO = int.Parse(form.Get("idjuego").ToString());
					IDUSUARIO = Convert.ToInt32(USUARIO.id); //int.Parse( form.Get("idusuario").ToString() );
					
					 meta = new Models.JUEGOS.Clases.cResultado()
					{
						idregistro = 0,
						resultado = "SI",
						resultado_detalle = "Completado",
					};
					data = new Models.JUEGOS.Metodos.mJuegoPagos().JuegoPagosLista_xIdPersona(idPersona: IDPERSONA);
					code = 200;
				}


			}
			catch (Exception ex)
			{
				meta.resultado_detalle = ex.Message;
			}

			return Json(data: new { data = data, code = code, meta = meta }  /*new JsonRequestBehavior( )*/  );
			//return Json(data: new { data = 2 , code = code , meta = new { idregistro = idregistro, resultado = resultado, resultado_detalle = resultado_detalle } }  , new JsonRequestBehavior( )  );
		}


		#endregion

	}
}