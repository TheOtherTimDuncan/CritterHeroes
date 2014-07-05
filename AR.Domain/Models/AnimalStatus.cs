using System;
using System.Collections.Generic;
using TOTD.Utility.StringHelpers;

namespace AR.Domain.Models
{
    public class AnimalStatus
    {
        public AnimalStatus(string name, string description)
        {
            if (name.IsNullOrEmpty())
            {
                throw new ArgumentNullException("name");
            }

            this.Name = name;
            this.Description = description;
        }

        public string Name
        {
            get;
            private set;
        }

        public string Description
        {
            get;
            private set;
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

        public bool Equals(AnimalStatus other)
        {
            if (other == null)
            {
                return false;
            }

            return this.Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as AnimalStatus);
        }

        public static bool operator ==(AnimalStatus animalStatus1, AnimalStatus animalStatus2)
        {
            return Object.Equals(animalStatus1, animalStatus2);
        }

        public static bool operator !=(AnimalStatus animalStatus1, AnimalStatus animalStatus2)
        {
            return !(animalStatus1 == animalStatus2);
        }
    }
}
