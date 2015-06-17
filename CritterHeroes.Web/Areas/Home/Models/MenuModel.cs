using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts;

namespace CritterHeroes.Web.Areas.Home.Models
{
    public class MenuModel
    {
        public IHttpUser CurrentUser
        {
            get;
            set;
        }

        public string OrganizationShortName
        {
            get;
            set;
        }

        public string UserDisplayName
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