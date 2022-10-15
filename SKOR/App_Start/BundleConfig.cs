using System.Web;
using System.Web.Optimization;

namespace Skor
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/Scripts/js").Include(
                        "~/Scripts/jquery-3.3.1.js",
                        "~/Scripts/framework7.js",
                        "~/Scripts/my-app.js",
                        "~/Scripts/skor/htclib.js"
                        ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/framework7.css", "~/Content/buttons.css", "~/Content/my-app.css", "~/style.css"));
        }
    }
}
