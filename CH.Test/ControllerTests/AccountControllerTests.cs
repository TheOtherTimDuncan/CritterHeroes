using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CH.Domain.Contracts.Identity;
using CH.Domain.Contracts.Logging;
using CH.Domain.Identity;
using CH.Domain.Models.Logging;
using CH.Website.Controllers;
using CH.Website.Models;
using FluentAssertions;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CH.Test.ControllerTests
{
    [TestClass]
    public class AccountControllerTests : BaseTest
    {
        [TestMethod]
        public async Task GetEditProfileReturnsViewWithModel()
        {
            string userName = "test.user";

            IdentityUser user = new IdentityUser()
            {
                UserName = userName,
                Email = "email@email.com",
                FirstName = "First",
                LastName = "Last"
            };

            Mock<IApplicationUserStore> mockUserStore = new Mock<IApplicationUserStore>();
            mockUserStore.Setup(x => x.FindByNameAsync(userName)).Returns(Task.FromResult(user));

            Mock<IAuthenticationManager> mockAuthenticationManager = new Mock<IAuthenticationManager>();

            Uri returnUri = new Uri("http://google.com");

            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Setup(x => x.Request.UrlReferrer).Returns(returnUri);
            mockHttpContext.Setup(x => x.User).Returns(new GenericPrincipal(new GenericIdentity(userName), null));

            ApplicationUserManager userManager = new ApplicationUserManager(mockUserStore.Object);
            AccountController controller = new AccountController(userManager, mockAuthenticationManager.Object, null);
            controller.ControllerContext = new ControllerContext(mockHttpContext.Object, new RouteData(), controller);

            ViewResult viewResult = (ViewResult)await controller.EditProfile();
            viewResult.Model.Should().NotBeNull();
            EditProfileModel model = viewResult.Model as EditProfileModel;
            model.Should().NotBeNull();
            model.Username.Should().Be(userName);
            model.Email.Should().Be(user.Email);
            model.FirstName.Should().Be(user.FirstName);
            model.LastName.Should().Be(user.LastName);
            model.ReturnUrl.Should().Be(returnUri.AbsoluteUri);

            mockUserStore.Verify(x => x.FindByNameAsync(userName), Times.Once);
        }

        [TestMethod]
        public async Task PostEditProfileUpdatesUser()
        {
            string userName = "test.user";

            IdentityUser user = new IdentityUser()
            {
                UserName = userName,
                Email = "email@email.com",
                FirstName = "First",
                LastName = "Last"
            };

            Mock<IApplicationUserStore> mockUserStore = new Mock<IApplicationUserStore>();
            mockUserStore.Setup(x => x.FindByNameAsync(userName)).Returns(Task.FromResult(user));
            mockUserStore.Setup(x => x.UpdateAsync(user)).Returns(Task.FromResult(IdentityResult.Success));

            // These methods are called by the user validator to check for unique emails
            mockUserStore.Setup(x => x.GetEmailAsync(user)).Returns(Task.FromResult(user.Email));
            mockUserStore.Setup(x => x.FindByEmailAsync(user.Email)).Returns(Task.FromResult<IdentityUser>(null));

            Mock<IAuthenticationManager> mockAuthenticationManager = new Mock<IAuthenticationManager>();

            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Setup(x => x.User).Returns(new GenericPrincipal(new GenericIdentity(userName), null));

            ApplicationUserManager userManager = new ApplicationUserManager(mockUserStore.Object);
            AccountController controller = new AccountController(userManager, mockAuthenticationManager.Object, null);
            controller.ControllerContext = new ControllerContext(mockHttpContext.Object, new RouteData(), controller);

            Uri returnUri = new Uri("http://google.com");

            EditProfileModel model = new EditProfileModel()
            {
                ReturnUrl = returnUri.AbsoluteUri,
                Username = user.UserName,
                FirstName = "New First",
                LastName = "New Last"
            };

            ActionResult actionResult = await controller.EditProfile(model);

            user.FirstName.Should().Be(model.FirstName);
            user.LastName.Should().Be(model.LastName);

            RedirectResult redirectResult = actionResult as RedirectResult;
            redirectResult.Should().NotBeNull();
            redirectResult.Url.Should().Be(returnUri.AbsoluteUri, "successful update should redirect to url in model");

            mockUserStore.Verify(x => x.FindByNameAsync(userName), Times.Exactly(2));
            mockUserStore.Verify(x => x.UpdateAsync(user), Times.Once);
        }

        [TestMethod]
        public async Task PostEditProfileUpdatesUserAndUpdatesUsernameWhenChanged()
        {
            string userName = "test.user";

            IdentityUser user = new IdentityUser()
            {
                UserName = userName,
                Email = "email@email.com"
            };

            Uri returnUri = new Uri("http://google.com");

            EditProfileModel model = new EditProfileModel()
            {
                ReturnUrl = returnUri.AbsoluteUri,
                Username = "new.user",
                FirstName = "New First",
                LastName = "New Last"
            };

            Mock<IApplicationUserStore> mockUserStore = new Mock<IApplicationUserStore>();
            mockUserStore.Setup(x => x.FindByNameAsync(userName)).Returns(Task.FromResult(user));
            mockUserStore.Setup(x => x.UpdateAsync(user)).Returns(Task.FromResult(IdentityResult.Success));

            // These methods are called by the user validator to check for unique emails
            mockUserStore.Setup(x => x.GetEmailAsync(user)).Returns(Task.FromResult(user.Email));
            mockUserStore.Setup(x => x.FindByEmailAsync(user.Email)).Returns(Task.FromResult<IdentityUser>(null));

            Mock<IAuthenticationManager> mockAuthenticationManager = new Mock<IAuthenticationManager>();
            mockAuthenticationManager.Setup(x => x.SignOut());
            mockAuthenticationManager.Setup(x => x.SignIn(It.IsAny<ClaimsIdentity>()));

            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Setup(x => x.User).Returns(new GenericPrincipal(new GenericIdentity(userName), null));

            Mock<IUserLogger> mockLogger = new Mock<IUserLogger>();
            mockLogger.Setup(x => x.LogAction(UserActions.UsernameChanged, model.Username, It.IsAny<string>())).Returns(Task.FromResult(0));

            ApplicationUserManager userManager = new ApplicationUserManager(mockUserStore.Object);
            Mock<IClaimsIdentityFactory<IdentityUser, string>> mockClaimsIdentityFactory = new Mock<IClaimsIdentityFactory<IdentityUser, string>>();
            mockClaimsIdentityFactory.Setup(x => x.CreateAsync(userManager, user, It.IsAny<string>())).Returns(Task.FromResult(new ClaimsIdentity()));
            userManager.ClaimsIdentityFactory = mockClaimsIdentityFactory.Object;

            AccountController controller = new AccountController(userManager, mockAuthenticationManager.Object, mockLogger.Object);
            controller.ControllerContext = new ControllerContext(mockHttpContext.Object, new RouteData(), controller);

            ActionResult actionResult = await controller.EditProfile(model);

            user.UserName.Should().Be(model.Username);
            user.FirstName.Should().Be(model.FirstName);
            user.LastName.Should().Be(model.LastName);

            RedirectResult redirectResult = actionResult as RedirectResult;
            redirectResult.Should().NotBeNull();
            redirectResult.Url.Should().Be(returnUri.AbsoluteUri, "successful update should redirect to url in model");

            mockUserStore.Verify(x => x.FindByNameAsync(userName), Times.Once);
            mockUserStore.Verify(x => x.UpdateAsync(user), Times.Once);
            mockAuthenticationManager.Verify(x => x.SignOut(), Times.Once);
            mockAuthenticationManager.Verify(x => x.SignIn(It.IsAny<ClaimsIdentity>()), Times.Once);
            mockClaimsIdentityFactory.Verify(x => x.CreateAsync(userManager, user, It.IsAny<string>()), Times.Once);
            mockLogger.Verify(x => x.LogAction<string>(UserActions.UsernameChanged, model.Username, It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task PostEditProfileReturnsModelErrorIfUsernameChangedToDuplicate()
        {
            string userName = "test.user";

            IdentityUser user = new IdentityUser()
            {
                UserName = "dupe.user",
                Email = "email@email.com",
                FirstName = "First",
                LastName = "Last"
            };

            Mock<IApplicationUserStore> mockUserStore = new Mock<IApplicationUserStore>();
            mockUserStore.Setup(x => x.FindByNameAsync(user.UserName)).Returns(Task.FromResult(user));

            Mock<IAuthenticationManager> mockAuthenticationManager = new Mock<IAuthenticationManager>();

            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Setup(x => x.User).Returns(new GenericPrincipal(new GenericIdentity(userName), null));

            ApplicationUserManager userManager = new ApplicationUserManager(mockUserStore.Object);
            AccountController controller = new AccountController(userManager, mockAuthenticationManager.Object, null);
            controller.ControllerContext = new ControllerContext(mockHttpContext.Object, new RouteData(), controller);

            EditProfileModel model = new EditProfileModel()
            {
                Username = user.UserName,
                Email = "email@email.com",
                FirstName = "First",
                LastName = "Last"
            };

            ActionResult actionResult = await controller.EditProfile(model);
            actionResult.Should().BeOfType<ViewResult>();

            controller.ModelState.IsValid.Should().BeFalse();
            controller.ModelState[""].Errors[0].ErrorMessage.Should().Be("The username you entered is not available. Please enter a different username.");

            mockUserStore.Verify(x => x.FindByNameAsync(user.UserName), Times.Once);
        }

        [TestMethod]
        public async Task PostEditProfileAddsIdentityErrorsToModelState()
        {
            string userName = "test.user";

            IdentityUser user = new IdentityUser()
            {
                UserName = userName,
                Email = "email@email.com",
                FirstName = "First",
                LastName = "Last"
            };

            Mock<IApplicationUserStore> mockUserStore = new Mock<IApplicationUserStore>();
            mockUserStore.Setup(x => x.FindByNameAsync(userName)).Returns(Task.FromResult(user));
            // UpdateAsync can't return errors so no need to mock it

            Mock<IAuthenticationManager> mockAuthenticationManager = new Mock<IAuthenticationManager>();

            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Setup(x => x.User).Returns(new GenericPrincipal(new GenericIdentity(userName), null));

            IdentityResult errorResult = new IdentityResult("error");
            Mock<IIdentityValidator<IdentityUser>> fakeValidator = new Mock<IIdentityValidator<IdentityUser>>();
            fakeValidator.Setup(x => x.ValidateAsync(user)).Returns(Task.FromResult(errorResult));

            ApplicationUserManager userManager = new ApplicationUserManager(mockUserStore.Object);
            userManager.UserValidator = fakeValidator.Object;

            AccountController controller = new AccountController(userManager, mockAuthenticationManager.Object, null);
            controller.ControllerContext = new ControllerContext(mockHttpContext.Object, new RouteData(), controller);

            EditProfileModel model = new EditProfileModel()
            {
                Username = user.UserName,
                FirstName = "New First",
                LastName = "New Last"
            };

            ActionResult actionResult = await controller.EditProfile(model);
            actionResult.Should().BeOfType<ViewResult>();
            controller.ModelState[""].Errors[0].ErrorMessage.Should().Be(errorResult.Errors.First());

            mockUserStore.Verify(x => x.FindByNameAsync(userName), Times.Once);
            fakeValidator.Verify(x => x.ValidateAsync(user), Times.Once);
        }
    }
}
