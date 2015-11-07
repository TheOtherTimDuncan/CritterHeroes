using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CH.Test.Mocks;
using CritterHeroes.Web.Areas.Account.CommandHandlers;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Areas.Admin.Critters;
using CritterHeroes.Web.Areas.Common.ActionExtensions;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Email;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Models.Identity;
using CritterHeroes.Web.Models.Logging;
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
            string urlLogo = "logo";

            AppUser user = new AppUser(email);

            OrganizationContext organizationContext = new OrganizationContext()
            {
                FullName = "FullName"
            };

            EditProfileSecureModel model = new EditProfileSecureModel()
            {
                NewEmail = "new@new.com"
            };

            Mock<IAppUserManager> mockUserManager = new Mock<IAppUserManager>();
            mockUserManager.Setup(x => x.FindByNameAsync(user.UserName)).Returns(Task.FromResult(user));
            mockUserManager.Setup(x => x.UpdateAsync(user)).Returns(Task.FromResult(IdentityResult.Success));
            mockUserManager.Setup(x => x.CreateIdentityAsync(user)).Returns(Task.FromResult(new ClaimsIdentity()));

            Mock<IUserLogger> mockLogger = new Mock<IUserLogger>();

            Mock<IHttpUser> mockHttpUser = new Mock<IHttpUser>();
            mockHttpUser.Setup(x => x.Username).Returns(user.Email);

            Mock<IStateManager<UserContext>> mockUserContextManager = new Mock<IStateManager<UserContext>>();

            Mock<IStateManager<OrganizationContext>> mockOrganizationStateManager = new Mock<IStateManager<OrganizationContext>>();
            mockOrganizationStateManager.Setup(x => x.GetContext()).Returns(organizationContext);

            MockUrlGenerator mockUrlGenerator = new MockUrlGenerator();

            Mock<IOrganizationLogoService> mockLogoService = new Mock<IOrganizationLogoService>();
            mockLogoService.Setup(x => x.GetLogoUrl()).Returns(urlLogo);

            Mock<IEmailService> mockEmailService = new Mock<IEmailService>();
            mockEmailService.Setup(x => x.SendEmailAsync(It.IsAny<ResetPasswordAttemptEmailCommand>())).Returns((ResetPasswordAttemptEmailCommand emailCommand) =>
            {
                emailCommand.OrganizationFullName.Should().Be(organizationContext.FullName);
                emailCommand.LogoUrl.Should().Be(urlLogo);

                emailCommand.HomeUrl.Should().Be(mockUrlGenerator.UrlHelper.AbsoluteAction(nameof(CrittersController.Index), CritterActionExtensions.ControllerRouteName));

                return Task.FromResult(CommandResult.Success());
            });

            EditProfileSecureCommandHandler command = new EditProfileSecureCommandHandler(mockUserManager.Object, mockLogger.Object, mockHttpUser.Object, mockUserContextManager.Object, mockUrlGenerator.Object, mockEmailService.Object, mockOrganizationStateManager.Object, mockLogoService.Object);
            CommandResult commandResult = await command.ExecuteAsync(model);
            commandResult.Succeeded.Should().BeTrue();

            user.Email.Should().Be(email);
            user.Person.NewEmail.Should().Be(model.NewEmail);
            user.EmailConfirmed.Should().BeFalse();

            mockUserManager.Verify(x => x.FindByNameAsync(user.UserName), Times.Once);
            mockUserManager.Verify(x => x.UpdateAsync(user), Times.Once);

            mockLogger.Verify(x => x.LogActionAsync<string>(UserActions.EmailChanged, email, It.IsAny<string>()), Times.Once);

            mockEmailService.Verify(x => x.SendEmailAsync(It.IsAny<ConfirmEmailEmailCommand>()), Times.Once);
        }
    }
}
