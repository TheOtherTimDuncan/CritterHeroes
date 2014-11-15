using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CH.Domain.Contracts.Identity;
using CH.Domain.Identity;
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

            Mock<IApplicationUserManager> mockUserManager = new Mock<IApplicationUserManager>();
            mockUserManager.Setup(x => x.FindByNameAsync(userName)).Returns(Task.FromResult(user));

            Mock<IAuthenticationManager> mockAuthenticationManager = new Mock<IAuthenticationManager>();

            Uri returnUri = new Uri("http://google.com");

            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Setup(x => x.Request.UrlReferrer).Returns(returnUri);
            mockHttpContext.Setup(x => x.User).Returns(new GenericPrincipal(new GenericIdentity(userName), null));

            AccountController controller = new AccountController(mockUserManager.Object, mockAuthenticationManager.Object);
            controller.ControllerContext = new ControllerContext(mockHttpContext.Object, new RouteData(), controller);

            ViewResult viewResult = (ViewResult)await controller.EditProfile();
            viewResult.Model.Should().NotBeNull();
            EditProfileModel model = viewResult.Model as EditProfileModel;
            model.Should().NotBeNull();
            model.OriginalUsername.Should().Be(userName);
            model.Username.Should().Be(userName);
            model.Email.Should().Be(user.Email);
            model.FirstName.Should().Be(user.FirstName);
            model.LastName.Should().Be(user.LastName);
            model.ReturnUrl.Should().Be(returnUri.AbsoluteUri);

            mockUserManager.Verify(x => x.FindByNameAsync(userName), Times.Once);
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

            Mock<IApplicationUserManager> mockUserManager = new Mock<IApplicationUserManager>();
            mockUserManager.Setup(x => x.FindByNameAsync(userName)).Returns(Task.FromResult(user));
            mockUserManager.Setup(x => x.UpdateAsync(user)).Returns(Task.FromResult(IdentityResult.Success));

            Mock<IAuthenticationManager> mockAuthenticationManager = new Mock<IAuthenticationManager>();

            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Setup(x => x.User).Returns(new GenericPrincipal(new GenericIdentity(userName), null));

            AccountController controller = new AccountController(mockUserManager.Object, mockAuthenticationManager.Object);
            controller.ControllerContext = new ControllerContext(mockHttpContext.Object, new RouteData(), controller);

            Uri returnUri = new Uri("http://google.com");

            EditProfileModel model = new EditProfileModel()
            {
                ReturnUrl = returnUri.AbsoluteUri,
                FirstName = "New First",
                LastName = "New Last"
            };

            ActionResult actionResult = await controller.EditProfile(model);

            user.FirstName.Should().Be(model.FirstName);
            user.LastName.Should().Be(model.LastName);

            RedirectResult redirectResult = actionResult as RedirectResult;
            redirectResult.Should().NotBeNull();
            redirectResult.Url.Should().Be(returnUri.AbsoluteUri, "successful update should redirect to url in model");

            mockUserManager.Verify(x => x.FindByNameAsync(userName), Times.Once);
            mockUserManager.Verify(x => x.UpdateAsync(user), Times.Once);
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

            IdentityResult errorResult = new IdentityResult("error");

            Mock<IApplicationUserManager> mockUserManager = new Mock<IApplicationUserManager>();
            mockUserManager.Setup(x => x.FindByNameAsync(userName)).Returns(Task.FromResult(user));
            mockUserManager.Setup(x => x.UpdateAsync(user)).Returns(Task.FromResult(errorResult));

            Mock<IAuthenticationManager> mockAuthenticationManager = new Mock<IAuthenticationManager>();

            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Setup(x => x.User).Returns(new GenericPrincipal(new GenericIdentity(userName), null));

            AccountController controller = new AccountController(mockUserManager.Object, mockAuthenticationManager.Object);
            controller.ControllerContext = new ControllerContext(mockHttpContext.Object, new RouteData(), controller);

            EditProfileModel model = new EditProfileModel()
            {
                FirstName = "New First",
                LastName = "New Last"
            };

            ActionResult actionResult = await controller.EditProfile(model);
            actionResult.Should().BeOfType<ViewResult>();
            controller.ModelState[""].Errors[0].ErrorMessage.Should().Be(errorResult.Errors.First());

            mockUserManager.Verify(x => x.FindByNameAsync(userName), Times.Once);
            mockUserManager.Verify(x => x.UpdateAsync(user), Times.Once);
        }
    }
}
