using System;
using CritterHeroes.Web.Areas.Models.Modal;

namespace CritterHeroes.Web.Areas.Account.Models
{
    public class ForgotPasswordModel
    {
        public ModalDialogModel ModalDialog
        {
            get;
            set;
        }

        public string EmailAddress
        {
            get;
            set;
        }

        public string Username
        {
            get;
            set;
        }
    }
}