using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Common.Identity;
using CritterHeroes.Web.Common.Proxies.Configuration;
using CritterHeroes.Web.DataProviders.Azure;
using CritterHeroes.Web.DataProviders.Azure.Identity;
using FluentAssertions;
using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TOTD.Utility.StringHelpers;

namespace CH.Test.Azure
{
    [TestClass]
    public class IdentityTests
    {
        private string testUsername = "email@email.com";
        private UserStore userStore;

        [TestInitialize]
        public void InitializeTest()
        {
            userStore = new UserStore(new AzureConfiguration());
        }

        [TestCleanup]
        public void CleanupTest()
        {
            userStore.FindByNameAsync(testUsername).ContinueWith((task) =>
            {
                if (task.Result != null)
                {
                    Task.WaitAll(userStore.DeleteAsync(task.Result));
                }
            });
        }

        [TestMethod]
        public async Task TestCRUD()
        {
            AppUser user = new AppUser("email@email.com")
            {
                PasswordHash = "passwordhash",
                NewEmail = "new@new.com",
                FirstName = "first",
                LastName = "last"
            };
            user.AddRole(IdentityRole.MasterAdmin);
            await userStore.CreateAsync(user);

            AppUser result = await userStore.FindByIdAsync(user.Id);
            result.Should().NotBeNull();
            result.UserName.Should().Be(user.UserName);
            result.PasswordHash.Should().Be(user.PasswordHash);
            result.Email.Should().Be(user.Email);
            result.NewEmail.Should().Be(user.NewEmail);
            result.FirstName.Should().Be(user.FirstName);
            result.LastName.Should().Be(user.LastName);
            result.IsEmailConfirmed.Should().Be(user.IsEmailConfirmed);

            result.Roles.Should().HaveCount(1);
            result.Roles.First().ID.Should().Be(IdentityRole.MasterAdmin.ID);

            AppUser second = await userStore.FindByNameAsync(user.Email);
            second.Should().NotBeNull();
            second.UserName.Should().Be(user.UserName);

            second.Email = "new@new.com";
            await userStore.UpdateAsync(second);

            AppUser updated = await userStore.FindByNameAsync(second.Email);
            updated.Should().NotBeNull();
            updated.Email.Should().Be(second.Email);

            await userStore.DeleteAsync(updated);
            AppUser deleted = await userStore.FindByIdAsync(user.Id);
            deleted.Should().BeNull();
        }

        [TestMethod]
        public void UsernameAndEmailAreTheSame()
        {
            AppUser user = new AppUser("email@email.com");
            user.Email.Should().Be("email@email.com");
            user.UserName.Should().Be(user.Email);

            user.UserName = "new@new.com";
            user.Email.Should().Be(user.UserName);
        }

        //[TestMethod]
        public async Task SeedUser()
        {
            AppUser user = new AppUser("tduncan72@gmail.com");
            user.IsEmailConfirmed = true;
            user.FirstName = "Tim";
            user.LastName = "Duncan";
            user.AddRole(IdentityRole.MasterAdmin);
            UserManager<AppUser> userManager = new UserManager<AppUser>(userStore);
            IdentityResult result = await userManager.CreateAsync(user, "testing");
        }
    }
}
