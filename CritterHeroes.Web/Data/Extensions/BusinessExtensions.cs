using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Data.Models;

namespace CritterHeroes.Web.Data.Extensions
{
    public static class BusinessExtensions
    {
        public static IQueryable<Business > MatchingID(this IQueryable<Business> source, int businessID)
        {
            return source.Where(x => x.ID == businessID);
        }

        public static IQueryable<Business> MatchingRescueGroupsID(this IQueryable<Business> source, int rescueGroupsID)
        {
            return source.Where(x => x.RescueGroupsID == rescueGroupsID);
        }

        public async static Task<Business> FindByIDAsync(this IQueryable<Business> source, int businessID)
        {
            return await source.MatchingID(businessID).SingleOrDefaultAsync();
        }

        public async static Task<Business> FindByRescueGroupsIDAsync(this IQueryable<Business> source, int rescueGroupsID)
        {
            return await source.MatchingRescueGroupsID(rescueGroupsID).SingleOrDefaultAsync();
        }
    }
}
