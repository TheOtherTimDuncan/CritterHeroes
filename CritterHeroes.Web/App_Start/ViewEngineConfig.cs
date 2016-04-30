using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CritterHeroes
{
    public static class ViewEngineConfig
    {
        public static void ConfigureViewEngines(ViewEngineCollection viewEngines)
        {
            // {0} = view name or layout name
            // {1} = controller name
            // {2} = area name

            string[] locations = new string[] 
            { 
                "~/Features/{1}/Views/{0}.cshtml",
                "~/Features/{1}s/Views/{0}.cshtml",
                "~/Features/Common/Views/{0}.cshtml"
            };

            string[] areaLocations = new string[] 
            {
                "~/Features/{2}/{1}/Views/{0}.cshtml",
                "~/Features/{2}/{1}s/Views/{0}.cshtml",
                "~/Features/{2}/Views/{0}.cshtml"
            };

            viewEngines.Clear();
            viewEngines.Add(new RazorViewEngine()
            {
                ViewLocationFormats = locations,
                MasterLocationFormats = locations,
                PartialViewLocationFormats = locations,

                AreaMasterLocationFormats = areaLocations,
                AreaPartialViewLocationFormats = areaLocations,
                AreaViewLocationFormats = areaLocations
            });
        }
    }
}
