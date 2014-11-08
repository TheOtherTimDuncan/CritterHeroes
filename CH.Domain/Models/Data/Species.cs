using System;
using System.Collections.Generic;
using TOTD.Utility.ExceptionHelpers;

namespace CH.Domain.Models.Data
{
    public class Species
    {
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

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

        public bool Equals(Species other)
        {
            if (other == null)
            {
                return false;
            }

            return this.Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Species);
        }

        public static bool operator ==(Species species1, Species species2)
        {
            return Object.Equals(species1, species2);
        }

        public static bool operator !=(Species species1, Species species2)
        {
            return !(species1 == species2);
        }
    }
}
