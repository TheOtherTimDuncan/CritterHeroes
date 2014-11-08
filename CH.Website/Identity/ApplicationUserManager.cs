using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using CH.Azure.Identity;
using CH.Domain.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace CH.Website.Identity
{
    public class ApplicationUserManager : UserManager<IdentityUser>
    {
        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            ApplicationUserManager manager = new ApplicationUserManager(new UserStore(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString));
            return manager;
        }

        public ApplicationUserManager(IUserStore<IdentityUser> store)
            : base(store)
        {
        }
    }
}