using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Domain.Contracts;
using CritterHeroes.Web.Domain.Contracts.Configuration;
using CritterHeroes.Web.Domain.Contracts.Events;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Storage
{
    public class BreedSourceStorage : RescueGroupsStorage<BreedSource>
    {
        public BreedSourceStorage(IRescueGroupsConfiguration configuration, IHttpClient client, IAppEventPublisher publisher)
            : base(configuration, client, publisher)
        {
            this.Fields = new[]
            {
                new SearchField("breedID"),
                new SearchField("breedName"),
                new SearchField("breedSpecies")
            };
        }

        public override string ObjectType
        {
            get
            {
                return "animalBreeds";
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
                return "breedID";
            }
        }

        protected override string KeyField
        {
            get
            {
                return "breedID";
            }
        }

        public override async Task<IEnumerable<BreedSource>> GetAllAsync(params SearchFilter[] searchFilters)
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
