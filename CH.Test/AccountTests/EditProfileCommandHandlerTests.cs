using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.Data.Models.Identity;
using CritterHeroes.Web.Features.Account.Commands;
using CritterHeroes.Web.Features.Account.Models;
using CritterHeroes.Web.Shared.Commands;
using CritterHeroes.Web.Shared.StateManagement;
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
            AppUser user = new AppUser("email@email.com");
            user.Person.FirstName = null;
            user.Person.LastName = null;

            Mock<IAppUserManager> mockUserManager = new Mock<IAppUserManager>();
            mockUserManager.Setup(x => x.FindByNameAsync(user.UserName)).Returns(Task.FromResult(user));
            mockUserManager.Setup(x => x.UpdateAsync(user)).Returns(Task.FromResult(IdentityResult.Success));

            Mock<IHttpUser> mockHttpUser = new Mock<IHttpUser>();
            mockHttpUser.Setup(x => x.Username).Returns(user.Email);

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

            user.Person.FirstName.Should().Be(model.FirstName);
            user.Person.LastName.Should().Be(model.LastName);

            mockUserManager.Verify(x => x.FindByNameAsync(user.UserName), Times.Once);
            mockUserManager.Verify(x => x.UpdateAsync(user), Times.Once);
            mockUserContextManager.Verify(x => x.SaveContext(It.IsAny<UserContext>()), Times.Once);
        }
    }
}
