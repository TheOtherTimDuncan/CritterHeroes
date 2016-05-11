using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Data.Models.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;

namespace CritterHeroes.Web.Shared.Identity
{
    public class AppUserManager : UserManager<AppUser, int>, IAppUserManager
    {
        public AppUserManager(IAppUserStore store)
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

            UserValidator = new UserValidator<AppUser, int>(this)
            {
                RequireUniqueEmail = true,
                AllowOnlyAlphanumericUserNames = false
            };

            TokenLifespan = TimeSpan.FromHours(24);
            IDataProtectionProvider provider = new MachineKeyProtectionProvider();
            UserTokenProvider = new DataProtectorTokenProvider<AppUser, int>(provider.Create("Critter Heroes"))
            {
                TokenLifespan = this.TokenLifespan
            };
        }

        public TimeSpan TokenLifespan
        {
            get;
            private set;
        }

        public AppUserManager UserManager
        {
            get
            {
                return this;
            }
        }

        public async Task<ClaimsIdentity> CreateIdentityAsync(AppUser user)
        {
            return await CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
        }

        public override async Task<ClaimsIdentity> CreateIdentityAsync(AppUser user, string authenticationType)
        {
            ClaimsIdentity identity = await base.CreateIdentityAsync(user, authenticationType);

            Claim claimUserID = new Claim(AppClaimTypes.UserID, user.Id.ToString());
            identity.AddClaim(claimUserID);

            return identity;
        }

        public async Task<AppUser> FindByUnconfirmedEmailAsync(string email)
        {
            return await this.Users.SingleOrDefaultAsync(x => x.Person.NewEmail == email);
        }

        // Let's convert tokens to base64 here so they are safe to use in a url whether that url is generated in the
        // web service or in the web site

        public override async Task<string> GenerateEmailConfirmationTokenAsync(int userId)
        {
            string token = await base.GenerateEmailConfirmationTokenAsync(userId);
            return ToBase64(token);
        }

        public override async Task<IdentityResult> ConfirmEmailAsync(int userId, string token)
        {
            string converted = FromBase64(token);
            return await base.ConfirmEmailAsync(userId, converted);
        }

        public override async Task<string> GeneratePasswordResetTokenAsync(int userId)
        {
            string token = await base.GeneratePasswordResetTokenAsync(userId);
            return ToBase64(token);
        }

        public override async Task<IdentityResult> ResetPasswordAsync(int userId, string token, string newPassword)
        {
            string converted = FromBase64(token);
            return await base.ResetPasswordAsync(userId, converted, newPassword);
        }

        private string ToBase64(string original)
        {
            byte[] data = Encoding.UTF8.GetBytes(original);
            return Convert.ToBase64String(data);
        }

        private string FromBase64(string encoded)
        {
            try
            {
                byte[] data = Convert.FromBase64String(encoded);
                return Encoding.UTF8.GetString(data);
            }
            catch
            {
                // If there is an error converting the token assume it's invalid and return null
                return null;
            }
        }
    }
}
