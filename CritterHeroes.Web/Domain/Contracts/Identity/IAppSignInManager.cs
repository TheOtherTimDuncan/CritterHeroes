using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;

namespace CritterHeroes.Web.Domain.Contracts.Identity
{
    public interface IAppSignInManager
    {
        Task<SignInStatus> PasswordSignInAsync(string userName, string password);
    }
}
