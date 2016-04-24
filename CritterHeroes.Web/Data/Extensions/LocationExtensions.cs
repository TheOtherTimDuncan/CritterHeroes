using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Data.Models;

namespace CritterHeroes.Web.Data.Extensions
{
    public static class LocationExtensions
    {
        public static IQueryable<Location> MatchingID(this IQueryable<Location> source, int locationID)
        {
            return source.Where(x => x.ID == locationID);
        }

        public static IQueryable<Location> MatchingRescueGroupsID(this IQueryable<Location> source, int rescueGroupsID)
        {
            return source.Where(x => x.RescueGroupsID == rescueGroupsID);
        }

        public async static Task<Location> FindByIDAsync(this IQueryable<Location> source, int locationID)
        {
            return await source.MatchingID(locationID).SingleOrDefaultAsync();
        }

        public async static Task<Location> FindByRescueGroupsIDAsync(this IQueryable<Location> source, int rescueGroupsID)
        {
            return await source.MatchingRescueGroupsID(rescueGroupsID).SingleOrDefaultAsync();
        }
    }
}
