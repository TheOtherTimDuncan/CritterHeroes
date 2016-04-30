using System;
using System.Collections.Generic;
using CritterHeroes.Web.Features.Common.Models;

namespace CritterHeroes.Web.Features.Admin.Contacts.Models
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
