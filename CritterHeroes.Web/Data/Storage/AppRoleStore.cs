using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Data.Contexts;
using CritterHeroes.Web.Data.Models.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CritterHeroes.Web.Data.Storage
{
    public class AppRoleStore : RoleStore<AppRole, int, AppUserRole>, IQueryableRoleStore<AppRole, int>, IRoleStore<AppRole, int>, IDisposable
    {
        public AppRoleStore(AppUserStorageContext context)
            : base(context)
        {
        }
    }
}