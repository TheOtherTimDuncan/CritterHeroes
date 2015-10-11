using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Common.Models;
using CritterHeroes.Web.Areas.Critters.Queries;

namespace CritterHeroes.Web.Areas.Critters.Models
{
    public class CrittersListModel
    {
        public PagingModel Paging
        {
            get; set;
        }

        public CrittersListQuery Query
        {
            get;
            set;
        }

        public IEnumerable<SelectListItem> StatusItems
        {
            get;
            set;
        }

        public IEnumerable<CritterModel> Critters
        {
            get;
            set;
        }
    }
}
