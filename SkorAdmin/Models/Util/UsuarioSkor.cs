using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkorAdmin.Models.Util
{
    public class UsuarioSkor
    {
        public string NombreUsuario { get; set; }
        public string Clave { get; set; }

        public UsuarioSkor() { }

        public UsuarioSkor(string u, string c) {
            NombreUsuario = u;
            Clave = c;
        }

    }
}