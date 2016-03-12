using System;
using System.Collections.Generic;
using System.Linq;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.Data.Models
{
    public class CritterColor
    {
        protected CritterColor()
        {
        }

        public CritterColor(string description)
        {
            ThrowIf.Argument.IsNullOrEmpty(description, nameof(description));

            this.Description = description;
        }

        public int ID
        {
            get;
            private set;
        }

        public string RescueGroupsID
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }
    }
}
