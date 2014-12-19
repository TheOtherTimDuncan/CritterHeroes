using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CH.Domain.Contracts;
using CH.Website.Models.Modal;

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

        public ModalDialogModel ModalDialog
        {
            get;
            set;
        }

        public IUrlGenerator UrlGenerator
        {
            get;
            set;
        }
    }
}