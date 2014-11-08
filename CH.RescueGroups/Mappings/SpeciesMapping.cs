using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CH.Domain.Models.Data;
using Newtonsoft.Json.Linq;

namespace CH.RescueGroups.Mappings
{
    public class SpeciesMapping : BaseRescueGroupsMapping<Species>
    {
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

        public override IEnumerable<Species> ToModel(IEnumerable<JProperty> tokens)
        {
            return
                from t in tokens
                select new Species(t.Name, t.Value.Value<string>("speciesSingular"), t.Value.Value<string>("speciesPlural"), t.Value.Value<string>("speciesSingularYoung"), t.Value.Value<string>("speciesPluralYoung"));
        }
    }
}
