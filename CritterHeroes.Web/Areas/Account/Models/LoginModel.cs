using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CritterHeroes.Web.Areas.Account.Models
{
    public class LoginModel
    {
        public string ReturnUrl
        {
            get;
            set;
        }

        [Display(Name = "Username")]
        [Required(ErrorMessage = "Please enter a username.")]
        public string Username
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Please enter a password.")]
        [DataType(DataType.Password)]
        public string Password
        {
            get;
            set;
        }
    }
}