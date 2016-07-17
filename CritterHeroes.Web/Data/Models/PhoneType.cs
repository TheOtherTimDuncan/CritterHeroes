using System;
using System.Collections.Generic;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.Data.Models
{
    public class PhoneTypeNames
    {
        public const string Home = "Home";
        public const string Work = "Work";
        public const string Cell = "Cell";
        public const string Fax = "Fax";

        public static IEnumerable<string> GetAll()
        {
            yield return Home;
            yield return Work;
            yield return Cell;
            yield return Fax;
        }
    }

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
