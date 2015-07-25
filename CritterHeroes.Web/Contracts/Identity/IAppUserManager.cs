using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CritterHeroes.Web.Common.Identity;
using CritterHeroes.Web.Data.Models.Identity;
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

        Task<AppUser> FindByEmailAsync(string email);
        Task<AppUser> FindByIdAsync(int userID);
        Task<AppUser> FindByNameAsync(string userName);

        Task<ClaimsIdentity> CreateIdentityAsync(AppUser user);
        Task<ClaimsIdentity> CreateIdentityAsync(AppUser user, string authenticationType);

        Task<string> GeneratePasswordResetTokenAsync(int userID);
        Task<IdentityResult> ResetPasswordAsync(int userID, string token, string newPassword);
        Task<bool> CheckPasswordAsync(AppUser user, string password);

        Task<string> GenerateEmailConfirmationTokenAsync(int userId);
        Task<IdentityResult> ConfirmEmailAsync(int userId, string token);

        Task<IdentityResult> UpdateAsync(AppUser user);
    }
}