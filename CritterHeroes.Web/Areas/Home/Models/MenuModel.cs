using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Areas.Common.Models;
using CritterHeroes.Web.Contracts;

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

        public IEnumerable<ControllerActionModel> NavItems
        {
            get;
            set;
        }

        public IEnumerable<ControllerActionModel> AdminNavItems
        {
            get;
            set;
        }
    }
}
