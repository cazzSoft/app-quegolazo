using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Skor.Models.JUEGOS.Metodos
{
    public class mCartillasUsuario
    {
        System.Data.SqlClient.SqlConnection conn = new dbConnection().EntidadSql();
        public  eResultado  CartillaUsuario_ActualizarIdJuego(int idcartillausuario, int idJuego)
        {
            eResultado l = new eResultado();
            try
            {

                using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("apuestas.a2022_Apuestas_CartillasUsuario_Actualizar_IdJuego", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IdCartillaUsuario", idcartillausuario));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IdJuego", idJuego));
                    System.Data.DataTable dt = new System.Data.DataTable();
                    System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                    conn.Open();
                    da.Fill(dt);
                    if (int.Parse(dt.Rows.Count.ToString()) > 0)
                    {
                        System.Data.DataRow item = dt.Rows[0];
                        eResultado i = new eResultado() { 
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