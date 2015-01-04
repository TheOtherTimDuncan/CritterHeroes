using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Account;
using CritterHeroes.Web.Areas.Account.Commands;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Areas.Account.Queries;
using CritterHeroes.Web.Areas.Models.Modal;
using CritterHeroes.Web.Common.Identity;
using CritterHeroes.Web.Common.Services.Commands;
using CritterHeroes.Web.Common.Services.Queries;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Middleware;
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
            mockDispatcher.Setup(x => x.Dispatch(loginQuery)).Returns(new LoginModel());

            AccountController controller = new AccountController(mockDispatcher.Object, null);

            ViewResult viewResult = controller.Login(loginQuery) as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.Model.Should().NotBeNull();
            viewResult.Model.Should().BeOfType<LoginModel>();

            mockDispatcher.Verify(x => x.Dispatch(loginQuery), Times.Once);
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

            Mock<HttpContextBase> mockHttpContext = GetMockHttpContext();

            AccountController controller = new AccountController(null, mockDispatcher.Object);
            controller.Url = new UrlHelper(mockHttpContext.Object.Request.RequestContext, GetRouteCollection());

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

            Mock<HttpContextBase> mockHttpContext = GetMockHttpContext();

            AccountController controller = new AccountController(null, mockDispatcher.Object);
            controller.Url = new UrlHelper(mockHttpContext.Object.Request.RequestContext, GetRouteCollection());

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
            mockDispatcher.Setup(x => x.DispatchAsync<EditProfileModel>(It.IsAny<UserIDQuery>())).Returns<UserIDQuery>((query) =>
            {
                query.UserID.Should().Be(userID);
                return Task.FromResult(new EditProfileModel());
            });

            ClaimsIdentity identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(AppClaimTypes.UserID, userID));

            Mock<HttpContextBase> mockHttpContext = GetMockHttpContext();
            mockHttpContext.Setup(x => x.User).Returns(new ClaimsPrincipal(identity));

            AccountController controller = new AccountController(mockDispatcher.Object, null);
            controller.ControllerContext = CreateControllerContext(mockHttpContext, controller);

            ViewResult viewResult = (ViewResult)await controller.EditProfile();
            viewResult.Model.Should().NotBeNull();
            viewResult.Model.Should().BeOfType<EditProfileModel>();

            mockDispatcher.Verify(x => x.DispatchAsync<EditProfileModel>(It.IsAny<UserIDQuery>()), Times.Once);
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

            Mock<HttpContextBase> mockHttpContext = GetMockHttpContext();
            mockHttpContext.Setup(x => x.User).Returns(new ClaimsPrincipal(identity));

            AccountController controller = new AccountController(null, mockDispatcher.Object);
            controller.ControllerContext = CreateControllerContext(mockHttpContext, controller);

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

            Mock<HttpContextBase> mockHttpContext = GetMockHttpContext();
            mockHttpContext.Setup(x => x.User).Returns(new ClaimsPrincipal(identity));

            AccountController controller = new AccountController(null, mockDispatcher.Object);
            controller.ControllerContext = CreateControllerContext(mockHttpContext, controller);

            ActionResult actionResult = await controller.EditProfile(model);
            actionResult.Should().BeOfType<ViewResult>();

            controller.ModelState.IsValid.Should().BeFalse();
            controller.ModelState[""].Errors[0].ErrorMessage.Should().Be("Error");

            mockDispatcher.Verify(x => x.DispatchAsync<EditProfileModel>(model), Times.Once);
        }

        [TestMethod]
        public async Task PostForgotPasswordReturnsViewWithModelAndModalDialogIfCommandSucceeds()
        {
            ForgotPasswordModel model = new ForgotPasswordModel();
            model.ModalDialog.Should().BeNull("this should be the default");

            OrganizationContext organizationContext = new OrganizationContext();

            Mock<ICommandDispatcher> mockDispatcher = new Mock<ICommandDispatcher>();
            mockDispatcher.Setup(x => x.DispatchAsync<ForgotPasswordCommand>(It.IsAny<ForgotPasswordCommand>())).Returns<ForgotPasswordCommand>((command) =>
            {
                command.ModalDialog = new ModalDialogModel();
                return Task.FromResult(CommandResult.Success());
            });

            AccountController controller = new AccountController(null, mockDispatcher.Object);
            controller.ControllerContext = CreateControllerContext(GetMockHttpContext(), controller);
            controller.Request.GetOwinContext().SetOrganizationContext(organizationContext);

            ActionResult actionResult = await controller.ForgotPassword(model);

            ViewResult viewResult = actionResult as ViewResult;
            viewResult.Should().NotBeNull();

            ForgotPasswordModel resultModel = viewResult.Model as ForgotPasswordModel;
            resultModel.Should().NotBeNull();

            resultModel.ModalDialog.Should().NotBeNull();

            mockDispatcher.Verify(x => x.DispatchAsync<ForgotPasswordCommand>(It.IsAny<ForgotPasswordCommand>()), Times.Once);
        }
    }
}
