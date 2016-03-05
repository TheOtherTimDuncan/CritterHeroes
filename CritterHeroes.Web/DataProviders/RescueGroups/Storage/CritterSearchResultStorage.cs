using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.Contracts.Events;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;
using Newtonsoft.Json.Linq;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Storage
{
    public class CritterSearchResultStorage : RescueGroupsSearchStorageBase<CritterSearchResult>
    {
        private IEnumerable<SearchField> _fields;

        public CritterSearchResultStorage(IRescueGroupsConfiguration configuration, IHttpClient client, IAppEventPublisher publisher)
            : base(configuration, client, publisher)
        {
            this._fields = new[]
            {
                new SearchField("animalID"),
                new SearchField("animalStatusID"),
                new SearchField("animalStatus"),
                new SearchField("animalName"),
                new SearchField("animalSex"),
                new SearchField("animalPrimaryBreedID"),
                new SearchField("animalPrimaryBreed"),
                new SearchField("animalSpecies"),
                new SearchField("animalFosterID"),
                new SearchField("animalInternalID"),
                new SearchField("animalRescueID"),
                new SearchField("animalCreatedDate"),
                new SearchField("animalUpdatedDate"),
                new SearchField("animalGroups"),
                new SearchField("animalPictures"),
                new SearchField("fosterFirstname"),
                new SearchField("fosterLastname"),
                new SearchField("fosterEmail"),
                new SearchField("animalLocationID"),
                new SearchField("locationName"),
                new SearchField("animalCourtesy"),
                new SearchField("animalDescription"),
                new SearchField("animalGeneralAge"),
                new SearchField("animalSpecialneeds"),
                new SearchField("animalSpecialneedsDescription"),
                new SearchField("animalSpecialDiet"),
                new SearchField("animalReceivedDate")
            };
        }

        public override string ObjectType
        {
            get
            {
                return "animals";
            }
        }

        protected override string SortField
        {
            get
            {
                return "animalID";
            }
        }

        public override IEnumerable<SearchField> Fields
        {
            get
            {
                return _fields;
            }
        }

        public override IEnumerable<CritterSearchResult> FromStorage(IEnumerable<JProperty> tokens)
        {
            return tokens.Select(x => x.Value.ToObject<CritterSearchResult>());
        }
    }
}
