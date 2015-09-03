using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using CritterHeroes.Web.Data.Models;
using System.Threading.Tasks;

namespace CritterHeroes.Web.Data.Extensions
{
    public static class GroupExtensions
    {
        public static IQueryable<Group> MatchingName(this IQueryable<Group> source, string name)
        {
            return source.Where(x => x.Name == name);
        }

        public async static Task<Group> FindByNameAsync(this IQueryable<Group> source, string name)
        {
            return await source.MatchingName(name).SingleOrDefaultAsync();
        }
    }
}
