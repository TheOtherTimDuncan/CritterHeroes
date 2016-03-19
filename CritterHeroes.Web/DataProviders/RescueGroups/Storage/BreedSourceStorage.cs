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
    public class BreedSourceStorage : RescueGroupsStorage<BreedSource>
    {
        public BreedSourceStorage(IRescueGroupsConfiguration configuration, IHttpClient client, IAppEventPublisher publisher)
            : base(configuration, client, publisher)
        {
            this.Fields = new[]
            {
                new SearchField("species"),
                new SearchField("name")
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
                return "name";
            }
        }

        public override IEnumerable<BreedSource> FromStorage(IEnumerable<JProperty> tokens)
        {
            return tokens.Select(x => new BreedSource(x.Name, x.Value.Value<string>("species"), x.Value.Value<string>("name")));
        }
    }
}
