using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Areas.Account.Models
{
    public class ConfirmEmailModel
    {
        public string Email
        {
            get;
            set;
        }

        public string ConfirmationCode
        {
            get;
            set;
        }

        public bool? IsSuccess
        {
            get;
            set;
        }
    }
}
