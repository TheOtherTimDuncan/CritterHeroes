using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AR.Domain.Models.Data;
using Newtonsoft.Json.Linq;

namespace AR.RescueGroups.Mappings
{
    public class AnimalStatusMapping : BaseRescueGroupsMapping<AnimalStatus>
    {
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

        public override IEnumerable<AnimalStatus> ToModel(IEnumerable<JToken> values)
        {
            return
                from v in values
                select new AnimalStatus(v.Value<string>("name"), v.Value<string>("description"));
        }
    }
}
