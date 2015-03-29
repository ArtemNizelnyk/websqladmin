using System.Web;
using System.Web.Optimization;

namespace WebSQLMan
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/libs/jquery.js", "~/Scripts/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jstree").Include(
                        "~/Scripts/jstree.js"));

            bundles.Add(new StyleBundle("~/Content/treestyle").Include("~/Scripts/themes/default/style.css"));

        }
    }
}