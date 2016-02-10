using System;
using System.Collections.Generic;
using System.Linq;

namespace CritterHeroes.Web.Areas.Admin.Critters.Models
{
    public class CritterSummaryModel
    {
        public IEnumerable<StatusModel> StatusSummary
        {
            get;
            set;
        }

        public string StatusTotal
        {
            get
            {
                return StatusSummary.Sum(x => x.Count).ToString("#,#");
            }
        }
    }
}
