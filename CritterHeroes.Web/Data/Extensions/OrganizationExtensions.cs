using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Models;

namespace CritterHeroes.Web.Data.Extensions
{
    public static class OrganizationExtensions
    {
        public static Organization FindByID(this ISqlStorageContext<Organization> storageContext, Guid organizationID)
        {
            return storageContext.Get(x => x.ID == organizationID);
        }

        public async static Task<Organization> FindByIDAsync(this ISqlStorageContext<Organization> storageContext, Guid organizationID)
        {
            return await storageContext.GetAsync(x => x.ID == organizationID);
        }
    }
}