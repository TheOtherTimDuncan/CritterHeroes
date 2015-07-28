using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Models;

namespace CritterHeroes.Web.Data.Extensions
{
    public static class SpeciesExtensions
    {
        public static Species FindByName(this ISqlStorageContext<Species> storageContext, string name)
        {
            return storageContext.Get(x => x.Name == name);
        }

        public async static Task<Species> FindByNameAsync(this ISqlStorageContext<Species> storageContext, string name)
        {
            return await storageContext.GetAsync(x => x.Name == name);
        }
    }
}