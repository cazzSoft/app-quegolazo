using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Skor.Models.JUEGOS.Clases
{
	public class cCartilla
	{
		public int IdCartilla { get; set; }
		public int idEmpresa { get; set; }
		public int idAdministrador { get; set; }
		public string Nombre { get; set; }
		public DateTime FechaCierre { get; set; }
		public string Descripcion { get; set; }
		public string Premios { get; set; }
		public string Condiciones { get; set; }
		public bool Cerrada { get; set; }
		public string URLBanner { get; set; }
		public bool Terminada { get; set; }
		public bool Activa { get; set; }

		public Models.JUEGOS.Clases.cJuegoCartilla JUEGOCARTILLA;
		
	}
}