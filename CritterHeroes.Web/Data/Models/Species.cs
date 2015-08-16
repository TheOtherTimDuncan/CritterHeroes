using System;
using System.Collections.Generic;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.Data.Models
{
    public class Species
    {
        protected Species()
        {
        }

        public Species(string name, string singular, string plural, string youngSingular, string youngPlural)
        {
            ThrowIf.Argument.IsNullOrEmpty(name, "name");
            ThrowIf.Argument.IsNullOrEmpty(singular, "singular");
            ThrowIf.Argument.IsNullOrEmpty(plural, "plural");

            this.Name = name;
            this.Singular = singular;
            this.Plural = plural;
            this.YoungPlural = youngPlural;
            this.YoungSingular = youngSingular;
        }

        public int ID
        {
            get;
            private set;
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
