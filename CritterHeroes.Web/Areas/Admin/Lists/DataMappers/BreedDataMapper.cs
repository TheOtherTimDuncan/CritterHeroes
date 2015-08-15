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
    public class BreedDataMapper : BaseDataMapper<BreedSource, Breed>
    {
        public BreedDataMapper(ISqlStorageContext<Breed> sqlStorageContext, IStorageContext<BreedSource> storageContext, IStateManager<OrganizationContext> orgStorageContext)
            : base(sqlStorageContext, storageContext, orgStorageContext)
        {
        }

        protected override async Task<IEnumerable<string>> GetSourceItems(IStorageContext<BreedSource> storageContext)
        {
            IEnumerable<BreedSource> sources = await storageContext.GetAllAsync();
            return sources.Select(x =>
            {
                string result = x.Species;
                if (x.BreedName != null)
                {
                    result += " - " + x.BreedName;
                }
                return result;
            });
        }

        protected override async Task<IEnumerable<string>> GetTargetItems(ISqlStorageContext<Breed> sqlStorageContext)
        {
            IEnumerable<string> result = await
            (
                from x in sqlStorageContext.Entities
                select x.Species + (x.BreedName != null ? " - " + x.BreedName : "")
            ).ToListAsync();
            return result;
        }
    }
}
