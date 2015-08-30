using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;
using Newtonsoft.Json.Linq;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Storage
{
    public class CritterSearchResultStorage : RescueGroupsStorage<CritterSearchResult>
    {
        public CritterSearchResultStorage(IRescueGroupsConfiguration configuration)
            : base(configuration)
        {
        }

        public override string ObjectType
        {
            get
            {
                return "animals";
            }
        }

        public override string ObjectAction
        {
            get
            {
                return "publicSearch";
            }
        }

        public override bool IsPrivate
        {
            get
            {
                return true;
            }
        }

        public override IEnumerable<CritterSearchResult> FromStorage(IEnumerable<JProperty> tokens)
        {
            return tokens.Select(x => new CritterSearchResult()
            {
                ID = x.Value.Value<int>("animalID"),
                Name = x.Value.Value<string>("animalName")
            });
        }

        public override JObject CreateRequest(params JProperty[] requestProperties)
        {
            JObject request = base.CreateRequest(requestProperties);
            return request;
        }
    }
}
