﻿using System;
using System.Collections.Generic;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.Data.Models
{
    public class AnimalStatus : BaseDataItem<AnimalStatus>
    {
        protected AnimalStatus()
        {
        }

        public AnimalStatus(string id, string name, string description)
        {
            ThrowIf.Argument.IsNullOrEmpty(id, "id");
            ThrowIf.Argument.IsNullOrEmpty(name, "name");

            this.ID = int.Parse(id);
            this.Name = name;
            this.Description = description;
        }

        public AnimalStatus(int id, string name, string description)
        {
            ThrowIf.Argument.IsNullOrEmpty(name, "name");

            this.ID = id;
            this.Name = name;
            this.Description = description;
        }

        public int ID
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

        public override bool Equals(AnimalStatus other)
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
