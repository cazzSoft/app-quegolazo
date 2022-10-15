using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace SkorAdmin.Models.Util
{
    public class General
    {
        private static string usuarioVar = "usuarioSkor";

        public static string getJsonFromObject(object objeto)
        {

            string myObjectJson = new JavaScriptSerializer().Serialize(objeto);

            return myObjectJson;
        }

        public static void setCookie(string key, string data)
        {
            var cookie = new HttpCookie(key, data)
            {
                Expires = DateTime.Now.AddYears(1)
            };
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

        public static void setUsuario(UsuarioSkor elUser)
        {
            string jsonO;
            if (elUser != null)
            {
                jsonO = General.getJsonFromObject(elUser);
                General.setCookie(usuarioVar, jsonO);
            }
        }

        public static UsuarioSkor getUsuario()
        {
            string cookie = getCookie(usuarioVar);
            if (cookie != "")
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                return json_serializer.Deserialize<UsuarioSkor>(cookie);
            }
            else
            {
                return null;
            }

        }
    }
}