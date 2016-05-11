using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;

namespace CritterHeroes.Web.Shared.OwinExtensions
{
    public static class OwinContextExtensions
    {
        public static string GetReferrer(this IOwinRequest owinRequest)
        {
            string[] value;
            if (owinRequest.Headers.TryGetValue("referer", out value))
            {
                return value[0];
            }

            return null;
        }
    }
}
