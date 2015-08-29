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

        public static IQueryable<AppUser> MatchingID(this IQueryable<AppUser> source, int userID)
        {
            return source.Where(x => x.Id == userID);
        }

        public static async Task<AppUser> FindByUsernameAsync(this IQueryable<AppUser> source, string username)
        {
            return await source.MatchingUsername(username).SingleOrDefaultAsync();
        }

        public static async Task<AppUser> FindByIDAsync(this IQueryable<AppUser> source, int userID)
        {
            return await source.MatchingID(userID).SingleOrDefaultAsync();
        }
    }
}
