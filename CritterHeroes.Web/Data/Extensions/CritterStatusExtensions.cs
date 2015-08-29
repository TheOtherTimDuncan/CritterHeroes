using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Data.Models;

namespace CritterHeroes.Web.Data.Extensions
{
    public static class CritterStatusExtensions
    {
        public static IQueryable<CritterStatus> MatchingID(this IQueryable<CritterStatus> source, int statusID)
        {
            return source.Where(x => x.ID == statusID);
        }

        public static IQueryable<CritterStatus> MatchingName(this IQueryable<CritterStatus> source, string name)
        {
            return source.Where(x => x.Name == name);
        }

        public static IQueryable<CritterStatus> MatchingRescueGroupsID(this IQueryable<CritterStatus> source, string rescueGroupsID)
        {
            return source.Where(x => x.RescueGroupsID == rescueGroupsID);
        }

        public async static Task<CritterStatus> FindByIDAsync(this IQueryable<CritterStatus> source, int statusID)
        {
            return await source.MatchingID(statusID).SingleOrDefaultAsync();
        }

        public async static Task<CritterStatus> FindByNameAsync(this IQueryable<CritterStatus> source, string name)
        {
            return await source.MatchingName(name).SingleOrDefaultAsync();
        }

        public async static Task<CritterStatus> FindByRescueGroupsIDAsync(this IQueryable<CritterStatus> source, string rescueGroupsID)
        {
            return await source.MatchingRescueGroupsID(rescueGroupsID).SingleOrDefaultAsync();
        }
    }
}