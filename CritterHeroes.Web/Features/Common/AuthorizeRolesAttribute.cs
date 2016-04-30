using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CritterHeroes.Web.Features.Common
{
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute()
            : base()
        {
        }

        public AuthorizeRolesAttribute(params string[] roles)
        {
            this.Roles = string.Join(",", roles);
        }
    }
}
