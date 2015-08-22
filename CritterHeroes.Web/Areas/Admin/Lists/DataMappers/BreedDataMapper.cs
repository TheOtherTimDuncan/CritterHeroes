using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Extensions;
using CritterHeroes.Web.Data.Models;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;

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
            Species species = _speciesStorage.FindByName(source.Species);
            if (species == null)
            {
                species = new Species(source.Species, source.Species, source.Species, null, null);
                _speciesStorage.Add(species);
                _speciesStorage.SaveChanges();
            }

            return new Breed(int.Parse(source.ID), species.ID, source.BreedName);
        }
    }
}
