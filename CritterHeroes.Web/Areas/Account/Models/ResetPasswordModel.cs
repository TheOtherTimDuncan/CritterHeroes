using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Areas.Account.Models
{
    public class ResetPasswordModel : LoginModel
    {
        public string Code
        {
            get;
            set;
        }

        public string ConfirmPassword
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
