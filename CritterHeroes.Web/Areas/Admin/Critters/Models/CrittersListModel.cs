using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Areas.Common.Models;

namespace CritterHeroes.Web.Areas.Admin.Critters.Models
{
    public class CrittersListModel
    {
        public PagingModel Paging
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
