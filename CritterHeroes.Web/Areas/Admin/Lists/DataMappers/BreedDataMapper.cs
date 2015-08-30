using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using TOTD.Utility.StringHelpers;

namespace CritterHeroes.Web.Areas.Admin.Lists.DataMappers
{
    public class BreedDataMapper : BaseDataMapper<BreedSource, Breed>
    {
        private ISqlStorageContext<Species> _speciesStorage;

        public BreedDataMapper(ISqlStorageContext<Breed> sqlStorageContext, IRescueGroupsStorageContext<BreedSource> storageContext, IStateManager<OrganizationContext> orgStorageContext, ISqlStorageContext<Species> speciesStorageContext)
            : base(sqlStorageContext, storageContext, orgStorageContext)
        {
            this._speciesStorage = speciesStorageContext;
        }

        public override async Task<CommandResult> CopySourceToTarget()
        {
            IEnumerable<BreedSource> sources = await SourceStorageContext.GetAllAsync();

            // First remove any in the target that don't exist in the source
            IEnumerable<Breed> targets = await TargetStorageContext.GetAllAsync();
            foreach (Breed breed in targets)
            {
                if (breed.Critters.IsNullOrEmpty() && !sources.Any(x => x.ID == breed.RescueGroupsID))
                {
                    TargetStorageContext.Delete(breed);
                }
            }

            // Add any new breeds from the source or update existing ones to match
            foreach (BreedSource source in sources)
            {
                Breed breed = await TargetStorageContext.Entities.FindBySpeciesAndNameAsync(source.Species, source.BreedName);

                if (breed != null)
                {
                    breed.RescueGroupsID = source.ID;

                    if (breed.Species == null || !breed.Species.Name.SafeEquals(source.Species))
                    {
                        Species species = GetOrCreateSpecies(source.Species);
                        breed.ChangeSpecies(species.ID);
                    }
                }
                else
                {
                    breed = CreateTargetFromSource(source);
                    TargetStorageContext.Add(breed);
                }
            }

            await TargetStorageContext.SaveChangesAsync();

            return CommandResult.Success();
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
                select x.Species.Name + (x.BreedName != null ? " - " + x.BreedName : "")
            ).ToListAsync();
            return result;
        }

        protected override Breed CreateTargetFromSource(BreedSource source)
        {
            Species species = GetOrCreateSpecies(source.Species);
            return new Breed(species.ID, source.BreedName, source.ID);
        }

        private Species GetOrCreateSpecies(string speciesName)
        {
            Species species = _speciesStorage.Entities.FindByName(speciesName);
            if (species == null)
            {
                species = new Species(speciesName, speciesName, speciesName, null, null);
                _speciesStorage.Add(species);
                _speciesStorage.SaveChanges();
            }
            return species;
        }
    }
}
