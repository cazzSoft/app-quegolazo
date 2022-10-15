using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Skor.Controllers
{
    public class AboutController : Controller
    {
        // GET: About
        public ActionResult Index()
        {


            return View();
        }

        [HttpPost]
        public JsonResult Mensaje()
        {
            xf a = new xf();

            a.fulfillmentText = "miRespuesta";
            a.payload = new msg();
            a.payload.telegram = new tlgrm();
            a.payload.telegram.text = "Escoja una opción";
            a.payload.telegram.reply_markup = new tlgrRM();


            a.payload.telegram.reply_markup.inline_keyboard = new List<List<ik>>();

            List<ik> b = new List<ik>();
            b.Add(new ik("Opcion1", "Do Opcion1"));
            a.payload.telegram.reply_markup.inline_keyboard.Add(b);

            List<ik> c = new List<ik>();
            c.Add(new ik("OpcionDOS", "Do Opcion2"));
            a.payload.telegram.reply_markup.inline_keyboard.Add(c);

           
            return Json(a);
        }
    }
}

class xf {
    public string fulfillmentText = "Respuesta simple";
    public msg payload;

}

class msg {
    public tlgrm telegram;
}

class tlgrm {
    public string text;
    public tlgrRM reply_markup;
}

class tlgrRM {    
    public List<List<ik>> inline_keyboard;
}


 class LIK {
    public ik ik1;
}

class ik {
    public string text;
    public string callback_data;

    public  ik(string text, string data) {
        this.text = text;
        this.callback_data = data;
    }
}

