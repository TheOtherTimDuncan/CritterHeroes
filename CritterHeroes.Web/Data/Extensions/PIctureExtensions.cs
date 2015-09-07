using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Data.Models;

namespace CritterHeroes.Web.Data.Extensions
{
    public static class PIctureExtensions
    {
        public static IQueryable<Picture> MatchingID(this IQueryable<Picture> source, int pictureID)
        {
            return source.Where(x => x.ID == pictureID);
        }

        public static IQueryable<Picture> MatchingRescueGroupsID(this IQueryable<Picture> source, string rescueGroupsID)
        {
            return source.Where(x => x.RescueGroupsID == rescueGroupsID);
        }

        public async static Task<Picture> FindByIDAsync(this IQueryable<Picture> source, int pictureID)
        {
            return await source.MatchingID(pictureID).SingleOrDefaultAsync();
        }

        public async static Task<Picture> FindByRescueGroupsIDAsync(this IQueryable<Picture> source, string rescueGroupsID)
        {
            return await source.MatchingRescueGroupsID(rescueGroupsID).SingleOrDefaultAsync();
        }
    }
}
