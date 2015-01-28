using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Account.CommandHandlers;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Common.Identity;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Models.Logging;
using FluentAssertions;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CH.Test.CommandTests
{
    [TestClass]
    public class EditProfileCommandHandlerTests
    {
        [TestMethod]
        public async Task EditProfileCommandUpdatesUser()
        {
            IdentityUser user = new IdentityUser()
            {
                UserName = "test.user",
                Email = "email@email.com",
                FirstName = null,
                LastName = null
            };

            Mock<IApplicationUserManager> mockUserManager = new Mock<IApplicationUserManager>();
            mockUserManager.Setup(x => x.FindByIdAsync(user.Id)).Returns(Task.FromResult(user));
            mockUserManager.Setup(x => x.UpdateAsync(user)).Returns(Task.FromResult(IdentityResult.Success));

            Mock<IAuthenticationManager> mockAuthenticationManager = new Mock<IAuthenticationManager>();
            Mock<IUserLogger> mockUserLogger = new Mock<IUserLogger>();

            Mock<IHttpUser> mockHttpUser = new Mock<IHttpUser>();
            mockHttpUser.Setup(x => x.Username).Returns(user.UserName);
            mockHttpUser.Setup(x => x.UserID).Returns(user.Id);

            Mock<IStateManager<UserContext>> mockUserContextManager = new Mock<IStateManager<UserContext>>();

            EditProfileModel model = new EditProfileModel()
            {
                Username = user.UserName,
                FirstName = "New First",
                LastName = "New Last"
            };

            EditProfileCommandHandler command = new EditProfileCommandHandler(mockAuthenticationManager.Object, mockUserManager.Object, mockUserLogger.Object, mockHttpUser.Object, mockUserContextManager.Object);
            CommandResult commandResult = await command.ExecuteAsync(model);
            commandResult.Succeeded.Should().BeTrue();

            user.FirstName.Should().Be(model.FirstName);
            user.LastName.Should().Be(model.LastName);

            mockUserManager.Verify(x => x.FindByIdAsync(user.Id), Times.Once);
            mockUserManager.Verify(x => x.UpdateAsync(user), Times.Once);
            mockUserContextManager.Verify(x => x.SaveContext(It.IsAny<UserContext>()), Times.Once);
        }

        [TestMethod]
        public async Task EditProfileCommandUpdatesUserAndUpdatesUsernameWhenChanged()
        {
            IdentityUser user = new IdentityUser()
            {
                UserName = "test.user",
                Email = "email@email.com"
            };

            EditProfileModel model = new EditProfileModel()
            {
                Username = "new.user",
                FirstName = "New First",
                LastName = "New Last"
            };

            Mock<IApplicationUserManager> mockUserManager = new Mock<IApplicationUserManager>();
            mockUserManager.Setup(x => x.FindByIdAsync(user.Id)).Returns(Task.FromResult(user));
            mockUserManager.Setup(x => x.UpdateAsync(user)).Returns(Task.FromResult(IdentityResult.Success));
            mockUserManager.Setup(x => x.CreateIdentityAsync(user)).Returns(Task.FromResult(new ClaimsIdentity()));

            Mock<IAuthenticationManager> mockAuthenticationManager = new Mock<IAuthenticationManager>();
            mockAuthenticationManager.Setup(x => x.SignOut());
            mockAuthenticationManager.Setup(x => x.SignIn(It.IsAny<ClaimsIdentity>()));

            Mock<IUserLogger> mockLogger = new Mock<IUserLogger>();
            mockLogger.Setup(x => x.LogActionAsync(UserActions.UsernameChanged, model.Username, It.IsAny<string>())).Returns(Task.FromResult(0));

            Mock<IHttpUser> mockHttpUser = new Mock<IHttpUser>();
            mockHttpUser.Setup(x => x.Username).Returns(user.UserName);
            mockHttpUser.Setup(x => x.UserID).Returns(user.Id);

            Mock<IStateManager<UserContext>> mockUserContextManager = new Mock<IStateManager<UserContext>>();

            EditProfileCommandHandler command = new EditProfileCommandHandler(mockAuthenticationManager.Object, mockUserManager.Object, mockLogger.Object, mockHttpUser.Object, mockUserContextManager.Object);
            CommandResult commandResult = await command.ExecuteAsync(model);
            commandResult.Succeeded.Should().BeTrue();

            user.UserName.Should().Be(model.Username);
            user.FirstName.Should().Be(model.FirstName);
            user.LastName.Should().Be(model.LastName);

            mockUserManager.Verify(x => x.FindByIdAsync(user.Id), Times.Once);
            mockUserManager.Verify(x => x.UpdateAsync(user), Times.Once);
            mockUserManager.Verify(x => x.CreateIdentityAsync(user), Times.Once);

            mockAuthenticationManager.Verify(x => x.SignOut(), Times.Once);
            mockAuthenticationManager.Verify(x => x.SignIn(It.IsAny<ClaimsIdentity>()), Times.Once);

            mockLogger.Verify(x => x.LogActionAsync<string>(UserActions.UsernameChanged, model.Username, It.IsAny<string>()), Times.Once);
        }
    }
}
