using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Data.Contexts;
using CritterHeroes.Web.Data.Models.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CritterHeroes.Web.Data.Storage
{
    public class AppUserStore : UserStore<AppUser, AppRole, int, AppUserLogin, AppUserRole, AppUserClaim>, IAppUserStore, IDisposable
    {
        public AppUserStore(AppUserStorageContext context)
            : base(context)
        {
        }
    }
}