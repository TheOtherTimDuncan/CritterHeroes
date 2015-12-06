using System;
using System.Collections.Generic;
using CritterHeroes.Web.Areas.Common.Models;

namespace CritterHeroes.Web.Areas.Admin.Contacts.Models
{
    public class GroupSelectOptionModel : SelectOptionModel
    {
        public bool IsBusiness
        {
            get;
            set;
        }

        public bool IsPerson
        {
            get;
            set;
        }
    }
}
