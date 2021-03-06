﻿using System;
using System.Collections.Generic;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.Data.Models
{
    public class Species
    {
        protected Species()
        {
        }

        public Species(string name, string singular, string plural, string youngSingular = null, string youngPlural = null)
        {
            ThrowIf.Argument.IsNullOrEmpty(name, nameof(name));
            ThrowIf.Argument.IsNullOrEmpty(singular, nameof(singular));
            ThrowIf.Argument.IsNullOrEmpty(plural, nameof(plural));

            this.Name = name;
            this.Singular = singular;
            this.Plural = plural;
            this.YoungPlural = youngPlural;
            this.YoungSingular = youngSingular;

            this.Breeds = new HashSet<Breed>();
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
            set;
        }

        public string Plural
        {
            get;
            set;
        }

        public string YoungSingular
        {
            get;
            set;
        }

        public string YoungPlural
        {
            get;
            set;
        }

        public virtual ICollection<Breed> Breeds
        {
            get;
            private set;
        }

        public virtual ICollection<OrganizationSupportedCritter> OrganizationSupportedCritters
        {
            get;
            set;
        }
    }
}
