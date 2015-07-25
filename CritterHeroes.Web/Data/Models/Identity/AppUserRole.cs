using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CritterHeroes.Web.Data.Models.Identity
{
    public class AppUserRole : IdentityUserRole<int>
    {
        public virtual AppRole Role
        {
            get;
            set;
        }

        public virtual AppUser User
        {
            get;
            set;
        }
    }
}