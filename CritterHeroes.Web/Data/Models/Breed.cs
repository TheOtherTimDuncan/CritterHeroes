using System;
using System.Collections.Generic;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.Data.Models
{
    public class Breed
    {
        protected Breed()
        {
        }

        public Breed(int id, string species, string breedName)
        {
            ThrowIf.Argument.IsNullOrEmpty(species, "species");

            this.ID = id;
            this.Species = species;
            this.BreedName = breedName;
        }

        public int ID
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
