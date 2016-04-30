using System;
using System.Collections.Generic;
using System.Linq;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Models
{
    public abstract class BaseSource
    {
        public abstract int ID
        {
            get;
            set;
        }
    }
}
