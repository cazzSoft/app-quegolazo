using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Skor.Models.JUEGOS.Metodos
{
	public class mCartilla
	{
		System.Data.SqlClient.SqlConnection conn = new dbConnection().EntidadSql();
		//ApuestasEntities conex = new ApuestasEntities();
		public List<Clases.cCartilla> CartillaLista_xIdPersona_xIdJuego(int idPersona, int idJuego)
		{
			List<Clases.cCartilla> l = new List<Clases.cCartilla>();
			try
			{

				using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("apuestas.a2022_Apuestas_Cartillas_xIdPersona_xIdJuego", conn))
				{
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IdPersona", idPersona));
					cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IdJuego", idJuego));
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
							var i = new Clases.cCartilla()
							{
								Activa = bool.Parse(item["CartillaEstaActiva"].ToString()),
								Cerrada = bool.Parse(item["CartillaEstaCerrada"].ToString()),
								Terminada = bool.Parse(item["CartillaEstaTerminada"].ToString()),
								Condiciones =  item["CartillaCondiciones"].ToString() ,
								Descripcion =  item["CartillaDescripcion"].ToString(),
								FechaCierre =  DateTime.Parse( item["CartillaFechaCierre"].ToString()),
								idAdministrador = int.Parse(item["idAdministrador"].ToString()),
								IdCartilla = int.Parse(item["idCartilla"].ToString()),
								idEmpresa = int.Parse(item["idEmpresa"].ToString()),
								Nombre = item["CartillaNombre"].ToString(),
								Premios = item["CartillaPremios"].ToString(),
								URLBanner = item["CarillaURLBanner"].ToString(),

                                JUEGOCARTILLA = new Clases.cJuegoCartilla()
                                {
                                    idCartilla = int.Parse(item["idCartilla"].ToString()),
                                    idJuego = int.Parse(item["idJuego"].ToString()),
                                    IdJuegoCartilla = int.Parse(item["IdJuegoCartilla"].ToString()),

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
                                    }
                                }
                            }
							;
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
	}
}