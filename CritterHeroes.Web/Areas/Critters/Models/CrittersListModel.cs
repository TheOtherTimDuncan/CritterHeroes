using System;
using System.Collections.Generic;
using System.Linq;

namespace CritterHeroes.Web.Areas.Critters.Models
{
    public class CrittersListModel
    {
        public IEnumerable<CritterModel> Critters
        {
            get;
            set;
        }
    }
}
