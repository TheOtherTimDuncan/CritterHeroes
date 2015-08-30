using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;
using Newtonsoft.Json.Linq;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Storage
{
    public class SpeciesSourceStorage : RescueGroupsStorage<SpeciesSource>
    {
        public SpeciesSourceStorage(IRescueGroupsConfiguration configuration, IHttpClient client)
            : base(configuration, client)
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

        public override IEnumerable<SpeciesSource> FromStorage(IEnumerable<JProperty> tokens)
        {
            return tokens.Select(x => new SpeciesSource(
                x.Name,
                x.Value.Value<string>("speciesSingular"),
                x.Value.Value<string>("speciesPlural"),
                x.Value.Value<string>("speciesSingularYoung"),
                x.Value.Value<string>("speciesPluralYoung"))
            );
        }
    }
}
