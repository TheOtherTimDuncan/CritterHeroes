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
    public class SpeciesMapper : BaseDataMapper<SpeciesSource, Species>
    {
        public SpeciesMapper(ISqlStorageContext<Species> sqlStorageContext, IStorageContext<SpeciesSource> storageContext, IStateManager<OrganizationContext> orgStorageContext)
            : base(sqlStorageContext, storageContext, orgStorageContext)
        {
        }

        protected override async Task<IEnumerable<string>> GetSourceItems(IStorageContext<SpeciesSource> storageContext)
        {
            IEnumerable<SpeciesSource> sources = await storageContext.GetAllAsync();
            return sources.Select(x => x.Name);
        }

        protected override async Task<IEnumerable<string>> GetTargetItems(ISqlStorageContext<Species> sqlStorageContext)
        {
            IEnumerable<string> result = await sqlStorageContext.Entities.Select(x => x.Name).ToListAsync();
            return result;
        }

        protected override Species CreateTargetFromSource(SpeciesSource source)
        {
            return new Species(source.Name, source.Singular, source.Plural, source.YoungSingular, source.YoungPlural);
        }
    }
}
