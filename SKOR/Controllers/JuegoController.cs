using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Skor.Models;

namespace Skor.Controllers
{
	public class JuegoController : Controller
	{
		public ActionResult Carti()
		{
			List<Cartillas> cartillas, cartillasCerradas, cartillasAbiertas;
			try
			{
				
				using (var baseSk = new Models.skorEntities())
				{
					cartillas = (from camp in baseSk.Cartillas where camp.estaTerminada == false && camp.estaActiva == true select camp).ToList();
					cartillasAbiertas = cartillas.Where(camp => camp.estaCerrada == false).OrderByDescending(c => c.id).ToList();
					
				}

				ViewBag.cartillasAbiertas = cartillasAbiertas;
	

			}
			catch (Exception e)
			{
				ViewBag.error = e.Message;
			}
			return View();
		}

		// GET: Juego
		public ActionResult Index()
		{
			return View();
		}
		
		public ActionResult MisCartillas(int idjuego=0) {
            try {
				//var form = Request.Form;
				int IDPERSONA = 0;
				int IDJUEGO = 0;

				var USUARIO = vUsuarios.web.TraeUsuarioRegistrado();
				if (USUARIO == null || idjuego == 0)
				{
					return RedirectToAction("Index", "Inicio");
				}
				if (string.IsNullOrEmpty(Convert.ToString(USUARIO.idPersona)))
				{
					return RedirectToAction("Index", "Inicio");
				}

				IDPERSONA = Convert.ToInt32(USUARIO.idPersona);
				IDJUEGO = idjuego;

				var CARTILLAS = new Models.JUEGOS.Metodos.mCartilla().CartillaLista_xIdPersona_xIdJuego(idPersona: IDPERSONA, idJuego: IDJUEGO);
				ViewBag.CARTILLAS = CARTILLAS;
			}
			catch (Exception e)
            {
				ViewBag.error = e.Message;
			}
			
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
					code = 204;
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

		[HttpPost]
		public JsonResult Juegos_ValidarJuegoCodigo()
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
			string CODIGO = "";
			var item = new Models.JUEGOS.Clases.cJuego();
			try
			{
				var USUARIO = vUsuarios.web.TraeUsuarioRegistrado();
				if (USUARIO == null || form.Get("idjuego") == null || form.Get("codigo") == null)
				{
					code = 204;
					data = item;
				}
				else if ( string.IsNullOrEmpty(form.Get("idjuego"))  )
				{
					code = 204;
					data = item;
				}
				else
				{

					IDPERSONA = Convert.ToInt32(USUARIO.idPersona); //int.Parse( form.Get("idpersona").ToString() );
					IDJUEGO = int.Parse(form.Get("idjuego").ToString());
					IDUSUARIO = Convert.ToInt32(USUARIO.id); //int.Parse( form.Get("idusuario").ToString() );
					CODIGO =  form.Get("codigo").ToString() ; //VALOR PAGADO
					var juego = new Models.JUEGOS.Metodos.mJuego().JuegoLista_xId(idJuego: IDJUEGO);
					if (juego.Codigo == CODIGO)
					{
						meta = new Models.JUEGOS.Clases.cResultado() {
							idregistro = juego.IdJuego,
							resultado = "SI",
							resultado_detalle = "Código valido."
						};
					}
					else {
						meta = new Models.JUEGOS.Clases.cResultado()
						{
							idregistro = 0,
							resultado = "NO",
							resultado_detalle = "Código invalido."
						};
					}
					//new Models.JUEGOS.Metodos.mJuegoPagos().JuegoPagoValidarPago(idPersona: IDPERSONA, idJuego: IDJUEGO);
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


	}
}