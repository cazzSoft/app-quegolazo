using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Skor.Models
{
    public class MiCartillaUsuario
    {
        public int idCartillaUsuario;
        public int idCartilla;

        public Cartillas laCartilla;  //puede ser el objeto = tabla
        public List<SP_PronosticosCartillaUsuario_Result> MisPronosticos;   //puede ser obejtos igual a lo que devuelve el sp
        public List<SP_AvancesFaseCartilla_Result> MisAvancesfase;


        public MiCartillaUsuario(int idCartillaUsuario_) {
            idCartillaUsuario = idCartillaUsuario_;
            using (var baseSk = new skorEntities()) {
                //this.laCartilla = (from c in baseSk.Cartillas where c.id == idCartillaUsuario select c).FirstOrDefault();
                this.MisPronosticos = baseSk.SP_PronosticosCartillaUsuario(idCartillaUsuario).ToList();
                this.MisAvancesfase = baseSk.SP_AvancesFaseCartilla(idCartillaUsuario).ToList();
            }
        }

        public void cargarPronosticos() {
            using (var baseSk = new skorEntities())
            {
                this.MisPronosticos = baseSk.SP_PronosticosCartillaUsuario(idCartillaUsuario).ToList();
            }
        }


        public bool crearPronosticos() {

            //llamar a SP_PROC_CreaPronosticos this.id
            //this.cargaPronosticos();
            using (var baseSk = new skorEntities())
            {
                baseSk.SP_PROC_CreaPronosticos(this.idCartillaUsuario);
            }

            return true;
        }


        //Devuelve true si el pronostico esta completo
        public static bool guardarGrupos(int idCu, List< ResultadoGrupo> pg) {

            bool algunCompleto = false;

            using (var baseSk = new skorEntities()) {
                foreach (ResultadoGrupo pg1 in pg)
                {
                    Pronosticos pr;

                    pr = (from cr in baseSk.Pronosticos where cr.idCartillaUsuario == idCu && cr.idPartido == pg1.idPartido select cr).FirstOrDefault();

                    if (pg1.numEquipo == 1) { pr.idEquipo1 = pg1.idEquipo; } else { pr.idEquipo2 = pg1.idEquipo; }
                    baseSk.SaveChanges();

                    if (pr.idEquipo1 != null && pr.idEquipo1 > 1 && pr.idEquipo2 != null && pr.idEquipo2 > 1)
                    {
                        algunCompleto = true;
                    }                    
                }

                return algunCompleto;



                    //                    string sql = string.Format("UPDATE pronosticos set idEquipo{0} = {1} WHERE idCartillaUsuario={2} AND idPartido={3}", pg1.numEquipo, pg1.idEquipo, idCu, pg1.idPartido);
                    //baseSk.Database.ExecuteSqlCommand(sql);

                     //todo: cambiar a SP
                    //llamar "SP_ActualizaEquiposPronostico""                     
                }
            }
                






        }


    }

    public class ResultadoGrupo {

        public int idPartido { get; set; }
        public int numEquipo { get; set; }
        public int idEquipo { get; set; }

    }

