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
    public class SpeciesSourceStorage : RescueGroupsStorage<SpeciesSource>
    {
        public SpeciesSourceStorage(IRescueGroupsConfiguration configuration, IHttpClient client, IAppEventPublisher publisher)
            : base(configuration, client, publisher)
        {
            this.Fields = new[]
                {
                new SearchField("name"),
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
                return "name";
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
