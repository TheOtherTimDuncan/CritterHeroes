using System;
using System.Collections.Generic;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Models
{
    public class SpeciesSource
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

        public string Name
        {
            get;
            private set;
        }

        public string Singular
        {
            get;
            private set;
        }

        public string Plural
        {
            get;
            private set;
        }

        public string YoungSingular
        {
            get;
            private set;
        }

        public string YoungPlural
        {
            get;
            private set;
        }
    }
}
