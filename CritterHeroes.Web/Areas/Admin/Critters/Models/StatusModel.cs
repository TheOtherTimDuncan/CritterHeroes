using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CritterHeroes.Web.Areas.Admin.Critters.Models
{
    public class StatusModel
    {
        public string Status
        {
            get;
            set;
        }

        [DisplayFormat(DataFormatString = "{0:#,#}")]
        public int Count
        {
            get;
            set;
        }
    }
}
