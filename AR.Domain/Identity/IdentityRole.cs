using System;
using System.Collections.Generic;
using System.Linq;

namespace AR.Domain.Identity
{
    public class IdentityRole
    {
        public static IdentityRole MasterAdmin =    new IdentityRole("0", RoleNames.MasterAdmin);
        public static IdentityRole Admin =          new IdentityRole("1", RoleNames.Admin);
        public static IdentityRole Volunteer =      new IdentityRole("2", RoleNames.Volunteer);
        public static IdentityRole Foster =         new IdentityRole("3", RoleNames.Foster);
        public static IdentityRole Applicatant =    new IdentityRole("4", RoleNames.Applicant);

        // Require to use these with the AuthorizeAttribute
        public class RoleNames
        {
            public const string MasterAdmin     = "Master Admin";
            public const string Admin           = "Admin";
            public const string Volunteer       = "Volunteer";
            public const string Foster          = "Foster";
            public const string Applicant       = "Applicant";
        }

        public static IEnumerable<IdentityRole> All
        {
            get
            {
                yield return MasterAdmin;
                yield return Admin;
                yield return Volunteer;
                yield return Foster;
                yield return Applicatant;
            }
        }

        private IdentityRole(string id, string name)
        {
            this.ID = id;
            this.Name = name;
        }

        public string ID
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }
    }
}
