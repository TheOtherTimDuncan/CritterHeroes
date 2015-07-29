using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Models.Identity;

namespace CritterHeroes.Web.Data.Extensions
{
    public static class AppUserExtensions
    {
        public static AppUser FindByUsername(this ISqlStorageContext<AppUser> storageContext, string username)
        {
            return storageContext.Get(x => x.UserName == username);
        }

        public async static Task<AppUser> FindByUsernameAsync(this ISqlStorageContext<AppUser> storageContext, string username)
        {
            return await storageContext.GetAsync(x => x.UserName == username);
        }
    }
}