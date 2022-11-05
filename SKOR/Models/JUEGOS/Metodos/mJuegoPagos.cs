using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Skor.Models.JUEGOS.Metodos
{
	public class mJuegoPagos
	{
		System.Data.SqlClient.SqlConnection conn = new dbConnection().EntidadSql();
		//ApuestasEntities conex = new ApuestasEntities();
		//COSSULTA TODOS LOS PAGOS DE UNA PERSONA
		public List<Clases.cJuegoPagos> JuegoPagosLista()
		{
			List<Clases.cJuegoPagos> l = new List<Clases.cJuegoPagos>();
			try
			{

				using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("apuestas.a2022_Apuestas_JuegoPagos_Lista", conn))
				{
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					//cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IdPersona", idPersona));
					System.Data.DataTable dt = new System.Data.DataTable();
					System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
					conn.Open();
					da.Fill(dt);
					if (int.Parse(dt.Rows.Count.ToString()) > 0)
					{
						//System.Data.DataRow item = dt.Rows[0];
						//i.encryptIds(); //encriptar los ids y llenar los ids en cero
						//l = i;
						foreach (System.Data.DataRow item in dt.Rows)
						{
							var i = new Clases.cJuegoPagos()
							{
								//IdJuego = int.Parse(item["IdJuego"].ToString()),
								IdJuegoPagos = int.Parse(item["IdJuegoPagos"].ToString()),
								idPersona = int.Parse(item["idPersona"].ToString()),
								idUsuario = int.Parse(item["idUsuario"].ToString()),
								idJuego = int.Parse(item["idJuego"].ToString()),
								Valor = decimal.Parse(item["Valor"].ToString()),
								Estado = bool.Parse(item["Estado"].ToString()),
								FC = DateTime.Parse(item["FC"].ToString()),
								FA = DateTime.Parse(item["FA"].ToString()),
								Eliminado = bool.Parse(item["Eliminado"].ToString()),
								idU = int.Parse(item["idU"].ToString()),
								JUEGO = new Clases.cJuego()
								{
									IdJuego = int.Parse(item["idJuego"].ToString()),
									idJuegoEstado = int.Parse(item["idJuegoEstado"].ToString()),
									Juego = item["Juego"].ToString(),
									Codigo = item["JuegoCodigo"].ToString(),
									ImgNombre = item["JuegoImgNombre"].ToString(),
									ImgRuta = item["JuegoImgRuta"].ToString(),
									ImgExt = item["JuegoImgExt"].ToString(),
									FechaInicio = DateTime.Parse(item["JuegoFechaInicio"].ToString()),
									FechaFin = DateTime.Parse(item["JuegoFechaFin"].ToString()),
									Publicado = bool.Parse(item["JuegoPublicado"].ToString()),
									FC = DateTime.Parse(item["FC"].ToString()),
									FA = DateTime.Parse(item["FA"].ToString()),
									Eliminado = bool.Parse(item["Eliminado"].ToString()),
									idU = int.Parse(item["idU"].ToString()),
									NumPatrocinadores = int.Parse(item["NumPatrocinadores"].ToString()),
									Bloqueo = bool.Parse(item["Bloqueo"].ToString()),
									PATROCINADORES = int.Parse(item["NumPatrocinadores"].ToString()) > 0 ? new Metodos.mPatrocinador().PatrocinadorLista_xIdJuego(idjuego: int.Parse(item["idJuego"].ToString())) : new List<Clases.cPatrocinador>(),  //traer todos los patricinadores de este juegos por el id del juego

									//objeto del estado del juego
									ESTADO = new Clases.cJuegoEstado()
									{
										IdJuegoEstado = int.Parse(item["idJuegoEstado"].ToString()),
										JuegoEstado = item["JuegoEstado"].ToString(),

										FC = DateTime.Parse(item["FC"].ToString()),
										FA = DateTime.Parse(item["FA"].ToString()),
										Eliminado = bool.Parse(item["Eliminado"].ToString()),
										idU = int.Parse(item["idU"].ToString()),

									}
								}
								,PERSONA = new Clases.cPersona() { 
									IdPersona = int.Parse(item["idPersona"].ToString()),
									Nombre1 =  item["Nombre1"].ToString() ,
									Nombre2 =  item["Nombre2"].ToString() ,
									Apellido1 =  item["Apellido1"].ToString() ,
									Apellido2 = item["Apellido2"].ToString() ,
									Identificacion =  item["Identificacion"].ToString() ,
								}
							};
							l.Add(i);

						}

					}

					conn.Close();
					return l;
				}



			}
			catch (Exception ex)
			{
				conn.Close();
				return l;
			}
			finally
			{
				conn.Close();
			}
		}

		//busqueda de todos los jueogos pagados de una persona
		public List<Clases.cJuegoPagos> JuegoPagosLista_xIdPersona(int idPersona)
		{
			List<Clases.cJuegoPagos> l = new List<Clases.cJuegoPagos>();
			try
			{

				using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("apuestas.a2022_Apuestas_JuegoPagos_Lista_xIdPersona", conn))
				{
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IdPersona", idPersona));
					System.Data.DataTable dt = new System.Data.DataTable();
					System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
					conn.Open();
					da.Fill(dt);
					if (int.Parse(dt.Rows.Count.ToString()) > 0)
					{
						//System.Data.DataRow item = dt.Rows[0];
						//i.encryptIds(); //encriptar los ids y llenar los ids en cero
						//l = i;
						foreach (System.Data.DataRow item in dt.Rows)
						{
							var i = new Clases.cJuegoPagos()
							{
								//IdJuego = int.Parse(item["IdJuego"].ToString()),
								IdJuegoPagos = int.Parse(item["IdJuegoPagos"].ToString()),
								idPersona = int.Parse(item["idPersona"].ToString()),
								idUsuario = int.Parse(item["idUsuario"].ToString()),
								idJuego = int.Parse(item["idJuego"].ToString()),
								Valor = decimal.Parse(item["Valor"].ToString()),
								Estado = bool.Parse(item["Estado"].ToString()),
								FC = DateTime.Parse(item["FC"].ToString()),
								FA = DateTime.Parse(item["FA"].ToString()),
								Eliminado = bool.Parse(item["Eliminado"].ToString()),
								idU = int.Parse(item["idU"].ToString()),
								JUEGO = new Clases.cJuego()
								{
									IdJuego = int.Parse(item["idJuego"].ToString()),
									idJuegoEstado = int.Parse(item["idJuegoEstado"].ToString()),
									Juego = item["Juego"].ToString(),
									Codigo = item["JuegoCodigo"].ToString(),
									ImgNombre = item["JuegoImgNombre"].ToString(),
									ImgRuta = item["JuegoImgRuta"].ToString(),
									ImgExt = item["JuegoImgExt"].ToString(),
									FechaInicio = DateTime.Parse(item["JuegoFechaInicio"].ToString()),
									FechaFin = DateTime.Parse(item["JuegoFechaFin"].ToString()),
									Publicado = bool.Parse(item["JuegoPublicado"].ToString()),
									FC = DateTime.Parse(item["FC"].ToString()),
									FA = DateTime.Parse(item["FA"].ToString()),
									Eliminado = bool.Parse(item["Eliminado"].ToString()),
									idU = int.Parse(item["idU"].ToString()),
									NumPatrocinadores = int.Parse(item["NumPatrocinadores"].ToString()),
									Bloqueo = bool.Parse(item["Bloqueo"].ToString()),
									PATROCINADORES = int.Parse(item["NumPatrocinadores"].ToString()) > 0 ? new Metodos.mPatrocinador().PatrocinadorLista_xIdJuego(idjuego: int.Parse(item["idJuego"].ToString())) : new List<Clases.cPatrocinador>(),  //traer todos los patricinadores de este juegos por el id del juego

									//objeto del estado del juego
									ESTADO = new Clases.cJuegoEstado()
									{
										IdJuegoEstado = int.Parse(item["idJuegoEstado"].ToString()),
										JuegoEstado = item["JuegoEstado"].ToString(),

										FC = DateTime.Parse(item["FC"].ToString()),
										FA = DateTime.Parse(item["FA"].ToString()),
										Eliminado = bool.Parse(item["Eliminado"].ToString()),
										idU = int.Parse(item["idU"].ToString()),

									}
								}
								,
								PERSONA = new Clases.cPersona()
								{
									IdPersona = int.Parse(item["idPersona"].ToString()),
									Nombre1 = item["Nombre1"].ToString(),
									Nombre2 = item["Nombre2"].ToString(),
									Apellido1 = item["Apellido1"].ToString(),
									Apellido2 = item["Apellido2"].ToString(),
									Identificacion = item["Identificacion"].ToString(),
								}
							};
							l.Add(i);

						}

					}

					conn.Close();
					return l;
				}



			}
			catch (Exception ex)
			{
				conn.Close();
				return l;
			}
			finally
			{
				conn.Close();
			}
		}

		//VALIDAR JUEGO PAGADO

		public Clases.cResultado JuegoPagoValidarPago(int idPersona,int idJuego)
		{
			Clases.cResultado l = new Clases.cResultado()
			{
				idregistro = 0,
				resultado = "NO",
				resultado_detalle = "ERROR",
			};
			try
			{

				using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("apuestas.a2022_Apuestas_JuegoPagos_ValidarJuego", conn))
				{
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					//PARAMETROS DE INGRESO
					cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IdPersona", idPersona));
					cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IdJuego", idJuego));

					System.Data.DataTable dt = new System.Data.DataTable();
					System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
					conn.Open();
					da.Fill(dt);
					if (int.Parse(dt.Rows.Count.ToString()) > 0)
					{
						System.Data.DataRow item = dt.Rows[0];
						l = new Clases.cResultado()
						{
							idregistro = int.Parse(item["idregistro"].ToString()),
							resultado = item["resultado"].ToString(),
							resultado_detalle = item["resultado_detalle"].ToString(),
						};

					}

					conn.Close();
					return l;
				}



			}
			catch (Exception ex)
			{
				conn.Close();
				return l;
			}
			finally
			{
				conn.Close();
			}
		}


		//INGRESO DE PAGO
		public Clases.cResultado JuegoPagoIngreso(Clases.cJuegoPagos _item)
		{
			Clases.cResultado l = new Clases.cResultado()
			{
				idregistro = 0,
				resultado = "NO",
				resultado_detalle = "ERROR",
			};
			try
			{

				using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("apuestas.a2022_Apuestas_JuegoPagos_Ingreso", conn))
				{
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					//PARAMETROS DE INGRESO
					cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IdPersona", _item.idPersona));
					cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IdUsuario", _item.idUsuario));
					cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IdJuego", _item.idJuego));
					cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Valor", _item.Valor));
					cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IdU", _item.idU));

					System.Data.DataTable dt = new System.Data.DataTable();
					System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
					conn.Open();
					da.Fill(dt);
					if (int.Parse(dt.Rows.Count.ToString()) > 0)
					{
						System.Data.DataRow item = dt.Rows[0];
						l = new Clases.cResultado()
						{
							idregistro = int.Parse(item["idregistro"].ToString()),
							resultado = item["resultado"].ToString(),
							resultado_detalle = item["resultado_detalle"].ToString(),
						};

					}

					conn.Close();
					return l;
				}



			}
			catch (Exception ex)
			{
				conn.Close();
				return l;
			}
			finally
			{
				conn.Close();
			}
		}

	}
}