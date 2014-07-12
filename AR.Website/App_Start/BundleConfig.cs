using System.Web;
using System.Web.Optimization;

namespace AR.Website
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //BundleTable.EnableOptimizations = true;

            bundles.UseCdn = true;

            // jQuery
            Bundle jqueryBundle = new ScriptBundle("~/bundles/jquery", "//ajax.aspnetcdn.com/ajax/jQuery/jquery-2.1.1.min.js")
                .Include("~/Scripts/Libraries/jquery-2.1.1.*");
            jqueryBundle.CdnFallbackExpression = "window.jQuery";
            bundles.Add(jqueryBundle);

            // jQuery validation
            Bundle validateBundle = new ScriptBundle("~/bundles/jqueryvalidation", "//ajax.aspnetcdn.com/ajax/jquery.validate/1.13.0/jquery.validate.min.js")
                .Include("~/Scripts/Libraries/jquery.validate.js");
            validateBundle.CdnFallbackExpression = "window.jQuery.validator";
            bundles.Add(validateBundle);

            // Unobtrusive validation
            Bundle unobtrusiveBundle = new ScriptBundle("~/bundles/jqueryunobtrusive", "//ajax.aspnetcdn.com/ajax/mvc/5.1/jquery.validate.unobtrusive.min.js")
                .Include("~/Scripts/Libraries/jquery.validate.unobtrusive.js");
            unobtrusiveBundle.CdnFallbackExpression = "window.jQuery.validator.unobtrusive";
            bundles.Add(unobtrusiveBundle);

            // Bootstrap javascript
            Bundle bootstrapBundle = new ScriptBundle("~/bundles/bootstrap", "//maxcdn.bootstrapcdn.com/bootstrap/3.2.0/js/bootstrap.min.js")
                .Include("~/Scripts/Libraries/bootstrap-3.2.0.*");
            bootstrapBundle.CdnFallbackExpression = "$.fn.modal";
            bundles.Add(bootstrapBundle);

            // Respond javascript
            Bundle respondBundle = new ScriptBundle("~/bundles/respond", "//ajax.aspnetcdn.com/ajax/respond/1.2.0/respond.js")
                .Include("~/Scripts/Libraries/respond.js");
            respondBundle.CdnFallbackExpression = "window.respond";
            bundles.Add(respondBundle);

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            Bundle modernizerBundle = new ScriptBundle("~/bundles/modernizr", "//ajax.aspnetcdn.com/ajax/modernizr/modernizr-2.7.2.js")
                .Include("~/Scripts/Libraries/modernizr-2.7.2.*");
            modernizerBundle.CdnFallbackExpression = "window.modernizer";
            bundles.Add(modernizerBundle);

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/site.css"));
        }
    }
}
