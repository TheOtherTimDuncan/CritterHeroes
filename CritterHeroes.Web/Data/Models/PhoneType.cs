using System;
using System.Collections.Generic;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.Data.Models
{
    public class PhoneType
    {
        protected PhoneType()
        {
        }

        public PhoneType(string name)
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
    }
}
