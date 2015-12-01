using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Areas.Home.Models
{
    public class MenuModel
    {
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

        public bool IsLoggedIn
        {
            get;
            set;
        }

        public bool ShowAdminMenu
        {
            get;
            set;
        }

        public bool ShowMasterAdminMenu
        {
            get;
            set;
        }
    }
}
