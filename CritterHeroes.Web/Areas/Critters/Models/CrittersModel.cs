using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Critters.Queries;

namespace CritterHeroes.Web.Areas.Critters.Models
{
    public class CrittersModel
    {
        public CrittersQuery Query
        {
            get;
            set;
        }

        public IEnumerable<SelectListItem> StatusItems
        {
            get;
            set;
        }
    }
}
