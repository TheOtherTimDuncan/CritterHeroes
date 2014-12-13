using System;

namespace CH.Website.Models
{
    public class ForgotPasswordModel
    {
        public bool ShowMessage
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