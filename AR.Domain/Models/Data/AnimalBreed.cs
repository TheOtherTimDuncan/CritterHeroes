using System;
using System.Collections.Generic;
using TOTD.Utility.ExceptionHelpers;

namespace AR.Domain.Models.Data
{
    public class AnimalBreed
    {
        public AnimalBreed(string id, string species, string breedName)
        {
            ThrowIf.Argument.IsNullOrEmpty(id, "id");
            ThrowIf.Argument.IsNullOrEmpty(species, "species");

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

        public override int GetHashCode()
        {
            return this.ID.GetHashCode();
        }

        public bool Equals(AnimalBreed other)
        {
            if (other == null)
            {
                return false;
            }

            return this.ID == other.ID;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as AnimalBreed);
        }

        public static bool operator ==(AnimalBreed animalBreed1, AnimalBreed animalBreed2)
        {
            return Object.Equals(animalBreed1, animalBreed2);
        }

        public static bool operator !=(AnimalBreed animalBreed1, AnimalBreed animalBreed2)
        {
            return !(animalBreed1 == animalBreed2);
        }
    }
}
