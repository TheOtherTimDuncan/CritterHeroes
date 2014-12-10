using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CH.Domain.Contracts;

namespace CH.Website.Services.Commands
{
    public class ForgotPasswordCommand
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

        public string OrganizationFullName
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