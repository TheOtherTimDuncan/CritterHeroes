using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CritterHeroes.Web.Areas.Account.Models
{
    public class LoginModel
    {
        public string ReturnUrl
        {
            get;
            set;
        }

        public string Username
        {
            get;
            set;
        }

        [DataType(DataType.Password)]
        public string Password
        {
            get;
            set;
        }
    }
}