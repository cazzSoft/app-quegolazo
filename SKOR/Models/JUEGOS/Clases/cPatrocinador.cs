using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Skor.Models.JUEGOS.Clases
{
	public class cPatrocinador
	{
		public int IdPatrocinador { get; set; }
		public string RazonSocial { get; set; }
		public int idPersona { get; set; }
		public string ImgNombre { get; set; }
		public string ImgRuta { get; set; }
		public string ImgExt { get; set; }
		public bool Estado { get; set; }
		public DateTime FC { get; set; }
		public DateTime FA { get; set; }
		public bool Eliminado { get; set; }
		public int idU { get; set; }

	}
}