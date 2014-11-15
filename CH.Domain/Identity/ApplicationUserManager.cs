using System;
using System.Collections.Generic;
using System.Linq;
using CH.Domain.Contracts.Identity;
using CH.Domain.Identity;
using Microsoft.AspNet.Identity;

namespace CH.Domain.Identity
{
    public class ApplicationUserManager : UserManager<IdentityUser>, IApplicationUserManager
    {
        public ApplicationUserManager(IUserStore<IdentityUser> store)
            : base(store)
        {
        }
    }
}