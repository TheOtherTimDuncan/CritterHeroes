﻿using System.Web;
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

            bundles.Add(new ScriptBundle("~/bundles/cdnFallback").Include("~/Scripts/ch.cdnFallback.js"));

            bundles.Add(new ScriptBundle("~/bundles/cheroes")
                .Include("~/Scripts/ch.data.js"));

            bundles.Add(new ScriptBundle("~/bundles/chusername")
                .Include("~/Scripts/ch.username.js"));

            bundles.Add(new ScriptBundle("~/bundles/datadashboard")
                .Include("~/Scripts/ch.data-dashboard.js"));

            bundles.Add(new ScriptBundle("~/bundles/cheditprofile")
                .Include("~/Scripts/ch.edit-profile.js"));

            bundles.Add(new ScriptBundle("~/bundles/chbusyindicator")
                .Include("~/Scripts/ch.busy-indicator.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            RegisterjQuery(bundles);
            RegisterBootstrap(bundles);
            RegisterModernizr(bundles);
        }

        public static void RegisterjQuery(BundleCollection bundles)
        {
            // jQuery
            Bundle jqueryBundle = new ScriptBundle("~/bundles/jquery", "//ajax.aspnetcdn.com/ajax/jQuery/jquery-2.1.1.min.js")
                .Include("~/Scripts/jquery-2.1.1.*");
            bundles.Add(jqueryBundle);

            // jQuery validation
            Bundle validateBundle = new ScriptBundle("~/bundles/jqueryvalidation", "//ajax.aspnetcdn.com/ajax/jquery.validate/1.13.0/jquery.validate.min.js")
                .Include("~/Scripts/jquery.validate.js");
            bundles.Add(validateBundle);

            // jQuery UI
            Bundle uiBundle = new ScriptBundle("~/bundles/jqueryui", "//ajax.aspnetcdn.com/ajax/jquery.ui/1.11.2/jquery-ui.min.js")
                .Include("~/Scripts/jquery-ui-1.11.2.*");
            bundles.Add(uiBundle);

            // Unobtrusive validation
            Bundle unobtrusiveBundle = new ScriptBundle("~/bundles/jqueryunobtrusive", "//ajax.aspnetcdn.com/ajax/mvc/5.1/jquery.validate.unobtrusive.min.js")
                .Include("~/Scripts/jquery.validate.unobtrusive.js");
            bundles.Add(unobtrusiveBundle);
        }

        public static void RegisterBootstrap(BundleCollection bundles)
        {
            // Bootstrap
            Bundle boostrapScript = new ScriptBundle("~/bundles/bootstrap", "//maxcdn.bootstrapcdn.com/bootstrap/3.3.1/js/bootstrap.min.js")
                .Include("~/Scripts/bootstrap.*");
            bundles.Add(boostrapScript);

            // Respond javascript
            Bundle respondBundle = new ScriptBundle("~/bundles/respond", "//ajax.aspnetcdn.com/ajax/respond/1.2.0/respond.js")
                .Include("~/Scripts/respond.js");
            bundles.Add(respondBundle);
        }

        public static void RegisterModernizr(BundleCollection bundles)
        {
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            Bundle modernizerBundle = new ScriptBundle("~/bundles/modernizr", "//ajax.aspnetcdn.com/ajax/modernizr/modernizr-2.7.2.js")
                .Include("~/Scripts/modernizr-2.7.2.*");
            bundles.Add(modernizerBundle);
        }
    }
}