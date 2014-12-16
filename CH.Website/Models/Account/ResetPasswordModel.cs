using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CH.Website.Models.Account
{
    public class ResetPasswordModel : LoginModel
    {
        public string Code
        {
            get;
            set;
        }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword
        {
            get;
            set;
        }

        public bool ShowMessage
        {
            get;
            set;
        }
    }
}