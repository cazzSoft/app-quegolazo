using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Skor.Models
{
    public class UsuarioPersona
    {
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string email { get; set; }
        public string clave { get; set; }
        public string telefono { get; set; }

        public UsuarioPersona() { }


        public bool guardar(ref string mensaje, ref long idAsignado)
        {
            MIS_Personas p;

            if (vUsuarios.usuario.VerificaNombreUsuarioRepetido(this.email))
            {
                mensaje = "Este email ya está registrado";
                return false;
            }

            else
            {
                using (var baseSk = new skorEntities()) {
                    p = new MIS_Personas() {nombre1 =nombre,apellido1=apellido,email=email,telefono=telefono };
                    baseSk.MIS_Personas.Add(p);
                    baseSk.SaveChanges();
                }

                vUsuarios.usuario u = new vUsuarios.usuario();
                u.idPersona = p.id;
                u.clave = this.clave;
                u.contraseña = this.clave;
                u.nombreUsuario = this.email;
                u.nivel = 1;
                u.activo = true;
                u.email = this.email;
                u.fechaCreacion = DateTime.Now;
                u.fechaModificacionPWD = DateTime.Now.AddYears(5);
                u.fechaValidezPWD = DateTime.Now.AddYears(1);
                u.guardar();
                idAsignado = u.id;

                return true;
            }
        }
    }
}