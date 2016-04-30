using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Features.Common
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

        public static object NoAreaRouteValue
        {
            get;
        } = new
        {
            area = ""
        };
    }
}
