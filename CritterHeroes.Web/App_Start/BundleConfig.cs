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

            bundles.Add(new ScriptBundle("~/bundles/cdnFallback").Include("~/src/js/ch.cdnFallback.js"));

            bundles.Add(new ScriptBundle("~/bundles/cheroes").Include("~/src/js/bundled/*.js"));

            bundles.Add(new ScriptBundle("~/bundles/datadashboard").Include("~/src/js/ch.data-dashboard.js"));
            bundles.Add(new ScriptBundle("~/bundles/cheditprofile").Include("~/src/js/ch.edit-profile.js"));
            bundles.Add(new ScriptBundle("~/bundles/chbusyindicator").Include("~/src/js/ch.busy-indicator.js"));
            bundles.Add(new ScriptBundle("~/bundles/chlogin").Include("~/src/js/ch.login.js"));
            bundles.Add(new ScriptBundle("~/bundles/orgedit").Include("~/src/js/ch.organization-edit-profile.js"));
            bundles.Add(new ScriptBundle("~/bundles/admincritters").Include("~/src/js/ch.admin-critters.js"));
            bundles.Add(new ScriptBundle("~/bundles/critterslist").Include("~/src/js/ch.critters-list.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            RegisterjQuery(bundles);
            RegisterBootstrap(bundles);
            RegisterModernizr(bundles);
            RegisterDropZone(bundles);
        }

        public static void RegisterjQuery(BundleCollection bundles)
        {
            // jQuery
            Bundle jqueryBundle = new ScriptBundle("~/bundles/jquery", "//ajax.aspnetcdn.com/ajax/jQuery/jquery-2.1.4.min.js")
                .Include("~/Scripts/jquery-2.1.4.*");
            bundles.Add(jqueryBundle);

            // jQuery validation
            Bundle validateBundle = new ScriptBundle("~/bundles/jqueryvalidation", "//ajax.aspnetcdn.com/ajax/jquery.validate/1.13.1/jquery.validate.min.js")
                .Include("~/Scripts/jquery.validate.js");
            bundles.Add(validateBundle);

            // Unobtrusive validation
            Bundle unobtrusiveBundle = new ScriptBundle("~/bundles/jqueryunobtrusive", "//ajax.aspnetcdn.com/ajax/mvc/5.2.3/jquery.validate.unobtrusive.min.js")
                .Include("~/Scripts/jquery.validate.unobtrusive.js");
            bundles.Add(unobtrusiveBundle);

            // ajax
            Bundle ajaxBundle = new ScriptBundle("~/bundles/jqueryunobtrusiveajax", "//ajax.aspnetcdn.com/ajax/mvc/3.0/jquery.unobtrusive-ajax.min.js")
                .Include("~/Scripts/jquery.unobtrusive-ajax.js");
            bundles.Add(ajaxBundle);
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
