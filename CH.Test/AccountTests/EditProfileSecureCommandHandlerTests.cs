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
using CritterHeroes.Web.Contracts.Email;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.Models.Logging;
using FluentAssertions;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CH.Test.AccountTests
{
    [TestClass]
    public class EditProfileSecureCommandHandlerTests
    {
        [TestMethod]
        public async Task EditProfileSecureCommandUpdatesUserEmailAndLogsChange()
        {
            string email = "email@email.com";

            AzureAppUser user = new AzureAppUser(email);

            EditProfileSecureModel model = new EditProfileSecureModel()
            {
                NewEmail = "new@new.com"
            };

            Mock<IAzureAppUserManager> mockUserManager = new Mock<IAzureAppUserManager>();
            mockUserManager.Setup(x => x.FindByIdAsync(user.Id)).Returns(Task.FromResult(user));
            mockUserManager.Setup(x => x.UpdateAsync(user)).Returns(Task.FromResult(IdentityResult.Success));
            mockUserManager.Setup(x => x.CreateIdentityAsync(user)).Returns(Task.FromResult(new ClaimsIdentity()));

            Mock<IUserLogger> mockLogger = new Mock<IUserLogger>();

            Mock<IHttpUser> mockHttpUser = new Mock<IHttpUser>();
            mockHttpUser.Setup(x => x.Username).Returns(user.Email);
            mockHttpUser.Setup(x => x.UserID).Returns(user.Id);

            Mock<IStateManager<UserContext>> mockUserContextManager = new Mock<IStateManager<UserContext>>();

            Mock<IEmailService> mockEmailService = new Mock<IEmailService>();

            Mock<IUrlGenerator> mockUrlGenerator = new Mock<IUrlGenerator>();

            EditProfileSecureCommandHandler command = new EditProfileSecureCommandHandler(mockUserManager.Object, mockLogger.Object, mockHttpUser.Object, mockUserContextManager.Object, mockUrlGenerator.Object, mockEmailService.Object);
            CommandResult commandResult = await command.ExecuteAsync(model);
            commandResult.Succeeded.Should().BeTrue();

            user.Email.Should().Be(email);
            user.NewEmail.Should().Be(model.NewEmail);
            user.IsEmailConfirmed.Should().BeFalse();

            mockUserManager.Verify(x => x.FindByIdAsync(user.Id), Times.Once);
            mockUserManager.Verify(x => x.UpdateAsync(user), Times.Once);

            mockLogger.Verify(x => x.LogActionAsync<string>(UserActions.EmailChanged, email, It.IsAny<string>()), Times.Once);

            mockEmailService.Verify(x => x.SendEmailAsync(It.IsAny<ConfirmEmailCommand>()), Times.Once);
        }
    }
}
