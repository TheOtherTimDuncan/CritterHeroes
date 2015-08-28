using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Data.Models.Identity;

namespace CritterHeroes.Web.Data.Extensions
{
    public static class AppUserExtensions
    {
        public static IQueryable<AppUser> MatchingUsername(this IQueryable<AppUser> source, string username)
        {
            return source.Where(x => x.UserName == username);
        }

        public static async Task<AppUser> FindByUsernameAsync(this IQueryable<AppUser> source, string username)
        {
            return await source.MatchingUsername(username).SingleOrDefaultAsync();
        }
    }
}