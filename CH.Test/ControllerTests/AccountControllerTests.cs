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
using CH.Domain.Services.Commands;
using CH.Domain.Contracts.Commands;
using CH.Domain.Contracts.Identity;
using CH.Domain.Contracts.Logging;
using CH.Domain.Contracts.Queries;
using CH.Domain.Identity;
using CH.Domain.Models.Logging;
using CH.Domain.Services.Queries;
using CH.Website.Controllers;
using CH.Website.Models;
using CH.Website.Services.Queries;
using FluentAssertions;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CH.Test.ControllerTests
{
    [TestClass]
    public class AccountControllerTests : BaseControllerTest
    {
        [TestMethod]
        public async Task GetLoginReturnsViewWithModel()
        {
            LoginQuery loginQuery = new LoginQuery()
            {
            };

            Mock<IQueryDispatcher> mockDispatcher = new Mock<IQueryDispatcher>();
            mockDispatcher.Setup(x => x.Dispatch<LoginQuery, LoginModel>(loginQuery)).Returns(Task.FromResult(new LoginModel()));

            AccountController controller = new AccountController(mockDispatcher.Object, null);

            ViewResult viewResult = (await controller.Login(loginQuery)) as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.Model.Should().NotBeNull();
            viewResult.Model.Should().BeOfType<LoginModel>();

            mockDispatcher.Verify(x => x.Dispatch<LoginQuery, LoginModel>(loginQuery), Times.Once);
        }

        [TestMethod]
        public async Task PostLoginReturnsViewWithModelErrorsIfLoginFails()
        {
            LoginModel model = new LoginModel();

            Mock<ICommandDispatcher> mockDispatcher = new Mock<ICommandDispatcher>();
            mockDispatcher.Setup(x => x.Dispatch<LoginModel>(model)).Returns(Task.FromResult(CommandResult.Failed("", "Error")));

            AccountController controller = new AccountController(null, mockDispatcher.Object);

            ViewResult viewResult = (await controller.Login(model, null)) as ViewResult;
            viewResult.Should().NotBeNull();

            controller.ModelState.IsValid.Should().BeFalse();
            controller.ModelState[""].Errors[0].ErrorMessage.Should().Be("Error");

            mockDispatcher.Verify(x => x.Dispatch<LoginModel>(model), Times.Once);
        }

        [TestMethod]
        public async Task PostLoginRedirectsToHomeIfLoginSuccessfulAndNoReturnUrl()
        {
            LoginModel model = new LoginModel();

            Mock<ICommandDispatcher> mockDispatcher = new Mock<ICommandDispatcher>();
            mockDispatcher.Setup(x => x.Dispatch<LoginModel>(model)).Returns(Task.FromResult(CommandResult.Success()));

            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();

            AccountController controller = new AccountController(null, mockDispatcher.Object);
            controller.Url = new UrlHelper(new RequestContext(mockHttpContext.Object, new RouteData()), GetRouteCollection());

            RedirectToRouteResult redirectResult = (await controller.Login(model, null)) as RedirectToRouteResult;
            VerifyRedirectToRouteResult(redirectResult, "Index", "Home");

            mockDispatcher.Verify(x => x.Dispatch<LoginModel>(model), Times.Once);
        }

        [TestMethod]
        public async Task PostLoginRedirectsToReturnUrlIfLoginSuccessfulAndHasReturnUrl()
        {
            LoginModel model = new LoginModel();

            Mock<ICommandDispatcher> mockDispatcher = new Mock<ICommandDispatcher>();
            mockDispatcher.Setup(x => x.Dispatch<LoginModel>(model)).Returns(Task.FromResult(CommandResult.Success()));

            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();

            AccountController controller = new AccountController(null, mockDispatcher.Object);
            controller.Url = new UrlHelper(new RequestContext(mockHttpContext.Object, new RouteData()), GetRouteCollection());

            RedirectResult redirectResult = (await controller.Login(model, "/Account/EditProfile")) as RedirectResult;
            redirectResult.Should().NotBeNull();
            redirectResult.Url.Should().Be("/Account/EditProfile");

            mockDispatcher.Verify(x => x.Dispatch<LoginModel>(model), Times.Once);
        }

        [TestMethod]
        public async Task GetEditProfileReturnsViewWithModel()
        {
            string userName = "test.user";

            Mock<IQueryDispatcher> mockDispatcher = new Mock<IQueryDispatcher>();
            mockDispatcher.Setup(x => x.Dispatch<UserQuery, EditProfileModel>(It.IsAny<UserQuery>())).Returns<UserQuery>((query) =>
            {
                query.Username.Should().Be(userName);
                return Task.FromResult(new EditProfileModel());
            });

            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Setup(x => x.User).Returns(new GenericPrincipal(new GenericIdentity(userName), null));

            AccountController controller = new AccountController(mockDispatcher.Object, null);
            controller.ControllerContext = new ControllerContext(mockHttpContext.Object, new RouteData(), controller);

            ViewResult viewResult = (ViewResult)await controller.EditProfile();
            viewResult.Model.Should().NotBeNull();
            viewResult.Model.Should().BeOfType<EditProfileModel>();

            mockDispatcher.Verify(x => x.Dispatch<UserQuery, EditProfileModel>(It.IsAny<UserQuery>()), Times.Once);
        }

        [TestMethod]
        public async Task PostEditProfileRedirectsToReturnUrlOnSuccess()
        {
            string userName = "test.user";

            Uri returnUri = new Uri("http://google.com");

            EditProfileModel model = new EditProfileModel()
            {
                ReturnUrl = returnUri.AbsoluteUri,
                Username = "new.user",
                FirstName = "New First",
                LastName = "New Last"
            };

            Mock<ICommandDispatcher> mockDispatcher = new Mock<ICommandDispatcher>();
            mockDispatcher.Setup(x => x.Dispatch<EditProfileModel>(model)).Returns(Task.FromResult(CommandResult.Success()));

            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Setup(x => x.User).Returns(new GenericPrincipal(new GenericIdentity(userName), null));

            AccountController controller = new AccountController(null, mockDispatcher.Object);
            controller.ControllerContext = new ControllerContext(mockHttpContext.Object, new RouteData(), controller);

            RedirectResult redirectResult = await controller.EditProfile(model) as RedirectResult;
            redirectResult.Url.Should().Be(returnUri.AbsoluteUri);

            mockDispatcher.Verify(x => x.Dispatch<EditProfileModel>(model), Times.Once);
        }

        [TestMethod]
        public async Task PostEditProfileReturnsModelErrorIfCommandFails()
        {
            string userName = "test.user";

            EditProfileModel model = new EditProfileModel();

            Mock<ICommandDispatcher> mockDispatcher = new Mock<ICommandDispatcher>();
            mockDispatcher.Setup(x => x.Dispatch<EditProfileModel>(model)).Returns(Task.FromResult(CommandResult.Failed("", "Error")));

            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Setup(x => x.User).Returns(new GenericPrincipal(new GenericIdentity(userName), null));

            AccountController controller = new AccountController(null, mockDispatcher.Object);
            controller.ControllerContext = new ControllerContext(mockHttpContext.Object, new RouteData(), controller);

            ActionResult actionResult = await controller.EditProfile(model);
            actionResult.Should().BeOfType<ViewResult>();

            controller.ModelState.IsValid.Should().BeFalse();
            controller.ModelState[""].Errors[0].ErrorMessage.Should().Be("Error");

            mockDispatcher.Verify(x => x.Dispatch<EditProfileModel>(model), Times.Once);
        }
    }
}
