using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Skor.Models.JUEGOS.Clases
{
	public class cJuegoCartilla
	{
		public int IdJuegoCartilla { get; set; }
		public int idJuego { get; set; }
		public int idCartilla { get; set; }
		public DateTime FC { get; set; }
		public DateTime FA { get; set; }
		public bool Eliminado { get; set; }
		public int idU { get; set; }

		public Models.JUEGOS.Clases.cJuego JUEGO;

	}
}