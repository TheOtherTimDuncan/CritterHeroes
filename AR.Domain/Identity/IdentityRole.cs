using System;
using System.Collections.Generic;
using System.Linq;

namespace AR.Domain.Identity
{
    public class IdentityRole
    {
        public static IdentityRole MasterAdmin =    new IdentityRole("0", "Master Admin");
        public static IdentityRole SuperAdmin =     new IdentityRole("1", "Super Admin");
        public static IdentityRole Volunteer =      new IdentityRole("2", "Volunteer");
        public static IdentityRole Foster =         new IdentityRole("3", "Foster");
        public static IdentityRole Applicatant =    new IdentityRole("4", "Applicant");

        public static IEnumerable<IdentityRole> All
        {
            get
            {
                yield return MasterAdmin;
                yield return SuperAdmin;
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
