using System;
using System.Security.Principal;
using CritterHeroes.Web.Common.StateManagement;

namespace CritterHeroes.Web.Areas.Home.Queries
{
    public class MenuQuery
    {
        public OrganizationContext OrganizationContext
        {
            get;
            set;
        }

        public IPrincipal CurrentUser
        {
            get;
            set;
        }

        public UserContext UserContext
        {
            get;
            set;
        }
    }
}