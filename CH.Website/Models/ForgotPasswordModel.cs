using System;
using System.Web.Mvc;

namespace CH.Website.Models
{
    public class ForgotPasswordModel
    {
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