using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Models;

namespace CritterHeroes.Web.Data.Extensions
{
    public static class BreedExtensions
    {
        public static Breed FindByID(this ISqlStorageContext<Breed> storageContext, int breedID)
        {
            return storageContext.Get(x => x.ID == breedID);
        }

        public async static Task<Breed> FindByIDAsync(this ISqlStorageContext<Breed> storageContext, int breedID)
        {
            return await storageContext.GetAsync(x => x.ID == breedID);
        }
    }
}