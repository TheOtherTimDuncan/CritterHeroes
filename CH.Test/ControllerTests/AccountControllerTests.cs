using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Account;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Areas.Account.Queries;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Common.Dispatchers;
using CritterHeroes.Web.Common.Identity;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Middleware;
using CritterHeroes.Web.Models;
using CritterHeroes.Web.Models.Logging;
using FluentAssertions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
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

            AccountController controller = CreateController<AccountController>();

            ViewResult viewResult = controller.Login(loginQuery) as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.Model.Should().NotBeNull();
            viewResult.Model.Should().BeOfType<LoginModel>();
        }

        [TestMethod]
        public async Task PostLoginReturnsViewWithModelErrorsIfLoginFails()
        {
            LoginModel model = new LoginModel();

            mockSignInManager.Setup(x => x.PasswordSignInAsync(model.Email, model.Password)).Returns(Task.FromResult(SignInStatus.Failure));

            AccountController controller = CreateController<AccountController>();

            ViewResult viewResult = (await controller.Login(model, null)) as ViewResult;
            viewResult.Should().NotBeNull();

            controller.ModelState.IsValid.Should().BeFalse();
            controller.ModelState[""].Errors[0].ErrorMessage.Should().Be("The username or password that you entered was incorrect. Please try again.");

            mockSignInManager.Verify(x => x.PasswordSignInAsync(model.Email, model.Password), Times.Once);
            mockUserLogger.Verify(x => x.LogActionAsync(UserActions.PasswordLoginFailure, model.Email), Times.Once);
        }

        [TestMethod]
        public async Task PostLoginRedirectsToHomeIfLoginSuccessfulAndNoReturnUrl()
        {
            LoginModel model = new LoginModel()
            {
                Email = "email@email.com",
                Password = "password"
            };

            mockSignInManager.Setup(x => x.PasswordSignInAsync(model.Email, model.Password)).Returns(Task.FromResult(SignInStatus.Success));

            Mock<HttpContextBase> mockHttpContext = GetMockHttpContext();

            AccountController controller = CreateController<AccountController>();
            controller.Url = new UrlHelper(mockHttpContext.Object.Request.RequestContext, GetRouteCollection());

            RedirectResult redirectResult = (await controller.Login(model, null)) as RedirectResult;
            redirectResult.Url.Should().EndWith("/Home/Index");

            mockSignInManager.Verify(x => x.PasswordSignInAsync(model.Email, model.Password), Times.Once);
            mockUserLogger.Verify(x => x.LogActionAsync(UserActions.PasswordLoginSuccess, model.Email), Times.Once);
        }

        [TestMethod]
        public async Task PostLoginRedirectsToReturnUrlIfLoginSuccessfulAndHasReturnUrl()
        {
            LoginModel model = new LoginModel()
            {
                Email = "email@email.com",
                Password = "password"
            };

            mockSignInManager.Setup(x => x.PasswordSignInAsync(model.Email, model.Password)).Returns(Task.FromResult(SignInStatus.Success));

            Mock<HttpContextBase> mockHttpContext = GetMockHttpContext();

            AccountController controller = CreateController<AccountController>();
            controller.Url = new UrlHelper(mockHttpContext.Object.Request.RequestContext, GetRouteCollection());

            RedirectResult redirectResult = (await controller.Login(model, "/Account/EditProfile")) as RedirectResult;
            redirectResult.Should().NotBeNull();
            redirectResult.Url.Should().Be("/Account/EditProfile");

            mockSignInManager.Verify(x => x.PasswordSignInAsync(model.Email, model.Password), Times.Once);
            mockUserLogger.Verify(x => x.LogActionAsync(UserActions.PasswordLoginSuccess, model.Email), Times.Once);
        }

        [TestMethod]
        public async Task GetEditProfileReturnsViewWithModel()
        {
            IdentityUser user = new IdentityUser("email@email.com")
            {
                FirstName = "First",
                LastName = "Last"
            };

            mockUserStore.Setup(x => x.FindByIdAsync(user.Id)).Returns(Task.FromResult(user));

            mockHttpUser.Setup(x => x.UserID).Returns(user.Id);

            Mock<IHeaderDictionary> mockHeaderDictionary = new Mock<IHeaderDictionary>();
            string[] headerValue = new string[] { "http://google.com" };
            mockHeaderDictionary.Setup(x => x.TryGetValue(It.IsAny<string>(), out headerValue)).Returns(true);

            mockOwinContext.Setup(x => x.Request.Headers).Returns(mockHeaderDictionary.Object);

            AccountController controller = CreateController<AccountController>();

            ViewResult viewResult = (ViewResult)await controller.EditProfile((string)null);
            viewResult.Model.Should().NotBeNull();
            viewResult.Model.Should().BeOfType<EditProfileModel>();

            mockUserStore.Verify(x => x.FindByIdAsync(user.Id), Times.Once);
        }

        [TestMethod]
        public async Task PostEditProfileRedirectsToReturnUrlOnSuccess()
        {
            IdentityUser user = new IdentityUser("email@email.com");

            Uri returnUri = new Uri("http://google.com");

            EditProfileModel model = new EditProfileModel()
            {
                ReturnUrl = returnUri.AbsoluteUri,
                Email = user.Email,
            };

            mockHttpUser.Setup(x => x.Username).Returns(user.Email);
            mockHttpUser.Setup(x => x.UserID).Returns(user.Id);

            mockUserManager.Setup(x => x.FindByIdAsync(user.Id)).Returns(Task.FromResult(user));
            mockUserManager.Setup(x => x.FindByIdAsync(user.Id)).Returns(Task.FromResult(user));
            mockUserManager.Setup(x => x.UpdateAsync(user)).Returns(Task.FromResult(IdentityResult.Success));

            Mock<UrlHelper> mockUrlHelper = new Mock<UrlHelper>();
            mockUrlHelper.Setup(x => x.IsLocalUrl(It.IsAny<string>())).Returns(true);

            AccountController controller = CreateController<AccountController>();
            controller.Url = mockUrlHelper.Object;

            RedirectResult redirectResult = await controller.EditProfile(model) as RedirectResult;
            redirectResult.Url.Should().Be(returnUri.AbsoluteUri);

            mockUserManager.Verify(x => x.FindByIdAsync(user.Id), Times.Once);
            mockUserManager.Verify(x => x.UpdateAsync(user), Times.Once);
            mockUserContextManager.Verify(x => x.SaveContext(It.IsAny<UserContext>()), Times.Once);
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
            ForgotPasswordModel model = new ForgotPasswordModel()
            {
                Email = "email@email.com",
            };
            model.ModalDialog.Should().BeNull("this should be the default");

            IdentityUser user = new IdentityUser(model.Email);

            OrganizationContext organizationContext = new OrganizationContext()
            {
                OrganizationID = Guid.NewGuid(),
                FullName = "Full Name",
                EmailAddress = "org@org.com"
            };

            string url = "http://www.password.reset.com/code";

            mockUserManager.Setup(x => x.FindByEmailAsync(model.Email)).Returns(Task.FromResult(user));
            mockUserManager.Setup(x => x.GeneratePasswordResetTokenAsync(user.Id)).Returns(Task.FromResult(url));

            mockEmailClient.Setup(x => x.SendAsync(It.IsAny<EmailMessage>(), user.Id)).Returns(Task.FromResult(0));

            AccountController controller = CreateController<AccountController>();
            controller.ControllerContext = CreateControllerContext(GetMockHttpContext(), controller);
            controller.Request.GetOwinContext().SetOrganizationContext(organizationContext);

            ActionResult actionResult = await controller.ForgotPassword(model);

            ViewResult viewResult = actionResult as ViewResult;
            viewResult.Should().NotBeNull();

            ForgotPasswordModel resultModel = viewResult.Model as ForgotPasswordModel;
            resultModel.Should().NotBeNull();

            resultModel.ModalDialog.Should().NotBeNull();

            mockUserManager.Verify(x => x.FindByEmailAsync(model.Email), Times.Once);
            mockUserManager.Verify(x => x.GeneratePasswordResetTokenAsync(user.Id), Times.Once);
            mockEmailClient.Verify(x => x.SendAsync(It.IsAny<EmailMessage>(), user.Id), Times.Once);
            mockUserLogger.Verify(x => x.LogActionAsync(UserActions.ForgotPasswordSuccess, user.Email), Times.Once);
        }

        [TestMethod]
        public async Task PostResetPasswordReturnsModelErrorIfUsernameIsInvalid()
        {
            ResetPasswordModel model = new ResetPasswordModel()
            {
                Email = "unit.test",
                Code = "code"
            };

            mockUserManager.Setup(x => x.FindByEmailAsync(model.Email)).Returns(Task.FromResult((IdentityUser)null));

            AccountController controller = new AccountController(new QueryDispatcher(container), new CommandDispatcher(container));
            ActionResult actionResult = await controller.ResetPassword(model);

            mockUserLogger.Verify(x => x.LogActionAsync(UserActions.ResetPasswordFailure, model.Email, It.IsAny<string>()), Times.Once);
            mockUserManager.Verify(x => x.FindByEmailAsync(model.Email), Times.Once);
        }

        [TestMethod]
        public async Task PostResetPasswordReturnsModelErrorIfResetPasswordFails()
        {
            ResetPasswordModel model = new ResetPasswordModel()
            {
                Email = "email@email.com",
                Code = "code"
            };

            IdentityUser user = new IdentityUser(model.Email);

            mockUserManager.Setup(x => x.FindByEmailAsync(model.Email)).Returns(Task.FromResult(user));
            mockUserManager.Setup(x => x.ResetPasswordAsync(user.Id, model.Code, model.Password)).Returns(Task.FromResult(IdentityResult.Failed("nope")));

            AccountController controller = CreateController<AccountController>();
            ActionResult actionResult = await controller.ResetPassword(model);

            mockUserLogger.Verify(x => x.LogActionAsync(UserActions.ResetPasswordFailure, model.Email, It.IsAny<string>()), Times.Once);
            mockUserManager.Verify(x => x.FindByEmailAsync(model.Email), Times.Once);
            mockUserManager.Verify(x => x.ResetPasswordAsync(user.Id, model.Code, model.Password), Times.Once);
        }

        [TestMethod]
        public async Task PostResetPasswordReturnsModelErrorIfLoginFails()
        {
            ResetPasswordModel model = new ResetPasswordModel()
            {
                Email = "unit.test",
                Code = "code"
            };

            IdentityUser user = new IdentityUser(model.Email);

            mockUserManager.Setup(x => x.FindByEmailAsync(model.Email)).Returns(Task.FromResult(user));
            mockUserManager.Setup(x => x.ResetPasswordAsync(user.Id, model.Code, model.Password)).Returns(Task.FromResult(IdentityResult.Success));

            mockSignInManager.Setup(x => x.PasswordSignInAsync(model.Email, model.Password)).Returns(Task.FromResult(SignInStatus.Failure));

            AccountController controller = new AccountController(new QueryDispatcher(container), new CommandDispatcher(container));
            ActionResult actionResult = await controller.ResetPassword(model);

            mockUserLogger.Verify(x => x.LogActionAsync(UserActions.ResetPasswordFailure, model.Email, It.IsAny<string>()), Times.Once);
            mockUserManager.Verify(x => x.FindByEmailAsync(model.Email), Times.Once);
            mockUserManager.Verify(x => x.ResetPasswordAsync(user.Id, model.Code, model.Password), Times.Once);
            mockSignInManager.Verify(x => x.PasswordSignInAsync(model.Email, model.Password), Times.Once);
        }

        [TestMethod]
        public async Task PostResetPasswordReturnsViewIfResetPasswordSucceeds()
        {
            ResetPasswordModel model = new ResetPasswordModel()
            {
                Email = "unit.test",
                Code = "code",
                Password = "password"
            };

            IdentityUser user = new IdentityUser(model.Email);

            mockUserManager.Setup(x => x.FindByEmailAsync(model.Email)).Returns(Task.FromResult(user));
            mockUserManager.Setup(x => x.ResetPasswordAsync(user.Id, model.Code, model.Password)).Returns(Task.FromResult(IdentityResult.Success));

            mockSignInManager.Setup(x => x.PasswordSignInAsync(model.Email, model.Password)).Returns(Task.FromResult(SignInStatus.Success));

            AccountController controller = CreateController<AccountController>();
            ActionResult actionResult = await controller.ResetPassword(model);

            mockUserLogger.Verify(x => x.LogActionAsync(UserActions.ResetPasswordSuccess, model.Email), Times.Once);
            mockUserManager.Verify(x => x.FindByEmailAsync(model.Email), Times.Once);
            mockUserManager.Verify(x => x.ResetPasswordAsync(user.Id, model.Code, model.Password), Times.Once);
        }
    }
}
