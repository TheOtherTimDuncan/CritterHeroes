using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.Data.Models;
using Newtonsoft.Json.Linq;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Storage
{
    public class BreedRescueGroupsStorage : RescueGroupsStorage<Breed>
    {
        public BreedRescueGroupsStorage(IRescueGroupsConfiguration configuration)
            : base(configuration)
        {
        }

        public override string ObjectType
        {
            get
            {
                return "animalBreeds";
            }
        }

        public override bool IsPrivate
        {
            get
            {
                return true;
            }
        }

        public override IEnumerable<Breed> FromStorage(IEnumerable<JProperty> tokens)
        {
            return
                from t in tokens
                select new Breed(t.Name, t.Value.Value<string>("species"), t.Value.Value<string>("name"));
        }
    }
}
