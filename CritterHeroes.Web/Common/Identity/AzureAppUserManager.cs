using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CritterHeroes.Web.Common.Identity;
using CritterHeroes.Web.Contracts.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;

namespace CritterHeroes.Web.Common.Identity
{
    public class AzureAppUserManager : UserManager<AzureAppUser>, IAzureAppUserManager
    {
        public AzureAppUserManager(IAzureAppUserStore store)
            : base(store)
        {
            PasswordValidator = new PasswordValidator()
            {
                RequiredLength = 4,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            UserValidator = new UserValidator<AzureAppUser, string>(this)
            {
                RequireUniqueEmail = true,
                AllowOnlyAlphanumericUserNames = false
            };

            TokenLifespan = TimeSpan.FromHours(24);
            IDataProtectionProvider provider = new MachineKeyProtectionProvider();
            UserTokenProvider = new DataProtectorTokenProvider<AzureAppUser, string>(provider.Create("Critter Heroes"))
            {
                TokenLifespan = this.TokenLifespan
            };
        }

        public TimeSpan TokenLifespan
        {
            get;
            private set;
        }

        public AzureAppUserManager UserManager
        {
            get
            {
                return this;
            }
        }

        public async Task<ClaimsIdentity> CreateIdentityAsync(AzureAppUser user)
        {
            return await CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
        }

        public override async Task<ClaimsIdentity> CreateIdentityAsync(AzureAppUser user, string authenticationType)
        {
            ClaimsIdentity identity = await base.CreateIdentityAsync(user, authenticationType);

            Claim claimUserID = new Claim(AppClaimTypes.UserID, user.Id);
            identity.AddClaim(claimUserID);

            return identity;
        }
    }
}