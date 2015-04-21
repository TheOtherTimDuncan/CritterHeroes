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
    public class EditProfileSecureCommandHandlerTests
    {
        [TestMethod]
        public async Task EditProfileSecureCommandUpdatesUserEmailAndLogsChange()
        {
            IdentityUser user = new IdentityUser("email@email.com");

            EditProfileSecureModel model = new EditProfileSecureModel()
            {
                NewEmail = "new@new.com"
            };

            Mock<IApplicationUserManager> mockUserManager = new Mock<IApplicationUserManager>();
            mockUserManager.Setup(x => x.FindByIdAsync(user.Id)).Returns(Task.FromResult(user));
            mockUserManager.Setup(x => x.UpdateAsync(user)).Returns(Task.FromResult(IdentityResult.Success));
            mockUserManager.Setup(x => x.CreateIdentityAsync(user)).Returns(Task.FromResult(new ClaimsIdentity()));

            Mock<IAuthenticationManager> mockAuthenticationManager = new Mock<IAuthenticationManager>();

            Mock<IUserLogger> mockLogger = new Mock<IUserLogger>();

            Mock<IHttpUser> mockHttpUser = new Mock<IHttpUser>();
            mockHttpUser.Setup(x => x.Username).Returns(user.Email);
            mockHttpUser.Setup(x => x.UserID).Returns(user.Id);

            Mock<IStateManager<UserContext>> mockUserContextManager = new Mock<IStateManager<UserContext>>();

            EditProfileSecureCommandHandler command = new EditProfileSecureCommandHandler(mockAuthenticationManager.Object, mockUserManager.Object, mockLogger.Object, mockHttpUser.Object, mockUserContextManager.Object);
            CommandResult commandResult = await command.ExecuteAsync(model);
            commandResult.Succeeded.Should().BeTrue();

            user.Email.Should().Be(model.NewEmail);

            mockUserManager.Verify(x => x.FindByIdAsync(user.Id), Times.Once);
            mockUserManager.Verify(x => x.UpdateAsync(user), Times.Once);
            mockUserManager.Verify(x => x.CreateIdentityAsync(user), Times.Once);

            mockAuthenticationManager.Verify(x => x.SignOut(), Times.Once);
            mockAuthenticationManager.Verify(x => x.SignIn(It.IsAny<ClaimsIdentity>()), Times.Once);

            mockLogger.Verify(x => x.LogActionAsync<string>(UserActions.EmailChanged, model.NewEmail, It.IsAny<string>()), Times.Once);
        }
    }
}
