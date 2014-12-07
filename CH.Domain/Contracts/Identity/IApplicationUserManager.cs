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
        Task<IdentityUser> FindByIdAsync(string userID); 
        Task<IdentityUser> FindByNameAsync(string userName);

        Task<ClaimsIdentity> CreateIdentityAsync(IdentityUser user, string authenticationType);

        Task<IdentityResult> UpdateAsync(IdentityUser user);
    }
}
