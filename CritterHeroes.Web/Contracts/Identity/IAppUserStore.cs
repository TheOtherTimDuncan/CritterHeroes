using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Data.Models.Identity;
using Microsoft.AspNet.Identity;

namespace CritterHeroes.Web.Contracts.Identity
{
    public interface IAppUserStore :
        IUserStore<AppUser, int>,
        IUserPasswordStore<AppUser, int>,
        IUserEmailStore<AppUser, int>,
        IUserRoleStore<AppUser, int>,
        IUserLockoutStore<AppUser, int>,
        IUserTwoFactorStore<AppUser, int>
    {
    }
}
