using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Skor.Models
{
    public class bandoEquipo
    {


        public static  string GeneraCodigo(string nombre, int id)
        {
            string vowels = "aeiouy";

            string cod = "";

            string temp = nombre.Split(' ')[0];

            cod = new string(temp.Where(c => !vowels.Contains(c)).ToArray()) + id;
            
            return cod;
        }

    }
}