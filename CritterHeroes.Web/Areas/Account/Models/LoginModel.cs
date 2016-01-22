using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Areas.Account.Models
{
    public class LoginModel
    {
        public string ReturnUrl
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }
    }
}
