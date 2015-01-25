using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CritterHeroes.Web.Contracts.Commands;

namespace CritterHeroes.Web.Areas.Account.Models
{
    public class LoginModel : IUserCommand
    {
        public string ReturnUrl
        {
            get;
            set;
        }

        [Display(Name = "Username")]
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