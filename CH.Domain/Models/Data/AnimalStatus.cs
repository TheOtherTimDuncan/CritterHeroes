using System;
using System.Collections.Generic;
using TOTD.Utility.ExceptionHelpers;

namespace CH.Domain.Models.Data
{
    public class AnimalStatus
    {
        public AnimalStatus(string id, string name, string description)
        {
            ThrowIf.Argument.IsNullOrEmpty(id, "id");
            ThrowIf.Argument.IsNullOrEmpty(name, "name");

            this.ID = id;
            this.Name = name;
            this.Description = description;
        }

        public string ID
        {
            get;
            private set;
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
            return this.ID.GetHashCode();
        }

        public bool Equals(AnimalStatus other)
        {
            if (other == null)
            {
                return false;
            }

            return this.ID == other.ID;
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
