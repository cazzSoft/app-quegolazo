using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Correos
{
    public class SimpleMail : Mailer
    {
        string titulo;
        string correo;
        string contenido;
        public SimpleMail(string titulo, string email, string texto) : base()
        {
            this.titulo = titulo;
            this.correo = email;
            this.contenido = texto;
        }

        public override void EnviarCorreo()
        {

            Mailer.BuildContenido elContenido = delegate () {
                return this.contenido;
            };

            this.Enviar(this.titulo, this.correo, elContenido);
        }
    }
}