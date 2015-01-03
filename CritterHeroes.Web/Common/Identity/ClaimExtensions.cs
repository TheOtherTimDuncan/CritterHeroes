using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace CritterHeroes.Web.Common.Identity
{
    public static class AppClaimTypes
    {
        public const string UserID = "CritterHeroes.UserID";
    }

    public static class ClaimExtensions
    {
        public static string GetUserID(this IPrincipal principal)
        {
            ClaimsPrincipal claimsPrincipal = principal as ClaimsPrincipal;
            if (claimsPrincipal == null)
            {
                return null;
            }

            return claimsPrincipal.Claims.FirstOrDefault(x => x.Type == AppClaimTypes.UserID).Value;
        }
    }
}
