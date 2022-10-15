using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace Skor.Controllers.Util
{
    public class General
    {

        private static string usuarioVar = "usuarioSkor";

        public static void setCookie(string key, string data)
        {
            var cookie = new HttpCookie(key, data)
            {
                Expires = DateTime.Now.AddYears(1)
            };
            //HttpContext.Response.Cookies.Add(cookie);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static string getCookie(string key)
        {
            string valor = "";
            if (HttpContext.Current.Request.Cookies[key] != null)
            {
                valor = HttpContext.Current.Request.Cookies[key].Value;
            }

            return valor;
        }

        public static void setUsuario(Models.USR_usuarios elUser)
        {
            string jsonO;
            if (elUser != null)
            {
                jsonO = getJsonFromObject(elUser);
                setCookie(usuarioVar, jsonO);
            }
        }

        public static Models.USR_usuarios getUsuario()
        {
            string cookie = getCookie(usuarioVar);
            if (cookie != "")
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                //return json_serializer.Deserialize<Models.USR_usuarios>(cookie);
                return JsonConvert.DeserializeObject<Models.USR_usuarios>(cookie);
            }
            else
            {
                return null;
            }

        }

        public static string getJsonFromObject(object objeto)
        {
            //string myObjectJson = new JavaScriptSerializer().Serialize(objeto);
            string myObjectJson = JsonConvert.SerializeObject(objeto, Formatting.Indented);

            return myObjectJson;
        }

        public static Models.Cartillas getCartilla(int idCartilla)
        {

            vUsuarios.usuario user;
            Models.Cartillas laCartilla;

            user = vUsuarios.web.TraeUsuarioRegistrado();

            using (var baseSk = new Models.skorEntities())
            {
                laCartilla = (from c in baseSk.Cartillas where c.id == idCartilla select c).FirstOrDefault();
            }
            return laCartilla;
        }


        public static string LeerPlantilla(string path)
        {
            StreamReader reader = default(StreamReader);
            string str = null;

            path = HttpContext.Current.Server.MapPath("~/")+ path;

            reader = new StreamReader(path, Encoding.Default);
            str = reader.ReadToEnd();
            reader.Close();

            return str;
        }

    }
}