using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [DisplayFormat(DataFormatString = "{0:#,#}")]
        public int StatusTotal
        {
            get
            {
                return StatusSummary.Sum(x => x.Count);
            }
        }
    }
}
