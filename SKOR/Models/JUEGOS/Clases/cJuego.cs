using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Skor.Models.JUEGOS.Clases
{
	public class cJuego
	{
		public int IdJuego { get; set; }
		public string Juego { get; set; }
		public int idJuegoEstado { get; set; }
		public string Codigo { get; set; }
		public string JuegoDescripcion { get; set; }
		public string ImgNombre { get; set; }
		public string ImgRuta { get; set; }
		public string ImgExt { get; set; }
		public DateTime FechaInicio { get; set; }
		public DateTime FechaFin { get; set; }
		public bool Publicado { get; set; }
		public DateTime FC { get; set; }
		public DateTime FA { get; set; }
		public bool Eliminado { get; set; }
		public int idU { get; set; }
		public int NumPatrocinadores { get; set; }
		public bool Bloqueo { get; set; }

		public cJuegoEstado ESTADO;
		public	List<cPatrocinador> PATROCINADORES;

	}
}