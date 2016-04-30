using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CritterHeroes.Web.Features.Admin.Critters.Queries;

namespace CritterHeroes.Web.Features.Admin.Critters.Models
{
    public class CrittersModel
    {
        public CrittersQuery Query
        {
            get;
            set;
        }

        public bool ShowImport
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
