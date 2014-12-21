using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CH.Domain.Contracts.Commands;
using CH.Domain.Contracts.Queries;
using CH.Domain.Identity;
using CH.Domain.Services.Commands;
using CH.Domain.Services.Queries;
using CH.Website.Controllers;
using CH.Website.Models.Account;
using CH.Website.Services.Commands;
using CH.Website.Services.Queries;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CH.Test.ControllerTests
{
    [TestClass]
    public class AccountControllerTests : BaseControllerTest
    {
        [TestMethod]
        public void GetLoginReturnsViewWithModel()
        {
            LoginQuery loginQuery = new LoginQuery()
            {
            };

            Mock<IQueryDispatcher> mockDispatcher = new Mock<IQueryDispatcher>();
            mockDispatcher.Setup(x => x.Dispatch<LoginQuery, LoginModel>(loginQuery)).Returns(new LoginModel());

            AccountController controller = new AccountController(mockDispatcher.Object, null);

            ViewResult viewResult = controller.Login(loginQuery) as ViewResult;
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
            mockDispatcher.Setup(x => x.DispatchAsync<LoginModel>(model)).Returns(Task.FromResult(CommandResult.Failed("", "Error")));

            AccountController controller = new AccountController(null, mockDispatcher.Object);

            ViewResult viewResult = (await controller.Login(model, null)) as ViewResult;
            viewResult.Should().NotBeNull();

            controller.ModelState.IsValid.Should().BeFalse();
            controller.ModelState[""].Errors[0].ErrorMessage.Should().Be("Error");

            mockDispatcher.Verify(x => x.DispatchAsync<LoginModel>(model), Times.Once);
        }

        [TestMethod]
        public async Task PostLoginRedirectsToHomeIfLoginSuccessfulAndNoReturnUrl()
        {
            LoginModel model = new LoginModel();

            Mock<ICommandDispatcher> mockDispatcher = new Mock<ICommandDispatcher>();
            mockDispatcher.Setup(x => x.DispatchAsync<LoginModel>(model)).Returns(Task.FromResult(CommandResult.Success()));

            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();

            AccountController controller = new AccountController(null, mockDispatcher.Object);
            controller.Url = new UrlHelper(new RequestContext(mockHttpContext.Object, new RouteData()), GetRouteCollection());

            RedirectToRouteResult redirectResult = (await controller.Login(model, null)) as RedirectToRouteResult;
            VerifyRedirectToRouteResult(redirectResult, "Index", "Home");

            mockDispatcher.Verify(x => x.DispatchAsync<LoginModel>(model), Times.Once);
        }

        [TestMethod]
        public async Task PostLoginRedirectsToReturnUrlIfLoginSuccessfulAndHasReturnUrl()
        {
            LoginModel model = new LoginModel();

            Mock<ICommandDispatcher> mockDispatcher = new Mock<ICommandDispatcher>();
            mockDispatcher.Setup(x => x.DispatchAsync<LoginModel>(model)).Returns(Task.FromResult(CommandResult.Success()));

            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();

            AccountController controller = new AccountController(null, mockDispatcher.Object);
            controller.Url = new UrlHelper(new RequestContext(mockHttpContext.Object, new RouteData()), GetRouteCollection());

            RedirectResult redirectResult = (await controller.Login(model, "/Account/EditProfile")) as RedirectResult;
            redirectResult.Should().NotBeNull();
            redirectResult.Url.Should().Be("/Account/EditProfile");

            mockDispatcher.Verify(x => x.DispatchAsync<LoginModel>(model), Times.Once);
        }

        [TestMethod]
        public async Task GetEditProfileReturnsViewWithModel()
        {
            string userID = Guid.NewGuid().ToString();

            Mock<IQueryDispatcher> mockDispatcher = new Mock<IQueryDispatcher>();
            mockDispatcher.Setup(x => x.DispatchAsync<UserIDQuery, EditProfileModel>(It.IsAny<UserIDQuery>())).Returns<UserIDQuery>((query) =>
            {
                query.UserID.Should().Be(userID);
                return Task.FromResult(new EditProfileModel());
            });

            ClaimsIdentity identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(AppClaimTypes.UserID, userID));

            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Setup(x => x.User).Returns(new ClaimsPrincipal(identity));

            AccountController controller = new AccountController(mockDispatcher.Object, null);
            controller.ControllerContext = new ControllerContext(mockHttpContext.Object, new RouteData(), controller);

            ViewResult viewResult = (ViewResult)await controller.EditProfile();
            viewResult.Model.Should().NotBeNull();
            viewResult.Model.Should().BeOfType<EditProfileModel>();

            mockDispatcher.Verify(x => x.DispatchAsync<UserIDQuery, EditProfileModel>(It.IsAny<UserIDQuery>()), Times.Once);
        }

        [TestMethod]
        public async Task PostEditProfileRedirectsToReturnUrlOnSuccess()
        {
            string userID = Guid.NewGuid().ToString();

            Uri returnUri = new Uri("http://google.com");

            EditProfileModel model = new EditProfileModel()
            {
                ReturnUrl = returnUri.AbsoluteUri,
                Username = "new.user",
                FirstName = "New First",
                LastName = "New Last"
            };

            ClaimsIdentity identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(AppClaimTypes.UserID, userID));

            Mock<ICommandDispatcher> mockDispatcher = new Mock<ICommandDispatcher>();
            mockDispatcher.Setup(x => x.DispatchAsync<EditProfileModel>(model)).Returns(Task.FromResult(CommandResult.Success()));

            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Setup(x => x.User).Returns(new ClaimsPrincipal(identity));

            AccountController controller = new AccountController(null, mockDispatcher.Object);
            controller.ControllerContext = new ControllerContext(mockHttpContext.Object, new RouteData(), controller);

            RedirectResult redirectResult = await controller.EditProfile(model) as RedirectResult;
            redirectResult.Url.Should().Be(returnUri.AbsoluteUri);

            mockDispatcher.Verify(x => x.DispatchAsync<EditProfileModel>(model), Times.Once);
        }

        [TestMethod]
        public async Task PostEditProfileReturnsModelErrorIfCommandFails()
        {
            string userID = Guid.NewGuid().ToString();

            EditProfileModel model = new EditProfileModel();

            ClaimsIdentity identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(AppClaimTypes.UserID, userID));

            Mock<ICommandDispatcher> mockDispatcher = new Mock<ICommandDispatcher>();
            mockDispatcher.Setup(x => x.DispatchAsync<EditProfileModel>(model)).Returns(Task.FromResult(CommandResult.Failed("", "Error")));

            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Setup(x => x.User).Returns(new ClaimsPrincipal(identity));

            AccountController controller = new AccountController(null, mockDispatcher.Object);
            controller.ControllerContext = new ControllerContext(mockHttpContext.Object, new RouteData(), controller);

            ActionResult actionResult = await controller.EditProfile(model);
            actionResult.Should().BeOfType<ViewResult>();

            controller.ModelState.IsValid.Should().BeFalse();
            controller.ModelState[""].Errors[0].ErrorMessage.Should().Be("Error");

            mockDispatcher.Verify(x => x.DispatchAsync<EditProfileModel>(model), Times.Once);
        }
    }
}
