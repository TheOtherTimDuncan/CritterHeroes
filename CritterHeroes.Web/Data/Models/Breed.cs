using System;
using System.Collections.Generic;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.Data.Models
{
    public class Breed : BaseDataItem<Breed>
    {
        protected Breed()
        {
        }

        public Breed(string id, string species, string breedName)
        {
            ThrowIf.Argument.IsNullOrEmpty(id, "id");
            ThrowIf.Argument.IsNullOrEmpty(species, "species");

            this.ID = int.Parse(id);
            this.Species = species;
            this.BreedName = breedName;
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

        public override int GetHashCode()
        {
            return this.ID.GetHashCode();
        }

        public override bool Equals(Breed other)
        {
            if (other == null)
            {
                return false;
            }

            return this.ID == other.ID;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Breed);
        }

        public static bool operator ==(Breed animalBreed1, Breed animalBreed2)
        {
            return Object.Equals(animalBreed1, animalBreed2);
        }

        public static bool operator !=(Breed animalBreed1, Breed animalBreed2)
        {
            return !(animalBreed1 == animalBreed2);
        }
    }
}
