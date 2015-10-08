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

        public Breed(Species species, string breedName)
            : this(species, breedName, null)
        {

        }

        public Breed(Species species, string breedName, string rescueGroupsID)
            : this(breedName, rescueGroupsID)
        {
            ThrowIf.Argument.IsNull(species, nameof(species));

            this.Species = species;
            this.SpeciesID = species.ID;

        }

        public Breed(int speciesID, string breedName, string rescueGroupsID)
            : this(breedName, rescueGroupsID)
        {
            this.SpeciesID = speciesID;
        }

        protected Breed(string breedName, string rescueGroupsID)
        {

            this.BreedName = breedName;
            this.RescueGroupsID = rescueGroupsID;
        }

        public int ID
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

        public string BreedName
        {
            get;
            set;
        }

        public string RescueGroupsID
        {
            get;
            set;
        }

        public virtual ICollection<Critter> Critters
        {
            get;
            private set;
        }

        public void ChangeSpecies(int speciesID)
        {
            this.SpeciesID = speciesID;
            this.Species = null;
        }
    }
}
