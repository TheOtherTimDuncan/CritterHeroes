using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace CH.Website.Models
{
    public class MenuModel
    {
        public IPrincipal CurrentUser
        {
            get;
            set;
        }

        public string OrganizationShortName
        {
            get;
            set;
        }

        public string LogoUrl
        {
            get;
            set;
        }
    }
}