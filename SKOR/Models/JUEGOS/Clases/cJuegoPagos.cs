using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Skor.Models.JUEGOS.Clases
{
	public class cJuegoPagos
	{
		public int IdJuegoPagos { get; set; }
		public int idPersona { get; set; }
		public int idUsuario { get; set; }
		public int idJuego { get; set; }
		public decimal Valor { get; set; }
		public bool Estado { get; set; }
		public DateTime FC { get; set; }
		public DateTime FA { get; set; }
		public bool Eliminado { get; set; }
		public int idU { get; set; }

		public cJuego JUEGO;
		public cPersona PERSONA;

	}
}