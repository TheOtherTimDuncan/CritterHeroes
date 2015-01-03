using System;
using System.Security.Principal;
using CritterHeroes.Web.Areas.Home.Models;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.Queries;

namespace CritterHeroes.Web.Areas.Home.Queries
{
    public class MenuQuery : IAsyncQuery<MenuModel>
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