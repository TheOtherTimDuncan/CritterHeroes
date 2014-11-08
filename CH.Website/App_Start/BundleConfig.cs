using System.Web;
using System.Web.Optimization;

namespace CH.Website
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //BundleTable.EnableOptimizations = true;

            bundles.IgnoreList.Ignore("*.less");

            bundles.UseCdn = true;

            bundles.Add(new ScriptBundle("~/bundles/cdnFallback").Include("~/Scripts/ch.cdnFallback.js"));

            bundles.Add(new ScriptBundle("~/bundles/cheroes")
                .Include("~/Scripts/ch.data.js"));

            bundles.Add(new ScriptBundle("~/bundles/datadashboard")
                .Include("~/Scripts/ch.data-dashboard.js"));

            // jQuery
            Bundle jqueryBundle = new ScriptBundle("~/bundles/jquery", "//ajax.aspnetcdn.com/ajax/jQuery/jquery-2.1.1.min.js")
                .Include("~/Scripts/jquery-2.1.1.*");
            bundles.Add(jqueryBundle);

            // jQuery validation
            Bundle validateBundle = new ScriptBundle("~/bundles/jqueryvalidation", "//ajax.aspnetcdn.com/ajax/jquery.validate/1.13.0/jquery.validate.min.js")
                .Include("~/Scripts/jquery.validate.js");
            bundles.Add(validateBundle);

            // Unobtrusive validation
            Bundle unobtrusiveBundle = new ScriptBundle("~/bundles/jqueryunobtrusive", "//ajax.aspnetcdn.com/ajax/mvc/5.1/jquery.validate.unobtrusive.min.js")
                .Include("~/Scripts/jquery.validate.unobtrusive.js");
            bundles.Add(unobtrusiveBundle);

            // Bootstrap
            Bundle boostrapScript = new ScriptBundle("~/bundles/bootstrap", "//maxcdn.bootstrapcdn.com/bootstrap/3.2.0/js/bootstrap.min.js")
                .Include("~/Scripts/bootstrap-3.2.0.*");
            Bundle bootstrapCss = new StyleBundle("~/Content/bootstrap", "//maxcdn.bootstrapcdn.com/bootswatch/3.2.0/yeti/bootstrap.min.css")
                .Include("~/Content/bootstrap.yeti.*");
            bundles.Add(boostrapScript);
            bundles.Add(bootstrapCss);

            // Respond javascript
            Bundle respondBundle = new ScriptBundle("~/bundles/respond", "//ajax.aspnetcdn.com/ajax/respond/1.2.0/respond.js")
                .Include("~/Scripts/respond.js");
            bundles.Add(respondBundle);

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            Bundle modernizerBundle = new ScriptBundle("~/bundles/modernizr", "//ajax.aspnetcdn.com/ajax/modernizr/modernizr-2.7.2.js")
                .Include("~/Scripts/modernizr-2.7.2.*");
            bundles.Add(modernizerBundle);

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));
        }
    }
}
