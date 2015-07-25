using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using CritterHeroes.Web.Common.Identity;
using Microsoft.AspNet.Identity;

namespace CritterHeroes.Web.Contracts.Identity
{
    public interface IAppUserManager
    {
        TimeSpan TokenLifespan
        {
            get;
        }

        AppUserManager UserManager
        {
            get;
        }

        Task<AzureAppUser> FindByEmailAsync(string email);
        Task<AzureAppUser> FindByIdAsync(string userID);
        Task<AzureAppUser> FindByNameAsync(string userName);

        Task<ClaimsIdentity> CreateIdentityAsync(AzureAppUser user);
        Task<ClaimsIdentity> CreateIdentityAsync(AzureAppUser user, string authenticationType);

        Task<string> GeneratePasswordResetTokenAsync(string userID);
        Task<IdentityResult> ResetPasswordAsync(string userID, string token, string newPassword);
        Task<bool> CheckPasswordAsync(AzureAppUser user, string password);

        Task<string> GenerateEmailConfirmationTokenAsync(string userId);
        Task<IdentityResult> ConfirmEmailAsync(string userId, string token);

        Task<IdentityResult> UpdateAsync(AzureAppUser user);
    }
}
