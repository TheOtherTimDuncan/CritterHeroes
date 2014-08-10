using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using AR.Azure.Identity;
using AR.Domain.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace AR.Website.Identity
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