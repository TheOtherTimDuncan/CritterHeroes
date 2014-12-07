using System;
using System.Security.Principal;
using CH.Domain.StateManagement;

namespace CH.Website.Services.Queries
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