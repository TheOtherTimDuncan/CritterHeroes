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
            ResultLimit = 25;

            this._fields = new[]
            {
                new SearchField("animalID"),

                new SearchField("animalBirthdate","animalBirthdateExact"),
                new SearchField("animalCourtesy"),
                new SearchField("animalCreatedDate"),
                new SearchField("animalDescription"),
                new SearchField("animalFosterID","fosterEmail","fosterFirstname","fosterLastname"),
                new SearchField("animalGeneralAge"),
                new SearchField("animalGroups"),
                new SearchField("animalInternalID"),
                new SearchField("animalLocationID","locationName"),
                new SearchField("animalName"),
                new SearchField("animalPictures"),
                new SearchField("animalPrimaryBreedID","animalPrimaryBreed"),
                new SearchField("animalReceivedDate"),
                new SearchField("animalRescueID"),
                new SearchField("animalSex"),
                new SearchField("animalSpecialDiet"),
                new SearchField("animalSpecialneeds"),
                new SearchField("animalSpecialneedsDescription"),
                new SearchField("animalSpecies"),
                new SearchField("animalStatusID","animalStatus"),
                new SearchField("animalUpdatedDate"),
                new SearchField("animalColorID", "animalColor")
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
