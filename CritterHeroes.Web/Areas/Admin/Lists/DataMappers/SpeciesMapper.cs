using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Extensions;
using CritterHeroes.Web.Data.Models;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;
using TOTD.Utility.EnumerableHelpers;

namespace CritterHeroes.Web.Areas.Admin.Lists.DataMappers
{
    public class SpeciesMapper : BaseDataMapper<SpeciesSource, Species>
    {
        public SpeciesMapper(ISqlStorageContext<Species> sqlStorageContext, IRescueGroupsStorageContext<SpeciesSource> storageContext, IStateManager<OrganizationContext> orgStorageContext)
            : base(sqlStorageContext, storageContext, orgStorageContext)
        {
        }

        public override async Task<CommandResult> CopySourceToTarget()
        {
            IEnumerable<SpeciesSource> sources = await SourceStorageContext.GetAllAsync();

            // First remove any in the target that don't exist in the source
            // as long as nothing in OrganizationSupportedCritters is linked
            IEnumerable<Species> targets = await TargetStorageContext.GetAllAsync();
            foreach (Species species in targets)
            {
                if (species.OrganizationSupportedCritters.IsNullOrEmpty() && !sources.Any(x => x.Name == species.Name))
                {
                    TargetStorageContext.Delete(species);
                }
            }

            // Add any new species from the source or update existing ones to match
            foreach (SpeciesSource source in sources)
            {
                Species species = await TargetStorageContext.Entities.FindByNameAsync(source.Name);
                if (species != null)
                {
                    species.Singular = source.Singular;
                    species.Plural = source.Plural;
                    species.YoungSingular = source.YoungSingular;
                    species.YoungPlural = source.YoungPlural;
                }
                else
                {
                    species = CreateTargetFromSource(source);
                    TargetStorageContext.Add(species);
                }
            }

            await TargetStorageContext.SaveChangesAsync();

            return CommandResult.Success();
        }

        protected override async Task<IEnumerable<string>> GetSourceItems(IStorageContext<SpeciesSource> storageContext)
        {
            IEnumerable<SpeciesSource> sources = await storageContext.GetAllAsync();
            return sources.Select(x => x.Name);
        }

        protected override async Task<IEnumerable<string>> GetTargetItems(ISqlStorageContext<Species> sqlStorageContext)
        {
            IEnumerable<string> result = await sqlStorageContext.Entities.SelectToListAsync(x => x.Name);
            return result;
        }

        protected override Species CreateTargetFromSource(SpeciesSource source)
        {
            return new Species(source.Name, source.Singular, source.Plural, EmptyToNull(source.YoungSingular), EmptyToNull(source.YoungPlural));
        }
    }
}
