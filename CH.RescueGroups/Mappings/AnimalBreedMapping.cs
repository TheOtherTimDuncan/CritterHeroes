using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CH.Domain.Models.Data;
using Newtonsoft.Json.Linq;

namespace CH.RescueGroups.Mappings
{
    public class AnimalBreedMapping : BaseRescueGroupsMapping<AnimalBreed>
    {
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

        public override IEnumerable<AnimalBreed> ToModel(IEnumerable<JProperty> tokens)
        {
            return
                from t in tokens
                select new AnimalBreed(t.Name, t.Value.Value<string>("species"), t.Value.Value<string>("name"));
        }
    }
}
