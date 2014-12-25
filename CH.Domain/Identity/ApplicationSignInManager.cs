using System;
using System.Collections.Generic;
using System.Linq;
using CH.Domain.Contracts;
using CH.Domain.Contracts.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace CH.Domain.Identity
{
    public class ApplicationSignInManager : SignInManager<IdentityUser, string>, IApplicationSignInManager
    {
        public ApplicationSignInManager(IApplicationUserManager userManager, IAuthenticationManager authenticationManager) :
            base(userManager.UserManager, authenticationManager)
        {
        }
    }
}
