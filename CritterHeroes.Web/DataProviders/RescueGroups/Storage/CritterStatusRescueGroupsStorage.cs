using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;
using Newtonsoft.Json.Linq;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Storage
{
    public class CritterStatusRescueGroupsStorage : RescueGroupsStorage<CritterStatusSource>
    {
        public CritterStatusRescueGroupsStorage(IRescueGroupsConfiguration configuration)
            : base(configuration)
        {
        }

        public override string ObjectType
        {
            get
            {
                return "animalStatuses";
            }
        }

        public override bool IsPrivate
        {
            get
            {
                return true;
            }
        }

        public override IEnumerable<CritterStatusSource> FromStorage(IEnumerable<JProperty> tokens)
        {
            return
                from t in tokens
                select new CritterStatusSource(t.Name, t.Value.Value<string>("name"), t.Value.Value<string>("description"));
        }
    }
}
