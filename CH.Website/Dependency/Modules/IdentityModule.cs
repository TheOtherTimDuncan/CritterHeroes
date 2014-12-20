using System;
using System.Collections.Generic;
using CH.Azure.Identity;
using CH.Domain.Contracts.Identity;
using CH.Domain.Identity;
using Ninject.Modules;

namespace CH.Website.Dependency.Modules
{
    public class IdentityModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IApplicationUserStore>().To<UserStore>();
            Bind<IApplicationUserManager>().To<ApplicationUserManager>();
            Bind<IApplicationSignInManager>().To<ApplicationSignInManager>();
        }
    }
}
