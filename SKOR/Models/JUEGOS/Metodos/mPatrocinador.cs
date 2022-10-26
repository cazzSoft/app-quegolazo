using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Skor.Models.JUEGOS.Metodos
{
	public class mPatrocinador
	{
		System.Data.SqlClient.SqlConnection conn = new dbConnection().EntidadSql();
		ApuestasEntities conex = new ApuestasEntities();
		public List<Clases.cPatrocinador> PatrocinadorLista_xIdJuego( int idjuego, string valor="")
		{
			List<Clases.cPatrocinador> l = new List<Clases.cPatrocinador>();
			try
			{

				using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("apuestas.a2022_Apuestas_Patrocinador_Lista_xIdJuego", conn))
				{
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IdJuego", idjuego));
					cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Valor", valor));
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
							var i = new Clases.cPatrocinador()
							{
								IdPatrocinador = int.Parse(item["IdPatrocinador"].ToString()),
								idPersona = int.Parse(item["idPersona"].ToString()),
								RazonSocial = item["PatrocinadorRazonSocial"].ToString(),
								Estado= bool.Parse(item["Estado"].ToString()),
								ImgNombre = item["PatrocinadorImgNombre"].ToString(),
								ImgRuta = item["PatrocinadorImgRuta"].ToString(),
								ImgExt = item["PatrocinadorImgExt"].ToString(),
								FC = DateTime.Parse(item["FC"].ToString()),
								FA = DateTime.Parse(item["FA"].ToString()),
								Eliminado = bool.Parse(item["Eliminado"].ToString()),
								idU = int.Parse(item["idU"].ToString()),
								
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

	}
}