using System;
using System.Collections.Generic;
using System.Linq;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.Data.Models
{
    public class Location
    {
        protected Location()
        {
        }

        public Location(string name)
        {
            ThrowIf.Argument.IsNullOrEmpty(name, nameof(name));

            this.Name = name;
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

        public string Address
        {
            get;
            set;
        }

        public string City
        {
            get;
            set;
        }

        public string State
        {
            get;
            set;
        }

        public string Zip
        {
            get;
            set;
        }

        public string Phone
        {
            get;
            set;
        }

        public string Website
        {
            get;
            set;
        }

        public int? RescueGroupsID
        {
            get;
            set;
        }
    }
}
