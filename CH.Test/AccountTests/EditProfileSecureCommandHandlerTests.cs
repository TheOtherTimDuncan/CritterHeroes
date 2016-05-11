using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CH.Test.Mocks;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Email;
using CritterHeroes.Web.Contracts.Events;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.Data.Models.Identity;
using CritterHeroes.Web.Features.Account.Commands;
using CritterHeroes.Web.Features.Account.Models;
using CritterHeroes.Web.Features.Shared.ActionExtensions;
using CritterHeroes.Web.Models.LogEvents;
using CritterHeroes.Web.Shared.Commands;
using CritterHeroes.Web.Shared.StateManagement;
using FluentAssertions;
using Microsoft.AspNet.Identity;
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

            AppUser user = new AppUser(email);

            EditProfileSecureModel model = new EditProfileSecureModel()
            {
                NewEmail = "new@new.com"
            };

            Mock<IAppUserManager> mockUserManager = new Mock<IAppUserManager>();
            mockUserManager.Setup(x => x.FindByNameAsync(user.UserName)).Returns(Task.FromResult(user));
            mockUserManager.Setup(x => x.UpdateAsync(user)).Returns(Task.FromResult(IdentityResult.Success));
            mockUserManager.Setup(x => x.CreateIdentityAsync(user)).Returns(Task.FromResult(new ClaimsIdentity()));

            Mock<IAppEventPublisher> mockPublisher = new Mock<IAppEventPublisher>();
            mockPublisher.Setup(x => x.Publish(It.IsAny<UserLogEvent>())).Callback((UserLogEvent logEvent) =>
            {
                logEvent.MessageValues.Should().Contain(user.Email);
                logEvent.MessageValues.Should().Contain(model.NewEmail);
            });

            Mock<IHttpUser> mockHttpUser = new Mock<IHttpUser>();
            mockHttpUser.Setup(x => x.Username).Returns(user.Email);

            Mock<IStateManager<UserContext>> mockUserContextManager = new Mock<IStateManager<UserContext>>();

            MockUrlGenerator mockUrlGenerator = new MockUrlGenerator();

            Mock<IEmailService> mockEmailService = new Mock<IEmailService>();
            mockEmailService.Setup(x => x.SendEmailAsync(It.IsAny<ResetPasswordAttemptEmailCommand>())).Returns((ConfirmEmailEmailCommand emailCommand) =>
            {
                emailCommand.EmailData.UrlConfirm.Should().Be(mockUrlGenerator.Object.GenerateConfirmEmailAbsoluteUrl(model.NewEmail, emailCommand.EmailData.Token));

                return Task.FromResult(CommandResult.Success());
            });

            EditProfileSecureCommandHandler command = new EditProfileSecureCommandHandler(mockUserManager.Object, mockPublisher.Object, mockHttpUser.Object, mockUserContextManager.Object, mockUrlGenerator.Object, mockEmailService.Object);
            CommandResult commandResult = await command.ExecuteAsync(model);
            commandResult.Succeeded.Should().BeTrue();

            user.Email.Should().Be(email);
            user.Person.NewEmail.Should().Be(model.NewEmail);
            user.EmailConfirmed.Should().BeFalse();

            mockUserManager.Verify(x => x.FindByNameAsync(user.UserName), Times.Once);
            mockUserManager.Verify(x => x.UpdateAsync(user), Times.Once);

            mockPublisher.Verify(x => x.Publish(It.IsAny<UserLogEvent>()));

            mockEmailService.Verify(x => x.SendEmailAsync(It.IsAny<ConfirmEmailEmailCommand>()), Times.Once);
        }
    }
}
