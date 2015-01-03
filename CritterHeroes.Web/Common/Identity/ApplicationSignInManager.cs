using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace CritterHeroes.Web.Common.Identity
{
    public class ApplicationSignInManager : SignInManager<IdentityUser, string>, IApplicationSignInManager
    {
        public ApplicationSignInManager(IApplicationUserManager userManager, IAuthenticationManager authenticationManager) :
            base(userManager.UserManager, authenticationManager)
        {
        }
    }
}
