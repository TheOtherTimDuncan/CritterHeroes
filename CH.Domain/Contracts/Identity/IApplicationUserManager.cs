using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using CH.Domain.Identity;
using Microsoft.AspNet.Identity;

namespace CH.Domain.Contracts.Identity
{
    public interface IApplicationUserManager
    {
        TimeSpan TokenLifespan
        {
            get;
        }

        Task<IdentityUser> FindByEmailAsync(string email);
        Task<IdentityUser> FindByIdAsync(string userID); 
        Task<IdentityUser> FindByNameAsync(string userName);

        Task<ClaimsIdentity> CreateIdentityAsync(IdentityUser user, string authenticationType);

        Task<string> GeneratePasswordResetTokenAsync(string userID);
        Task<IdentityResult> ResetPasswordAsync(string userID, string token, string newPassword);

        Task<IdentityResult> UpdateAsync(IdentityUser user);
    }
}
