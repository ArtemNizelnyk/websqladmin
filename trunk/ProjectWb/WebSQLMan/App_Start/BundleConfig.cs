using System.Web;
using System.Web.Optimization;

namespace WebSQLMan
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Content/Scripts/libs/jquery.js", "~/Content/Scripts/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jstree").Include(
                        "~/Content/Scripts/jstree.js"));

            bundles.Add(new StyleBundle("~/Content/treestyle").Include("~/Content/Scripts/themes/default/style.css"));

        }
    }
}