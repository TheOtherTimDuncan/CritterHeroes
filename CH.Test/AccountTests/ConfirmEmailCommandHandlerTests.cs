using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Account.CommandHandlers;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Data.Models.Identity;
using CritterHeroes.Web.Models.Logging;
using FluentAssertions;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CH.Test.AccountTests
{
    [TestClass]
    public class ConfirmEmailCommandHandlerTests
    {
        [TestMethod]
        public async Task ReturnsFailedIfUserNotFound()
        {
            ConfirmEmailModel command = new ConfirmEmailModel()
            {
                Email = "email@email.com"
            };

            Mock<IAppUserManager> mockUserManager = new Mock<IAppUserManager>();
            mockUserManager.Setup(x => x.FindByUnconfirmedEmailAsync(command.Email)).Returns(Task.FromResult<AppUser>(null));

            Mock<IUserLogger> mockUserLogger = new Mock<IUserLogger>();

            ConfirmEmailCommandHandler handler = new ConfirmEmailCommandHandler(mockUserLogger.Object, mockUserManager.Object, null);
            CommandResult commandResult = await handler.ExecuteAsync(command);
            commandResult.Succeeded.Should().BeFalse();

            mockUserManager.Verify(x => x.FindByUnconfirmedEmailAsync(command.Email), Times.Once);
            mockUserLogger.Verify(x => x.LogActionAsync(UserActions.ConfirmEmailFailure, command.Email));
        }

        [TestMethod]
        public async Task ReturnsFailedIfConfirmEmailFails()
        {
            AppUser user = new AppUser("email@email.com");

            ConfirmEmailModel command = new ConfirmEmailModel()
            {
                Email = user.Email,
                ConfirmationCode = "invalid"
            };

            IdentityResult identityResult = IdentityResult.Failed("failed");

            Mock<IAppUserManager> mockUserManager = new Mock<IAppUserManager>();
            mockUserManager.Setup(x => x.FindByUnconfirmedEmailAsync(command.Email)).Returns(Task.FromResult(user));
            mockUserManager.Setup(x => x.ConfirmEmailAsync(user.Id, command.ConfirmationCode)).Returns(Task.FromResult(identityResult));

            Mock<IUserLogger> mockUserLogger = new Mock<IUserLogger>();

            ConfirmEmailCommandHandler handler = new ConfirmEmailCommandHandler(mockUserLogger.Object, mockUserManager.Object, null);
            CommandResult commandResult = await handler.ExecuteAsync(command);
            commandResult.Succeeded.Should().BeFalse();

            command.IsSuccess.Should().NotHaveValue();

            mockUserManager.Verify(x => x.FindByUnconfirmedEmailAsync(command.Email), Times.Once);
            mockUserManager.Verify(x => x.ConfirmEmailAsync(user.Id, command.ConfirmationCode), Times.Once);
            mockUserLogger.Verify(x => x.LogActionAsync(UserActions.ConfirmEmailFailure, command.Email, identityResult.Errors));
        }

        [TestMethod]
        public async Task ReturnsSucceededIfConfirmEmailSucceeds()
        {
            AppUser user = new AppUser("email@email.com");
            user.Person.NewEmail = "new@new.com";

            ConfirmEmailModel command = new ConfirmEmailModel()
            {
                Email = user.Person.NewEmail,
                ConfirmationCode = "code"
            };

            Mock<IAppUserManager> mockUserManager = new Mock<IAppUserManager>();
            mockUserManager.Setup(x => x.FindByUnconfirmedEmailAsync(command.Email)).Returns(Task.FromResult(user));
            mockUserManager.Setup(x => x.ConfirmEmailAsync(user.Id, command.ConfirmationCode)).Returns(Task.FromResult(IdentityResult.Success));

            Mock<IUserLogger> mockUserLogger = new Mock<IUserLogger>();

            Mock<IAuthenticationManager> mockAuthenticationManager = new Mock<IAuthenticationManager>();

            ConfirmEmailCommandHandler handler = new ConfirmEmailCommandHandler(mockUserLogger.Object, mockUserManager.Object, mockAuthenticationManager.Object);
            CommandResult commandResult = await handler.ExecuteAsync(command);
            commandResult.Succeeded.Should().BeTrue();
            command.IsSuccess.Should().BeTrue();

            user.Email.Should().Be(command.Email);

            mockUserManager.Verify(x => x.FindByUnconfirmedEmailAsync(command.Email), Times.Once);
            mockUserManager.Verify(x => x.ConfirmEmailAsync(user.Id, command.ConfirmationCode), Times.Once);
            mockUserManager.Verify(x => x.UpdateAsync(user), Times.Once);
            mockUserLogger.Verify(x => x.LogActionAsync(UserActions.ConfirmEmailSuccess, command.Email));
            mockAuthenticationManager.Verify(x => x.SignOut(), Times.Once);
        }
    }
}
