using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.Contracts.Events;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Storage
{
    public class SpeciesSourceStorage : RescueGroupsStorage<SpeciesSource>
    {
        public SpeciesSourceStorage(IRescueGroupsConfiguration configuration, IHttpClient client, IAppEventPublisher publisher)
            : base(configuration, client, publisher)
        {
            this.Fields = new[]
                {
                new SearchField("speciesID"),
                new SearchField("speciesSingular"),
                new SearchField("speciesPlural"),
                new SearchField("speciesSingularYoung"),
                new SearchField("speciesPluralYoung")
            };
        }

        public override string ObjectType
        {
            get
            {
                return "animalSpecies";
            }
        }

        public override IEnumerable<SearchField> Fields
        {
            get;
        }

        public override bool IsPrivate
        {
            get
            {
                return true;
            }
        }

        protected override string SortField
        {
            get
            {
                return "speciesID";
            }
        }

        protected override string KeyField
        {
            get
            {
                return "speciesID";
            }
        }

        public override async Task<IEnumerable<SpeciesSource>> GetAllAsync()
        {
            SearchFilter filter = new SearchFilter()
            {
                FieldName = KeyField,
                Operation = SearchFilterOperation.NotBlank
            };
            return await base.GetAllAsync(filter);
        }
    }
}
