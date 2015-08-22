using System;
using System.Collections.Generic;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.Data.Models
{
    public class OrganizationSupportedCritter
    {
        protected OrganizationSupportedCritter()
        {
        }

        public OrganizationSupportedCritter(Guid organizationID, Species species)
        {
            ThrowIf.Argument.IsNull(species, nameof(species));

            this.OrganizationID = organizationID;

            this.SpeciesID = species.ID;
            this.Species = species;
        }

        public int ID
        {
            get;
            private set;
        }

        public Guid OrganizationID
        {
            get;
            private set;
        }

        public int SpeciesID
        {
            get;
            private set;
        }

        public virtual Species Species
        {
            get;
            private set;
        }
    }
}
