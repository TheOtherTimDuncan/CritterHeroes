using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using AR.Azure.Identity;
using AR.Domain.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TOTD.Utility.StringHelpers;

namespace AR.Test.Azure
{
    [TestClass]
    public class IdentityTests
    {
        private string testUsername = "xx";
        private UserStore userStore;

        [TestInitialize]
        public void InitializeTest()
        {
            userStore = new UserStore(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);
        }

        [TestCleanup]
        public void CleanupTest()
        {
            userStore.FindByNameAsync(testUsername).ContinueWith((task) =>
            {
                if (task.Result != null)
                {
                    userStore.DeleteAsync(task.Result);
                }
            });
        }

        [TestMethod]
        public async Task TestCRUD()
        {
            IdentityUser user = new IdentityUser("test user")
            {
                PasswordHash = "passwordhash",
                Email = "email"
            };
            user.AddRole(IdentityRole.MasterAdmin);
            await userStore.CreateAsync(user);

            IdentityUser result = await userStore.FindByIdAsync(user.Id);
            Assert.IsNotNull(result);
            Assert.AreEqual(user.UserName, result.UserName);
            Assert.AreEqual(user.PasswordHash, result.PasswordHash);
            Assert.AreEqual(user.Email, result.Email);
            Assert.AreEqual(user.IsEmailConfirmed, result.IsEmailConfirmed);

            Assert.AreEqual(1, result.Roles.Count());
            Assert.AreEqual(IdentityRole.MasterAdmin.ID, result.Roles.First().ID);

            IdentityUser second = await userStore.FindByNameAsync(user.UserName);
            Assert.IsNotNull(second);
            Assert.AreEqual(user.UserName, second.UserName);

            second.UserName = "new user";
            await userStore.UpdateAsync(second);

            IdentityUser updated = await userStore.FindByNameAsync(second.UserName);
            Assert.IsNotNull(updated);
            Assert.AreEqual(second.UserName, updated.UserName);

            await userStore.DeleteAsync(updated);
            IdentityUser deleted = await userStore.FindByIdAsync(user.Id);
            Assert.IsNull(deleted);
        }

        //[TestMethod]
        public async Task SeedUser()
        {
            IdentityUser user = new IdentityUser("Tim.Duncan");
            user.IsEmailConfirmed = true;
            user.Email = "tduncan72@gmail.com";
            user.AddRole(IdentityRole.MasterAdmin);
            UserManager<IdentityUser> userManager = new UserManager<IdentityUser>(userStore);
            IdentityResult result= await userManager.CreateAsync(user, "testing");
        }
    }
}
