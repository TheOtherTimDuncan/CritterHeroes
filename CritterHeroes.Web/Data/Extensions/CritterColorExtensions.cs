using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Data.Models;

namespace CritterHeroes.Web.Data.Extensions
{
    public static class CritterColorExtensions
    {
        public static IQueryable<CritterColor> MatchingID(this IQueryable<CritterColor> source, int colorID)
        {
            return source.Where(x => x.ID == colorID);
        }

        public static IQueryable<CritterColor> MatchingRescueGroupsID(this IQueryable<CritterColor> source, string rescueGroupsID)
        {
            return source.Where(x => x.RescueGroupsID == rescueGroupsID);
        }

        public async static Task<CritterColor> FindByIDAsync(this IQueryable<CritterColor> source, int colorID)
        {
            return await source.MatchingID(colorID).SingleOrDefaultAsync();
        }

        public async static Task<CritterColor> FindByRescueGroupsIDAsync(this IQueryable<CritterColor> source, string rescueGroupsID)
        {
            return await source.MatchingRescueGroupsID(rescueGroupsID).SingleOrDefaultAsync();
        }
    }
}
