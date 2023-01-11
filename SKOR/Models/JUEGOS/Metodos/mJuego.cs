using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Skor.Models.JUEGOS.Metodos
{
	public class mJuego
	{
		System.Data.SqlClient.SqlConnection conn = new dbConnection().EntidadSql();
		//ApuestasEntities conex = new ApuestasEntities();
		public List<Clases.cJuego> JuegosPublicadosLista( )
		{
			List<Clases.cJuego> l = new List<Clases.cJuego>();
			try
			{

				using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("apuestas.a2022_Apuestas_JuegosPublicados_Lista", conn))
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
							var i = new Clases.cJuego()
							{
								IdJuego = int.Parse(item["IdJuego"].ToString()),
								idJuegoEstado = int.Parse(item["idJuegoEstado"].ToString()),
								Juego = item["Juego"].ToString(),
								Codigo = item["JuegoCodigo"].ToString(),
								JuegoDescripcion= item["JuegoDescripcion"].ToString(),
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
								PATROCINADORES = int.Parse(item["NumPatrocinadores"].ToString()) > 0 ? new Metodos.mPatrocinador().PatrocinadorLista_xIdJuego(idjuego: int.Parse(item["IdJuego"].ToString())) : new List<Clases.cPatrocinador>(),  //traer todos los patricinadores de este juegos por el id del juego

								//objeto del estado del juego
								ESTADO = new Clases.cJuegoEstado() { 
									IdJuegoEstado = int.Parse(item["idJuegoEstado"].ToString()),
									JuegoEstado = item["JuegoEstado"].ToString(),
									
									FC = DateTime.Parse(item["FC"].ToString()),
									FA = DateTime.Parse(item["FA"].ToString()),
									Eliminado = bool.Parse(item["Eliminado"].ToString()),
									idU = int.Parse(item["idU"].ToString()),

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

		public  Clases.cJuego  JuegoLista_xId(int idJuego)
		{
			 Clases.cJuego  l = new Clases.cJuego ();
			try
			{

				using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("apuestas.a2022_Apuestas_Juego_Lista_xId", conn))
				{
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IdJuego", idJuego));
					System.Data.DataTable dt = new System.Data.DataTable();
					System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
					conn.Open();
					da.Fill(dt);
					if (int.Parse(dt.Rows.Count.ToString()) > 0)
					{
						System.Data.DataRow item = dt.Rows[0];
						//i.encryptIds(); //encriptar los ids y llenar los ids en cero
						//l = i;
						//foreach (System.Data.DataRow item in dt.Rows)
						//{
							var i = new Clases.cJuego()
							{
								IdJuego = int.Parse(item["IdJuego"].ToString()),
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
								PATROCINADORES = int.Parse(item["NumPatrocinadores"].ToString()) > 0 ? new Metodos.mPatrocinador().PatrocinadorLista_xIdJuego(idjuego: int.Parse(item["IdJuego"].ToString())) : new List<Clases.cPatrocinador>(),  //traer todos los patricinadores de este juegos por el id del juego

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
							};
						//l.Add(i);
						l = i;
						//}

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