using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CritterHeroes.Web.Common.Identity;
using Microsoft.AspNet.Identity;

namespace CritterHeroes.Web.Contracts.Identity
{
    public interface IAppUserStore :
        IUserStore<AppUser>,
        IUserPasswordStore<AppUser>,
        IUserEmailStore<AppUser>,
        IUserRoleStore<AppUser>,
        IUserLockoutStore<AppUser, string>,
        IUserTwoFactorStore<AppUser, string>
    {
    }
}
