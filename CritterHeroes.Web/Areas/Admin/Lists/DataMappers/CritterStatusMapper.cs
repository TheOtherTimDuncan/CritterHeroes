using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Models;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;

namespace CritterHeroes.Web.Areas.Admin.Lists.DataMappers
{
    public class CritterStatusMapper : BaseDataMapper<CritterStatusSource, AnimalStatus>
    {
        public CritterStatusMapper(ISqlStorageContext<AnimalStatus> sqlStorageContext, IRescueGroupsStorageContext<CritterStatusSource> storageContext, IStateManager<OrganizationContext> orgStorageContext)
            : base(sqlStorageContext, storageContext, orgStorageContext)
        {
        }

        protected override async Task<IEnumerable<string>> GetSourceItems(IStorageContext<CritterStatusSource> storageContext)
        {
            IEnumerable<CritterStatusSource> sources = await storageContext.GetAllAsync();
            return sources.Select(x => x.Name);
        }

        protected override async Task<IEnumerable<string>> GetTargetItems(ISqlStorageContext<AnimalStatus> sqlStorageContext)
        {
            IEnumerable<string> result = await sqlStorageContext.Entities.Select(x => x.Name).ToListAsync();
            return result;
        }

        protected override AnimalStatus CreateTargetFromSource(CritterStatusSource source)
        {
            return new AnimalStatus(int.Parse(source.ID), source.Name, source.Description);
        }
    }
}
