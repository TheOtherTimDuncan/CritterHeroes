using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Models;

namespace CritterHeroes.Web.Data.Extensions
{
    public static class CritterExtensions
    {
        public static Critter FindByID(this ISqlStorageContext<Critter> storageContext, int critterID)
        {
            return storageContext.Get(x => x.ID == critterID);
        }

        public async static Task<Critter> FindByIDAsync(this ISqlStorageContext<Critter> storageContext, int critterID)
        {
            return await storageContext.GetAsync(x => x.ID == critterID);
        }

        public static Critter FindByRescueGroupsID(this ISqlStorageContext<Critter> storageContext, int rescueGroupsID)
        {
            return storageContext.Get(x => x.RescueGroupsID == rescueGroupsID);
        }

        public async static Task<Critter> FindByRescueGroupsIDAsync(this ISqlStorageContext<Critter> storageContext, int rescueGroupsID)
        {
            return await storageContext.GetAsync(x => x.RescueGroupsID == rescueGroupsID);
        }
    }
}
