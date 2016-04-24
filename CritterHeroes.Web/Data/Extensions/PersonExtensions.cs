using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using CritterHeroes.Web.Data.Models;
using System.Threading.Tasks;

namespace CritterHeroes.Web.Data.Extensions
{
    public static class PersonExtensions
    {
        public static IQueryable<Person> MatchingID(this IQueryable<Person> source, int personID)
        {
            return source.Where(x => x.ID == personID);
        }

        public static IQueryable<Person> MatchingRescueGroupsID(this IQueryable<Person> source, int rescueGroupsID)
        {
            return source.Where(x => x.RescueGroupsID == rescueGroupsID);
        }

        public async static Task<Person> FindByIDAsync(this IQueryable<Person> source, int personID)
        {
            return await source.MatchingID(personID).SingleOrDefaultAsync();
        }

        public async static Task<Person> FindByRescueGroupsIDAsync(this IQueryable<Person> source, int rescueGroupsID)
        {
            return await source.MatchingRescueGroupsID(rescueGroupsID).SingleOrDefaultAsync();
        }
    }
}
