using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CritterHeroes.Web.Data.Models.Identity
{
    public class AppRole : IdentityRole<int, AppUserRole>, IRole<int>
    {
    }
}