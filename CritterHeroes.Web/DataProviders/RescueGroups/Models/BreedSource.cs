using System;
using System.Collections.Generic;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Models
{
    public class BreedSource
    {
        public BreedSource(string id, string species, string breedName)
        {
            ThrowIf.Argument.IsNullOrEmpty(id, nameof(id));
            ThrowIf.Argument.IsNullOrEmpty(species, nameof(species));

            this.ID = id;
            this.Species = species;
            this.BreedName = breedName;
        }

        public string ID
        {
            get;
            private set;
        }

        public string Species
        {
            get;
            private set;
        }

        public string BreedName
        {
            get;
            private set;
        }
    }
}
