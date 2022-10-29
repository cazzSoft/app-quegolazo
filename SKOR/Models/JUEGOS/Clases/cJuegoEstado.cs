using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Skor.Models.JUEGOS.Clases
{
	public class cJuegoEstado
	{
		public int IdJuegoEstado { get; set; }
		public string JuegoEstado { get; set; }
		public DateTime FC { get; set; }
		public DateTime FA { get; set; }
		public bool Eliminado { get; set; }
		public int idU { get; set; }
	}
}