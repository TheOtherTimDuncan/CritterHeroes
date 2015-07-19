using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.Data.Models;
using Newtonsoft.Json.Linq;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Storage
{
    public class SpeciesRescueGroupsStorage : RescueGroupsStorage<Species>
    {
        public SpeciesRescueGroupsStorage(IRescueGroupsConfiguration configuration)
            : base(configuration)
        {
        }

        public override string ObjectType
        {
            get
            {
                return "animalSpecies";
            }
        }

        public override bool IsPrivate
        {
            get
            {
                return true;
            }
        }

        public override IEnumerable<Species> FromStorage(IEnumerable<JProperty> tokens)
        {
            return
                from t in tokens
                select new Species(t.Name, t.Value.Value<string>("speciesSingular"), t.Value.Value<string>("speciesPlural"), t.Value.Value<string>("speciesSingularYoung"), t.Value.Value<string>("speciesPluralYoung"));
        }
    }
}
