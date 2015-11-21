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
            RegisterModernizr(bundles);
            RegisterDropZone(bundles);
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

        public static void RegisterModernizr(BundleCollection bundles)
        {
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            Bundle modernizerBundle = new ScriptBundle("~/bundles/modernizr", "//ajax.aspnetcdn.com/ajax/modernizr/modernizr-2.8.3.js")
                .Include("~/Scripts/modernizr-2.8.3.*");
            bundles.Add(modernizerBundle);
        }

        public static void RegisterDropZone(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/dropzonescripts").Include(
                     "~/Scripts/dropzone/dropzone.js"));

            bundles.Add(new StyleBundle("~/Content/dropzonecss").Include(
                     "~/Scripts/dropzone/basic.css",
                     "~/Scripts/dropzone/dropzone.css"
                 ));
        }
    }
}
