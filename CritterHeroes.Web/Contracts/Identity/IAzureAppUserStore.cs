using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CritterHeroes.Web.Common.Identity;
using Microsoft.AspNet.Identity;

namespace CritterHeroes.Web.Contracts.Identity
{
    public interface IAzureAppUserStore :
        IUserStore<AzureAppUser>,
        IUserPasswordStore<AzureAppUser>,
        IUserEmailStore<AzureAppUser>,
        IUserRoleStore<AzureAppUser>,
        IUserLockoutStore<AzureAppUser, string>,
        IUserTwoFactorStore<AzureAppUser, string>
    {
    }
}
