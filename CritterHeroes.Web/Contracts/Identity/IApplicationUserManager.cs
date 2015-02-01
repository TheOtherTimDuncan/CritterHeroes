using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using CritterHeroes.Web.Common.Identity;
using Microsoft.AspNet.Identity;

namespace CritterHeroes.Web.Contracts.Identity
{
    public interface IApplicationUserManager
    {
        TimeSpan TokenLifespan
        {
            get;
        }

        ApplicationUserManager UserManager
        {
            get;
        }

        Task<IdentityUser> FindByEmailAsync(string email);
        Task<IdentityUser> FindByIdAsync(string userID);
        Task<IdentityUser> FindByNameAsync(string userName);

        Task<ClaimsIdentity> CreateIdentityAsync(IdentityUser user);
        Task<ClaimsIdentity> CreateIdentityAsync(IdentityUser user, string authenticationType);

        Task<string> GeneratePasswordResetTokenAsync(string userID);
        Task<IdentityResult> ResetPasswordAsync(string userID, string token, string newPassword);

        Task<string> GenerateEmailConfirmationTokenAsync(string userId);
        Task<IdentityResult> ConfirmEmailAsync(string userId, string token);

        Task<IdentityResult> UpdateAsync(IdentityUser user);
    }
}
