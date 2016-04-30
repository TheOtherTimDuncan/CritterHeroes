using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Models
{
    public class SpeciesSource : BaseSource
    {
        public SpeciesSource(string name, string singular, string plural, string youngSingular, string youngPlural)
        {
            ThrowIf.Argument.IsNullOrEmpty(name, nameof(name));
            ThrowIf.Argument.IsNullOrEmpty(singular, nameof(singular));
            ThrowIf.Argument.IsNullOrEmpty(plural, nameof(plural));

            this.Name = name;
            this.Singular = singular;
            this.Plural = plural;
            this.YoungPlural = youngPlural;
            this.YoungSingular = youngSingular;
        }

        public override int ID
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        [JsonProperty(PropertyName = "speciesID")]
        public string Name
        {
            get;
            private set;
        }

        [JsonProperty(PropertyName = "speciesSingular")]
        public string Singular
        {
            get;
            private set;
        }

        [JsonProperty(PropertyName = "speciesPlural")]
        public string Plural
        {
            get;
            private set;
        }

        [JsonProperty(PropertyName = "speciesSingularYoung")]
        public string YoungSingular
        {
            get;
            private set;
        }

        [JsonProperty(PropertyName = "speciesPluralYoung")]
        public string YoungPlural
        {
            get;
            private set;
        }
    }
}
