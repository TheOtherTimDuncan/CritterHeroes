using System;
using System.Collections.Generic;
using System.Linq;
using CH.Domain.Contracts.Configuration;
using CH.Domain.Models.Data;
using Newtonsoft.Json.Linq;

namespace CH.RescueGroups.Storage
{
    public class AnimalStatusRescueGroupsStorage : RescueGroupsStorage<AnimalStatus>
    {
        public AnimalStatusRescueGroupsStorage(IRescueGroupsConfiguration configuration)
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

        public override IEnumerable<AnimalStatus> FromStorage(IEnumerable<JProperty> tokens)
        {
            return
                from t in tokens
                select new AnimalStatus(t.Name, t.Value.Value<string>("name"), t.Value.Value<string>("description"));
        }
    }
}
