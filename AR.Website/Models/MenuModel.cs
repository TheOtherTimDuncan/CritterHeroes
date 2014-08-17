using System;
using System.Collections.Generic;
using System.Linq;
using AR.Domain.Identity;

namespace AR.Website.Models
{
    public class MenuModel
    {
        public bool IsAuthenticated
        {
            get;
            set;
        }

        public IEnumerable<IdentityRole> UserRoles
        {
            get;
            set;
        }
    }
}