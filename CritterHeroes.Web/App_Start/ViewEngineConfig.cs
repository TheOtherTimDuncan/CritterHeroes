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
                "~/Areas/{1}/Views/{0}.cshtml", 
                "~/Areas/{1}s/Views/{0}.cshtml", 
                "~/Areas/Common/Views/{0}.cshtml" 
            };

            string[] areaLocations = new string[] 
            { 
                "~/Areas/{2}/{1}/Views/{0}.cshtml", 
                "~/Areas/{2}/{1}s/Views/{0}.cshtml", 
                "~/Areas/{2}/Views/{0}.cshtml" 
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