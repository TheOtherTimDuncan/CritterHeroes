using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Data.Models.Identity;
using CritterHeroes.Web.Features.Account.Commands;
using CritterHeroes.Web.Features.Account.Models;
using CritterHeroes.Web.Shared.Commands;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CH.Test.AccountTests
{
    [TestClass]
    public class EditProfileLoginCommandHandlerTests
    {
        [TestMethod]
        public async Task ReturnsFailedIfPasswordIsInvalid()
        {
            AppUser user = new AppUser("email@email.com");

            EditProfileLoginModel command = new EditProfileLoginModel()
            {
                Password = "password"
            };

            Mock<IAppUserManager> mockUserManager = new Mock<IAppUserManager>();
            mockUserManager.Setup(x => x.FindByNameAsync(user.UserName)).Returns(Task.FromResult(user));
            mockUserManager.Setup(x => x.CheckPasswordAsync(user, command.Password)).Returns(Task.FromResult(false));

            Mock<IHttpUser> mockHttpUser = new Mock<IHttpUser>();
            mockHttpUser.Setup(x => x.Username).Returns(user.UserName);

            EditProfileLoginCommandHandler handler = new EditProfileLoginCommandHandler(mockUserManager.Object, mockHttpUser.Object);
            CommandResult commandResult = await handler.ExecuteAsync(command);

            commandResult.Succeeded.Should().BeFalse();

            mockUserManager.Verify(x => x.FindByNameAsync(user.UserName), Times.Once);
            mockUserManager.Verify(x => x.CheckPasswordAsync(user, command.Password), Times.Once);
        }

        [TestMethod]
        public async Task ReturnsSuccessIfPasswordIsValid()
        {
            AppUser user = new AppUser("email@email.com");

            EditProfileLoginModel command = new EditProfileLoginModel()
            {
                Password = "password"
            };

            Mock<IAppUserManager> mockUserManager = new Mock<IAppUserManager>();
            mockUserManager.Setup(x => x.FindByNameAsync(user.UserName)).Returns(Task.FromResult(user));
            mockUserManager.Setup(x => x.CheckPasswordAsync(user, command.Password)).Returns(Task.FromResult(true));

            Mock<IHttpUser> mockHttpUser = new Mock<IHttpUser>();
            mockHttpUser.Setup(x => x.Username).Returns(user.UserName);

            EditProfileLoginCommandHandler handler = new EditProfileLoginCommandHandler(mockUserManager.Object, mockHttpUser.Object);
            CommandResult commandResult = await handler.ExecuteAsync(command);

            commandResult.Succeeded.Should().BeTrue();

            mockUserManager.Verify(x => x.FindByNameAsync(user.UserName), Times.Once);
            mockUserManager.Verify(x => x.CheckPasswordAsync(user, command.Password), Times.Once);
        }
    }
}
