using System;
using System.Collections.Generic;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.Data.Models
{
    public class State
    {
        protected State()
        {
        }

        public State(string abbreviation, string name)
        {
            ThrowIf.Argument.IsNullOrEmpty(abbreviation, nameof(abbreviation));
            ThrowIf.Argument.IsNullOrEmpty(name, nameof(name));

            this.Abbreviation = abbreviation;
            this.Name = name;
        }

        public string Abbreviation
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
