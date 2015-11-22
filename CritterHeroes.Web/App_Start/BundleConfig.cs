using System.Web;
using System.Web.Optimization;

namespace CritterHeroes.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //BundleTable.EnableOptimizations = true;

            bundles.IgnoreList.Ignore("*.less");

            bundles.UseCdn = true;

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            RegisterBootstrap(bundles);
        }

        public static void RegisterBootstrap(BundleCollection bundles)
        {
            // Bootstrap
            Bundle boostrapScript = new ScriptBundle("~/bundles/bootstrap", "//maxcdn.bootstrapcdn.com/bootstrap/3.3.5/js/bootstrap.min.js")
                .Include("~/Scripts/bootstrap.*");
            bundles.Add(boostrapScript);

            // Respond javascript
            Bundle respondBundle = new ScriptBundle("~/bundles/respond", "//ajax.aspnetcdn.com/ajax/respond/1.4.0/respond.js")
                .Include("~/Scripts/respond.js");
            bundles.Add(respondBundle);
        }
    }
}
