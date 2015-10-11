using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CritterHeroes.Web.Areas.Critters.Models
{
    public class CrittersModel
    {
        public IEnumerable<SelectListItem> StatusItems
        {
            get;
            set;
        }
    }
}
