using System;
using System.Collections.Generic;
using TOTD.Utility.StringHelpers;

namespace CritterHeroes.Web.Common
{
    public static class SexHelper
    {
        public static string GetSexName(string sex)
        {
            if (sex.SafeEquals("M"))
            {
                return "Male";
            }

            if (sex.SafeEquals("F"))
            {
                return "Female";
            }

            return sex;
        }
    }
}
