using System;
using System.Collections.Generic;
using System.Linq;

namespace CritterHeroes.Web.Common.Identity
{
    public static class IdentityRole
    {
        public const string MasterAdmin = "MasterAdmin";
        public const string Admin = "Admin";
        public const string Volunteer = "Volunteer";
        public const string Foster = "Foster";
        public const string Applicant = "Applicant";

        public static IEnumerable<string> All
        {
            get
            {
                yield return IdentityRole.MasterAdmin;
                yield return IdentityRole.Admin;
                yield return IdentityRole.Volunteer;
                yield return IdentityRole.Foster;
                yield return IdentityRole.Applicant;
            }
        }
    }
}
