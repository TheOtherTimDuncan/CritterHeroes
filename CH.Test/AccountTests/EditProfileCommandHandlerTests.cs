using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Account.CommandHandlers;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Common.Identity;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Contracts.StateManagement;
using FluentAssertions;
using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CH.Test.AccountTests
{
    [TestClass]
    public class EditProfileCommandHandlerTests
    {
        [TestMethod]
        public async Task EditProfileCommandUpdatesUser()
        {
            AzureAppUser user = new AzureAppUser("email@email.com")
            {
                FirstName = null,
                LastName = null
            };

            Mock<IAzureAppUserManager> mockUserManager = new Mock<IAzureAppUserManager>();
            mockUserManager.Setup(x => x.FindByIdAsync(user.Id)).Returns(Task.FromResult(user));
            mockUserManager.Setup(x => x.UpdateAsync(user)).Returns(Task.FromResult(IdentityResult.Success));

            Mock<IHttpUser> mockHttpUser = new Mock<IHttpUser>();
            mockHttpUser.Setup(x => x.Username).Returns(user.Email);
            mockHttpUser.Setup(x => x.UserID).Returns(user.Id);

            Mock<IStateManager<UserContext>> mockUserContextManager = new Mock<IStateManager<UserContext>>();

            EditProfileModel model = new EditProfileModel()
            {
                Email = user.Email,
                FirstName = "New First",
                LastName = "New Last"
            };

            EditProfileCommandHandler command = new EditProfileCommandHandler(mockUserManager.Object, mockHttpUser.Object, mockUserContextManager.Object);
            CommandResult commandResult = await command.ExecuteAsync(model);
            commandResult.Succeeded.Should().BeTrue();

            user.FirstName.Should().Be(model.FirstName);
            user.LastName.Should().Be(model.LastName);

            mockUserManager.Verify(x => x.FindByIdAsync(user.Id), Times.Once);
            mockUserManager.Verify(x => x.UpdateAsync(user), Times.Once);
            mockUserContextManager.Verify(x => x.SaveContext(It.IsAny<UserContext>()), Times.Once);
        }
    }
}
