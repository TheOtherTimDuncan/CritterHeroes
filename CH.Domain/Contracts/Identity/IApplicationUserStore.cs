using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CH.Domain.Identity;
using Microsoft.AspNet.Identity;

namespace CH.Domain.Contracts.Identity
{
    public interface IApplicationUserStore :
        IUserStore<IdentityUser>,
        IUserPasswordStore<IdentityUser>,
        IUserEmailStore<IdentityUser>,
        IUserRoleStore<IdentityUser>,
        IUserLockoutStore<IdentityUser, string>,
        IUserTwoFactorStore<IdentityUser, string>
    {
    }
}
