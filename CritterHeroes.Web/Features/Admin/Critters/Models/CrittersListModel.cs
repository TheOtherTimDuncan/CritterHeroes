using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Features.Shared.Models;

namespace CritterHeroes.Web.Features.Admin.Critters.Models
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
