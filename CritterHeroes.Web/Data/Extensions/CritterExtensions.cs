using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Models;

namespace CritterHeroes.Web.Data.Extensions
{
    public static class CritterExtensions
    {
        public static IQueryable<Critter> MatchingID(this IQueryable<Critter> source, int critterID)
        {
            return source.Where(x => x.ID == critterID);
        }

        public static IQueryable<Critter> MatchingRescueGroupsID(this IQueryable<Critter> source, int rescueGroupsID)
        {
            return source.Where(x => x.RescueGroupsID == rescueGroupsID);
        }

        public static Critter FindByID(this IQueryable<Critter> storageContext, int critterID)
        {
            return storageContext.MatchingID(critterID).SingleOrDefault();
        }

        public async static Task<Critter> FindByIDAsync(this IQueryable<Critter> source, int critterID)
        {
            return await source.MatchingID(critterID).SingleOrDefaultAsync();
        }

        public async static Task<Critter> FindByRescueGroupsIDAsync(this IQueryable<Critter> source, int rescueGroupsID)
        {
            return await source.MatchingRescueGroupsID(rescueGroupsID).SingleOrDefaultAsync();
        }
    }
}
