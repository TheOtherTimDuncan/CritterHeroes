using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Areas.Models.Modal;
using CritterHeroes.Web.Common.StateManagement;

namespace CritterHeroes.Web.Areas.Account.Models
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

        public OrganizationContext OrganizationContext
        {
            get;
            set;
        }
    }
}