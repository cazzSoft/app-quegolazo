using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Skor.Models;

namespace Correos
{
    public class RegistroUsuario : Mailer
    {
        string titulo;
        string correo;
        UsuarioPersona laPersona;

        public RegistroUsuario(UsuarioPersona persona) : base()
        {
            laPersona = persona;
            this.esHTML = true;

            titulo = "Bienvenido a ¡Qué Golazo!";
            correo = persona.email;
        }

        public override void EnviarCorreo()
        {
            Mailer.BuildContenido elContenido = delegate () {
                string path;
                string contenido;

                
                path = HttpContext.Current.Server.MapPath("~/") + "Plantillas/RegistroUsuario.html";

                contenido = this.LeerPlantilla(path);
                contenido = contenido.Replace("{{Nombre}}", laPersona.nombre);
                contenido = contenido.Replace("{{Apellido}}", laPersona.apellido);
                contenido = contenido.Replace("{{SITEROOT}}", System.Configuration.ConfigurationManager.AppSettings["SITEROOT"]);
                contenido = contenido.Replace("{{anno}}", DateTime.Today.Year.ToString());


                return contenido;
            };
            
            this.Enviar(this.titulo, this.correo, elContenido);
        }

        
    }
}