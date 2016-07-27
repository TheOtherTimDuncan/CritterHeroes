using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Contexts;
using CritterHeroes.Web.Data.Models;
using TOTD.Utility.EnumerableHelpers;

namespace CH.RescueGroupsHelper.Importer
{
    public class BaseContactImporter
    {
        protected async Task<IEnumerable<PhoneType>> GetPhoneTypesAsync()
        {
            using (SqlStorageContext<PhoneType> storageContext = new SqlStorageContext<PhoneType>(new NullEventPublisher()))
            {
                return await storageContext.GetAllAsync();
            }
        }

        protected async Task AddOrUpdateGroupsAsync(ISqlStorageContext<Group> storageGroups, IEnumerable<string> groupNames)
        {
            if (!groupNames.IsNullOrEmpty())
            {
                foreach (string groupName in groupNames)
                {
                    Group group = await storageGroups.Entities.Where(x => x.Name == groupName).SingleOrDefaultAsync();
                    if (group == null)
                    {
                        group = new Group(groupName);
                        storageGroups.Add(group);
                    }
                    await storageGroups.SaveChangesAsync();
                }
            }
        }
    }
}
