using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Data.Models;

namespace CritterHeroes.Web.Data.Extensions
{
    public static class BreedExtensions
    {
        public static IQueryable<Breed> MatchingID(this IQueryable<Breed> source, int breedID)
        {
            return source.Where(x => x.ID == breedID);
        }

        public static IQueryable<Breed> MatchingRescueGroupsID(this IQueryable<Breed> source, string rescueGroupsID)
        {
            return source.Where(x => x.RescueGroupsID == rescueGroupsID);
        }

        public static IQueryable<Breed> MatchingSpeciesAndName(this IQueryable<Breed> source, string speciesName, string name)
        {
            return source.Where(x => x.Species.Name == speciesName && x.BreedName == name);
        }

        public async static Task<Breed> FindByIDAsync(this IQueryable<Breed> source, int breedID)
        {
            return await source.MatchingID(breedID).SingleOrDefaultAsync();
        }

        public async static Task<Breed> FindByRescueGroupsIDAsync(this IQueryable<Breed> source, string rescueGroupsID)
        {
            return await source.MatchingRescueGroupsID(rescueGroupsID).SingleOrDefaultAsync();
        }

        public async static Task<Breed> FindBySpeciesAndNameAsync(this IQueryable<Breed> source, string speciesName, string breedName)
        {
            return await source.MatchingSpeciesAndName(speciesName, breedName).SingleOrDefaultAsync();
            ;
        }

    }
}