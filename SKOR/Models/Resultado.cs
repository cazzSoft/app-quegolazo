using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Skor.Models
{
    public class Resultado
    {
        public Boolean exitoso { get; set; }
        public Object data { get; set; }
        public int codigo { get; set; }

        public Resultado()
        {
        }

        public Resultado(Boolean exitoso, object data, int codigo = 0)
        {
            this.exitoso = exitoso;
            this.data = data;
            this.codigo = codigo;
        }
    }
}