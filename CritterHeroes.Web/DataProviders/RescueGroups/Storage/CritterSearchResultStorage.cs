using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;
using Newtonsoft.Json.Linq;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Storage
{
    public class CritterSearchResultStorage : RescueGroupsSearchStorageBase<CritterSearchResult>
    {
        private IEnumerable<string> _fields;

        public CritterSearchResultStorage(IRescueGroupsConfiguration configuration, IHttpClient client)
            : base(configuration, client)
        {
            this._fields = new[]
            {
                "animalID",
                "animalStatusID",
                "animalStatus",
                "animalName",
                "animalSex",
                "animalPrimaryBreedID",
                "animalPrimaryBreed",
                "animalSpecies",
                "animalFosterID",
                "animalInternalID",
                "animalRescueID",
                "animalUpdatedDate",
                "animalGroups",
                "animalPictures",
                "fosterFirstname",
                "fosterLastname",
                "fosterEmail",
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

        protected override IEnumerable<string> Fields
        {
            get
            {
                return _fields;
            }
        }

        public override IEnumerable<CritterSearchResult> FromStorage(IEnumerable<JProperty> tokens)
        {
            return tokens.Select(x => new CritterSearchResult()
            {
                ID = x.Value.Value<int>("animalID"),
                Name = x.Value.Value<string>("animalName"),
                Sex = x.Value.Value<string>("animalSex"),
                StatusID = x.Value.Value<string>("animalStatusID"),
                Status = x.Value.Value<string>("animalStatus"),
                PrimaryBreedID = x.Value.Value<string>("animalPrimaryBreedID"),
                PrimaryBreed = x.Value.Value<string>("animalPrimaryBreed"),
                Species = x.Value.Value<string>("animalSpecies"),
                FosterContactID = x.Value.Value<string>("animalFosterID"),
                FosterFirstName = x.Value.Value<string>("fosterFirstname"),
                FosterLastName = x.Value.Value<string>("fosterLastname"),
                FosterEmail = x.Value.Value<string>("fosterEmail"),
                RescueID = x.Value.Value<string>("animalRescueID"),
                LastUpdated = x.Value.Value<string>("animalUpdatedDate")
            });
        }
    }
}
