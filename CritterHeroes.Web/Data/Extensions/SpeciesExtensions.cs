using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Data.Models;

namespace CritterHeroes.Web.Data.Extensions
{
    public static class SpeciesExtensions
    {
        public static IQueryable<Species> MatchingName(this IQueryable<Species> source, string name)
        {
            return source.Where(x => x.Name == name);
        }

        public static Species FindByName(this IQueryable<Species> source, string name)
        {
            return source.MatchingName(name).SingleOrDefault();
        }

        public async static Task<Species> FindByNameAsync(this IQueryable<Species> source, string name)
        {
            return await source.MatchingName(name).SingleOrDefaultAsync();
        }
    }
}