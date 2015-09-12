using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Areas.Common
{
    public class AreaName
    {
        public const string Admin = "Admin";

        public static object AdminRouteValue
        {
            get;
        } = new
        {
            area = Admin
        };
    }
}
