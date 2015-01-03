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
        private string testUsername = "xx";
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
            IdentityUser user = new IdentityUser("test user")
            {
                PasswordHash = "passwordhash",
                Email = "email",
                PreviousEmail = "previous",
                FirstName = "first",
                LastName = "last"
            };
            user.AddRole(IdentityRole.MasterAdmin);
            await userStore.CreateAsync(user);

            IdentityUser result = await userStore.FindByIdAsync(user.Id);
            result.Should().NotBeNull();
            result.UserName.Should().Be(user.UserName);
            result.PasswordHash.Should().Be(user.PasswordHash);
            result.Email.Should().Be(user.Email);
            result.PreviousEmail.Should().Be(user.PreviousEmail);
            result.FirstName.Should().Be(user.FirstName);
            result.LastName.Should().Be(user.LastName);
            result.IsEmailConfirmed.Should().Be(user.IsEmailConfirmed);

            result.Roles.Should().HaveCount(1);
            result.Roles.First().ID.Should().Be(IdentityRole.MasterAdmin.ID);

            IdentityUser second = await userStore.FindByNameAsync(user.UserName);
            second.Should().NotBeNull();
            second.UserName.Should().Be(user.UserName);

            second.UserName = "new user";
            await userStore.UpdateAsync(second);

            IdentityUser updated = await userStore.FindByNameAsync(second.UserName);
            updated.Should().NotBeNull();
            updated.UserName.Should().Be(second.UserName);

            await userStore.DeleteAsync(updated);
            IdentityUser deleted = await userStore.FindByIdAsync(user.Id);
            deleted.Should().BeNull();
        }

        //[TestMethod]
        public async Task SeedUser()
        {
            IdentityUser user = new IdentityUser("Tim.Duncan");
            user.IsEmailConfirmed = true;
            user.Email = "tduncan72@gmail.com";
            user.FirstName = "Tim";
            user.LastName = "Duncan";
            user.AddRole(IdentityRole.MasterAdmin);
            UserManager<IdentityUser> userManager = new UserManager<IdentityUser>(userStore);
            IdentityResult result = await userManager.CreateAsync(user, "testing");
        }
    }
}
