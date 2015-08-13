using System;
using System.Collections.Generic;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Models
{
    public class CritterStatusSource
    {
        public CritterStatusSource(string id, string name, string description)
        {
            ThrowIf.Argument.IsNullOrEmpty(id, nameof(id));
            ThrowIf.Argument.IsNullOrEmpty(name, nameof(name));

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
    }
}
