using System;
using System.Collections.Generic;
using System.Linq;
using TOTD.Utility.StringHelpers;

namespace CritterHeroes.Web.DataProviders.RescueGroups
{
    public static class RescueGroupsHelper
    {
        public static DateTimeOffset? GetDateTime(string value)
        {
            if (value.IsNullOrEmpty())
            {
                return null;
            }
            return DateTimeOffset.Parse(value);
        }

        public static bool YesNoToBoolean(string value)
        {
            return value.SafeEquals("YES");
        }
    }
}
