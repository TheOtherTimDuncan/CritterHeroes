using System;
using System.Collections.Generic;
using System.Linq;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.Data.Models
{
    public class Group
    {
        protected Group()
        {
        }

        public Group(string name)
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
            set;
        }

        public bool IsPerson
        {
            get;
            set;
        }

        public bool IsBusiness
        {
            get;
            set;
        }
    }
}
