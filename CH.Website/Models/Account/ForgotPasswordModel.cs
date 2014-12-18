using System;
using CH.Website.Models.Modal;

namespace CH.Website.Models.Account
{
    public class ForgotPasswordModel
    {
        public bool ShowMessage
        {
            get;
            set;
        }

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