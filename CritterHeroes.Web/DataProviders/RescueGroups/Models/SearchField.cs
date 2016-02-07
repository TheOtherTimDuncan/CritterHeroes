using System;
using System.Collections.Generic;
using System.Linq;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Models
{
    public class SearchField
    {
        public SearchField(string name)
        {
            this.Name = name;
            this.IsSelected = true;
        }

        public string Name
        {
            get;
            private set;
        }

        public bool IsSelected
        {
            get;
            set;
        }
    }
}
