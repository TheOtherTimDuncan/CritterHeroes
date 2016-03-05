using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;
using Newtonsoft.Json.Linq;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Storage
{
    public class BreedSourceStorage : RescueGroupsStorage<BreedSource>
    {
        public BreedSourceStorage(IRescueGroupsConfiguration configuration, IHttpClient client, IAppLogger logger)
            : base(configuration, client, logger)
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

        public override IEnumerable<BreedSource> FromStorage(IEnumerable<JProperty> tokens)
        {
            return tokens.Select(x => new BreedSource(x.Name, x.Value.Value<string>("species"), x.Value.Value<string>("name")));
        }
    }
}
