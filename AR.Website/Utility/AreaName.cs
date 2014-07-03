using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using TOTD.Utility.Misc;

namespace AR.Website.Utility
{
    public class AreaName : Enumeration<AreaName>
    {
        public static AreaName None = new AreaName(0, "");
        public static AreaName Admin = new AreaName(1, "Admin");

        public object RouteValue
        {
            get;
            private set;
        }

        private AreaName()
        {
        }

        private AreaName(int value, string displayName)
            : base(value, displayName)
        {
            RouteValue = new
            {
                area = displayName
            };
        }
    }
}