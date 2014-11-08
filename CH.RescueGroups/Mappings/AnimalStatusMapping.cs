using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CH.Domain.Models.Data;
using Newtonsoft.Json.Linq;

namespace CH.RescueGroups.Mappings
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

        public override IEnumerable<AnimalStatus> ToModel(IEnumerable<JProperty> tokens)
        {
            return
                from t in tokens
                select new AnimalStatus(t.Name, t.Value.Value<string>("name"), t.Value.Value<string>("description"));
        }
    }
}
