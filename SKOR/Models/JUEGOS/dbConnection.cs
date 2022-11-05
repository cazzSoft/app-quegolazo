using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace Skor.Models.JUEGOS
{
	public class dbConnection
	{
		private string connectionString = "data source=ANONYMOUS;initial catalog=skor-2021-10-6-12-5;persist security info=True;user id=sa;password=12345678;MultipleActiveResultSets=True;App=EntityFramework&quot;";

		//private string connectionString = "data source=192.168.100.99;initial catalog=skor-2021-10-6-12-5;persist security info=True;user id=sa;password=adstecnologia2022;MultipleActiveResultSets=True;App=EntityFramework&quot;";


		private SqlConnection conectardb = new SqlConnection();

		private static dbConnection con = null;

		public dbConnection()
		{
			this.conectardb.ConnectionString = connectionString;
		}


		public SqlConnection EntidadSql()
		{
			return conectardb;
		}

	}
}