using System;
using System.Collections.Generic;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.Data.Models
{
    public class AnimalStatus
    {
        protected AnimalStatus()
        {
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
    }
}
