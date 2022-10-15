using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Correos
{
    public class RecuperaClave : Mailer
    {
        private string email;
        vUsuarios.usuario elUser;
        private string pwdTmp;

        public RecuperaClave(vUsuarios.usuario user, string correo, string clave) {
            this.elUser = user;
            this.email = correo;
            this.pwdTmp = clave;
            this.esHTML = true;
        }

        public override void EnviarCorreo() {
            Mailer.BuildContenido elContenido = delegate () {
                string path;
                string contenido;


                path = HttpContext.Current.Server.MapPath("~/") + "Plantillas/RecuperaClave.html";

                contenido = this.LeerPlantilla(path);
                contenido = contenido.Replace("{{SITEROOT}}", System.Configuration.ConfigurationManager.AppSettings["SITEROOT"]);
                contenido = contenido.Replace("{{anno}}", DateTime.Today.Year.ToString());
                contenido = contenido.Replace("{{clave}}", pwdTmp);


                return contenido;
            };

            this.Enviar("¡Que golazo! - Recuperación de clave", this.email, elContenido);
        }

    }
}