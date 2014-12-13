using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using CH.Domain.Contracts.Identity;
using CH.Domain.Contracts.Logging;
using CH.Domain.Identity;
using CH.Domain.Models.Logging;
using CH.Domain.Services.Commands;
using CH.Website.Models.Account;
using CH.Website.Services.CommandHandlers;
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
            string userID = Guid.NewGuid().ToString();

            IdentityUser user = new IdentityUser()
            {
                UserName = userID,
                Email = "email@email.com",
                FirstName = null,
                LastName = null
            };

            Mock<IApplicationUserManager> mockUserManager = new Mock<IApplicationUserManager>();
            mockUserManager.Setup(x => x.FindByIdAsync(userID)).Returns(Task.FromResult(user));
            mockUserManager.Setup(x => x.UpdateAsync(user)).Returns(Task.FromResult(IdentityResult.Success));

            Mock<IAuthenticationManager> mockAuthenticationManager = new Mock<IAuthenticationManager>();
            Mock<IUserLogger> mockUserLogger = new Mock<IUserLogger>();

            EditProfileModel model = new EditProfileModel()
            {
                OriginalUsername = userID,
                Username = userID,
                FirstName = "New First",
                LastName = "New Last"
            };

            EditProfileCommandHandler command = new EditProfileCommandHandler(mockAuthenticationManager.Object, mockUserManager.Object, mockUserLogger.Object);
            CommandResult commandResult = await command.Execute(model);
            commandResult.Succeeded.Should().BeTrue();

            user.FirstName.Should().Be(model.FirstName);
            user.LastName.Should().Be(model.LastName);

            mockUserManager.Verify(x => x.FindByIdAsync(userID), Times.Once);
            mockUserManager.Verify(x => x.UpdateAsync(user), Times.Once);
        }

        [TestMethod]
        public async Task EditProfileCommandUpdatesUserAndUpdatesUsernameWhenChanged()
        {
            string userID = Guid.NewGuid().ToString();

            IdentityUser user = new IdentityUser()
            {
                UserName = userID,
                Email = "email@email.com"
            };

            EditProfileModel model = new EditProfileModel()
            {
                OriginalUsername = user.UserName,
                Username = "new.user",
                FirstName = "New First",
                LastName = "New Last"
            };

            Mock<IApplicationUserManager> mockUserManager = new Mock<IApplicationUserManager>();
            mockUserManager.Setup(x => x.FindByIdAsync(userID)).Returns(Task.FromResult(user));
            mockUserManager.Setup(x => x.UpdateAsync(user)).Returns(Task.FromResult(IdentityResult.Success));
            mockUserManager.Setup(x => x.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie)).Returns(Task.FromResult(new ClaimsIdentity()));

            Mock<IAuthenticationManager> mockAuthenticationManager = new Mock<IAuthenticationManager>();
            mockAuthenticationManager.Setup(x => x.SignOut());
            mockAuthenticationManager.Setup(x => x.SignIn(It.IsAny<ClaimsIdentity>()));

            Mock<IUserLogger> mockLogger = new Mock<IUserLogger>();
            mockLogger.Setup(x => x.LogAction(UserActions.UsernameChanged, model.Username, It.IsAny<string>())).Returns(Task.FromResult(0));

            EditProfileCommandHandler command = new EditProfileCommandHandler(mockAuthenticationManager.Object, mockUserManager.Object, mockLogger.Object);
            CommandResult commandResult = await command.Execute(model);
            commandResult.Succeeded.Should().BeTrue();

            user.UserName.Should().Be(model.Username);
            user.FirstName.Should().Be(model.FirstName);
            user.LastName.Should().Be(model.LastName);

            mockUserManager.Verify(x => x.FindByIdAsync(userID), Times.Once);
            mockUserManager.Verify(x => x.UpdateAsync(user), Times.Once);
            mockUserManager.Verify(x => x.CreateIdentityAsync(user, It.IsAny<string>()), Times.Once);

            mockAuthenticationManager.Verify(x => x.SignOut(), Times.Once);
            mockAuthenticationManager.Verify(x => x.SignIn(It.IsAny<ClaimsIdentity>()), Times.Once);

            mockLogger.Verify(x => x.LogAction<string>(UserActions.UsernameChanged, model.Username, It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task EditProfileCommandReturnsErrorIfUsernameChangedToDuplicate()
        {
            string userName = "test.user";

            IdentityUser user = new IdentityUser()
            {
                UserName = "dupe.user",
                Email = "email@email.com",
                FirstName = "First",
                LastName = "Last"
            };

            Mock<IApplicationUserManager> mockUserManager = new Mock<IApplicationUserManager>();
            mockUserManager.Setup(x => x.FindByNameAsync(user.UserName)).Returns(Task.FromResult(user));

            Mock<IUserLogger> mockLogger = new Mock<IUserLogger>();
            Mock<IAuthenticationManager> mockAuthenticationManager = new Mock<IAuthenticationManager>();

            EditProfileModel model = new EditProfileModel()
            {
                OriginalUsername = userName,
                Username = user.UserName,
                Email = "email@email.com",
                FirstName = "First",
                LastName = "Last"
            };

            EditProfileCommandHandler command = new EditProfileCommandHandler(mockAuthenticationManager.Object, mockUserManager.Object, mockLogger.Object);
            CommandResult commandResult = await command.Execute(model);
            commandResult.Succeeded.Should().BeFalse();

            commandResult.Errors[""][0].Should().Be("The username you entered is not available. Please enter a different username.");

            mockUserManager.Verify(x => x.FindByNameAsync(user.UserName), Times.Once);
        }
    }
}
