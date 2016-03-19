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
    public class CritterSourceStorage : RescueGroupsStorage<CritterSource>
    {
        public CritterSourceStorage(IRescueGroupsConfiguration configuration, IHttpClient client, IAppEventPublisher publisher)
            : base(configuration, client, publisher)
        {
            ResultLimit = 25;

            this.Fields = new[]
            {
                new SearchField("animalID"),
                new SearchField("animalBirthdate","animalBirthdateExact"),
                new SearchField("animalColorID", "animalColor"),
                new SearchField("animalCourtesy"),
                new SearchField("animalCreatedDate"),
                new SearchField("animalDescription"),
                new SearchField("animalFosterID","fosterEmail","fosterFirstname","fosterLastname"),
                new SearchField("animalGeneralAge"),
                new SearchField("animalGroups"),
                new SearchField("animalInternalID"),
                new SearchField("animalKillDate"),
                new SearchField("animalKillReason"),
                new SearchField("animalLocationID","locationName"),
                new SearchField("animalMicrochipped"),
                new SearchField("animalName"),
                new SearchField("animalNotes"),
                new SearchField("animalOKWithCats"),
                new SearchField("animalOKWithDogs"),
                new SearchField("animalOKWithKids"),
                new SearchField("animalOlderKidsOnly"),
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
                new SearchField("animalUpdatedDate")
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
            get;
        }

        public override bool IsPrivate
        {
            get
            {
                return true;
            }
        }

        public override IEnumerable<CritterSource> FromStorage(IEnumerable<JProperty> tokens)
        {
            return tokens.Select(x => x.Value.ToObject<CritterSource>());
        }
    }
}
