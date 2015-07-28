using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Models;

namespace CritterHeroes.Web.Data.Extensions
{
    public static class AnimalStatusExtensions
    {
        public static AnimalStatus FindByID(this ISqlStorageContext<AnimalStatus> storageContext, int statusID)
        {
            return storageContext.Get(x => x.ID == statusID);
        }

        public async static Task<AnimalStatus> FindByIDAsync(this ISqlStorageContext<AnimalStatus> storageContext, int statusID)
        {
            return await storageContext.GetAsync(x => x.ID == statusID);
        }
    }
}