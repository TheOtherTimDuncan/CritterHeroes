using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Models;

namespace CritterHeroes.Web.Data.Extensions
{
    public static class CritterStatusExtensions
    {
        public static CritterStatus FindByID(this ISqlStorageContext<CritterStatus> storageContext, int statusID)
        {
            return storageContext.Get(x => x.ID == statusID);
        }

        public async static Task<CritterStatus> FindByIDAsync(this ISqlStorageContext<CritterStatus> storageContext, int statusID)
        {
            return await storageContext.GetAsync(x => x.ID == statusID);
        }
    }
}