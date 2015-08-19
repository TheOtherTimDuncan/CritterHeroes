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

        public Breed(int id, Species species, string breedName)
            : this(id, breedName)
        {
            ThrowIf.Argument.IsNull(species, nameof(species));

            this.Species = species;
            this.SpeciesID = species.ID;
        }

        public Breed(int id, int speciesID, string breedName)
            : this(id, breedName)
        {
            this.SpeciesID = speciesID;
        }

        protected Breed(int id, string breedName)
        {

            this.ID = id;
            this.BreedName = breedName;
        }

        public int ID
        {
            get;
            private set;
        }

        public int SpeciesID
        {
            get;
            set;
        }

        public virtual Species Species
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
