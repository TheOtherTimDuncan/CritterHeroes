using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CH.Domain.Contracts.Identity;
using CH.Domain.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;

namespace CH.Domain.Identity
{
    public class ApplicationUserManager : UserManager<IdentityUser>, IApplicationUserManager
    {
        public ApplicationUserManager(IApplicationUserStore store)
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

            UserValidator = new UserValidator<IdentityUser, string>(this)
            {
                RequireUniqueEmail = true,
                AllowOnlyAlphanumericUserNames = false
            };

            TokenLifespan = TimeSpan.FromHours(24);
            IDataProtectionProvider provider = new MachineKeyProtectionProvider();
            UserTokenProvider = new DataProtectorTokenProvider<IdentityUser, string>(provider.Create("Critter Heroes"))
            {
                TokenLifespan = this.TokenLifespan
            };
        }

        public TimeSpan TokenLifespan
        {
            get;
            private set;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return this;
            }
        }

        public override async Task<ClaimsIdentity> CreateIdentityAsync(IdentityUser user, string authenticationType)
        {
            ClaimsIdentity identity = await base.CreateIdentityAsync(user, authenticationType);

            Claim claimUserID = new Claim(AppClaimTypes.UserID, user.Id);
            identity.AddClaim(claimUserID);

            return identity;
        }
    }
}