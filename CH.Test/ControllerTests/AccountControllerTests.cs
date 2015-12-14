using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Account;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Areas.Account.Queries;
using CritterHeroes.Web.Areas.Admin.Critters;
using CritterHeroes.Web.Areas.Models;
using CritterHeroes.Web.Common.ActionResults;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Common.Queries;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CH.Test.ControllerTests
{
    [TestClass]
    public class AccountControllerTests : BaseControllerTest
    {
        [TestMethod]
        public void LoginGetReturnsViewWithModel()
        {
            LoginModel model = new LoginModel();
            LoginQuery query = new LoginQuery();
            ViewResult result = TestControllerGet<AccountController, ViewResult, LoginModel>(query, model, (controller) => controller.Login(query));
            result.Model.Should().Be(model);
        }

        [TestMethod]
        public async Task LoginPostReturnsViewWithModelIfModelStateIsInvalid()
        {
            LoginModel model = new LoginModel();
            ViewResult result = await TestControllerPostWithInvalidModelStateAsync<AccountController, ViewResult>("error", async (controller) => await controller.Login(model, null));
            result.Model.Should().Be(model);
        }

        [TestMethod]
        public async Task LoginPostReturnsViewWithModelErrorsIfCommandHandlerFails()
        {
            LoginModel model = new LoginModel();
            string error = "error";
            ViewResult result = await TestControllerPostFailWithValidModelStateAsync<AccountController, ViewResult>(model, error, async (controller) => await controller.Login(model, null));
            result.Model.Should().Be(model);
        }

        [TestMethod]
        public async Task LoginPostRedirectsToHomeIfModelStateIsValidAndCommandHandlerSucceedsAndNoReturnUrl()
        {
            LoginModel model = new LoginModel();
            RedirectToLocalResult result = await TestControllerPostSuccessWithValidModelStateAsync<AccountController, RedirectToLocalResult>(model, async (controller) => await controller.Login(model, null));
        }

        [TestMethod]
        public async Task LoginPostRedirectsToReturnUrlfModelStateIsValidAndCommandHandlerSucceedsAndHasReturnUrl()
        {
            LoginModel model = new LoginModel();
            string url = "/Account/EditProfile";
            RedirectToLocalResult result = await TestControllerPostSuccessWithValidModelStateAsync<AccountController, RedirectToLocalResult>(model, async (controller) => await controller.Login(model, url));
            result.Url.Should().Be(url);
        }

        [TestMethod]
        public void LogoutRedirectsToHome()
        {
            mockCommandDispatcher.Setup(x => x.Dispatch(It.IsAny<LogoutModel>())).Returns(CommandResult.Success());

            RedirectToRouteResult result = TestController<AccountController, RedirectToRouteResult>((controller) => controller.LogOut());
            VerifyRedirectToRouteResult(result, nameof(CrittersController.Index), CrittersController.Route);

            mockCommandDispatcher.Verify(x => x.Dispatch(It.IsAny<LogoutModel>()), Times.Once);
        }

        [TestMethod]
        public async Task EditProfileGetReturnsViewWithModel()
        {
            EditProfileModel model = new EditProfileModel();

            mockQueryDispatcher.Setup(x => x.DispatchAsync<EditProfileModel>(It.IsAny<UserIDQuery>())).Returns(Task.FromResult(model));

            ViewResult result = await TestControllerAsync<AccountController, ViewResult>(async (controller) => await controller.EditProfile());
            result.Model.Should().Be(model);

            mockQueryDispatcher.Verify(x => x.DispatchAsync<EditProfileModel>(It.IsAny<UserIDQuery>()), Times.Once);
        }

        [TestMethod]
        public async Task EditProfilePostReturnsViewWithModelIfModelStateIsInvalid()
        {
            EditProfileModel model = new EditProfileModel();
            ViewResult result = await TestControllerPostWithInvalidModelStateAsync<AccountController, ViewResult>("error", async (controller) => await controller.EditProfile(model));
            result.Model.Should().Be(model);
        }

        [TestMethod]
        public async Task EditProfilePostReturnsViewWithModelErrorsIfCommandHandlerFails()
        {
            EditProfileModel model = new EditProfileModel();
            string error = "error";
            ViewResult result = await TestControllerPostFailWithValidModelStateAsync<AccountController, ViewResult>(model, error, async (controller) => await controller.EditProfile(model));
            result.Model.Should().Be(model);
        }

        [TestMethod]
        public async Task EditProfilePostRedirectsIfModelStateIsValidAndCommandHandlerSucceeds()
        {
            EditProfileModel model = new EditProfileModel();
            RedirectToPreviousResult result = await TestControllerPostSuccessWithValidModelStateAsync<AccountController, RedirectToPreviousResult>(model, async (controller) => await controller.EditProfile(model));
        }

        [TestMethod]
        public void ForgotPasswordGetReturnsPartialView()
        {
            PartialViewResult result = TestController<AccountController, PartialViewResult>((controller) => controller.ForgotPassword());
        }

        [TestMethod]
        public async Task ForgotPasswordPostReturnsJsonWithErrorIfModelStateIsInvalid()
        {
            ForgotPasswordModel model = new ForgotPasswordModel();

            string error = "error";

            JsonResult result = await TestControllerPostWithInvalidModelStateAsync<AccountController, JsonResult>(error, async (controller) => await controller.ForgotPassword(model));

            JsonCommandResult resultModel = result.Data as JsonCommandResult;
            resultModel.Should().NotBeNull();

            resultModel.Succeeded.Should().BeFalse();
            resultModel.Message.Should().Be(error);
        }

        [TestMethod]
        public async Task ForgotPasswordPostReturnJsonWithErrorIfModelStateIsValidAndCommandHandlerFails()
        {
            ForgotPasswordModel model = new ForgotPasswordModel();

            string error = "error";

            JsonResult result = await TestControllerPostFailWithValidModelStateAsync<AccountController, JsonResult>(model, error, async (controller) => await controller.ForgotPassword(model));

            JsonCommandResult resultModel = result.Data as JsonCommandResult;
            resultModel.Should().NotBeNull();

            resultModel.Succeeded.Should().BeFalse();
            resultModel.Message.Should().Be(error);
        }

        [TestMethod]
        public async Task ForgotPasswordPostReturnJsonIfModelStateIsValidAndCommandHandlerSucceeds()
        {
            ForgotPasswordModel model = new ForgotPasswordModel();
            JsonResult result = await TestControllerPostSuccessWithValidModelStateAsync<AccountController, JsonResult>(model, async (controller) => await controller.ForgotPassword(model));

            JsonCommandResult resultModel = result.Data as JsonCommandResult;
            resultModel.Should().NotBeNull();

            resultModel.Succeeded.Should().BeTrue();
        }

        [TestMethod]
        public void ResetPasswordGetReturnsViewWithModel()
        {
            string code = "code";

            ViewResult result = TestController<AccountController, ViewResult>((controller) => controller.ResetPassword(code));

            ResetPasswordModel model = result.Model as ResetPasswordModel;
            model.Should().NotBeNull();
            model.Code.Should().Be(code);
        }

        [TestMethod]
        public async Task ResetPostReturnsViewWithModelIfModelStateIsInvalid()
        {
            ResetPasswordModel model = new ResetPasswordModel();
            ViewResult result = await TestControllerPostWithInvalidModelStateAsync<AccountController, ViewResult>("error", async (controller) => await controller.ResetPassword(model));
            result.Model.Should().Be(model);
        }

        [TestMethod]
        public async Task ResetPasswordPostReturnsViewWithModelErrorsIfCommandHandlerFails()
        {
            ResetPasswordModel model = new ResetPasswordModel();
            string error = "error";
            ViewResult result = await TestControllerPostFailWithValidModelStateAsync<AccountController, ViewResult>(model, error, async (controller) => await controller.ResetPassword(model));
            result.Model.Should().Be(model);
        }

        [TestMethod]
        public async Task ConfirmEmailGetReturnsViewWithEmptyModelIfEmailIsMissing()
        {
            ViewResult result = await TestControllerAsync<AccountController, ViewResult>(async (controller) => await controller.ConfirmEmail(email: null, confirmationCode: "code"));
            ConfirmEmailModel model = result.Model as ConfirmEmailModel;
            model.Should().NotBeNull();
            model.Email.Should().BeNull();
            model.ConfirmationCode.Should().BeNull();
        }

        [TestMethod]
        public async Task ConfirmEmailGetReturnsViewWithEmptyModelIfCodeIsMissing()
        {
            ViewResult result = await TestControllerAsync<AccountController, ViewResult>(async (controller) => await controller.ConfirmEmail(email: "email@email.com", confirmationCode: null));
            ConfirmEmailModel model = result.Model as ConfirmEmailModel;
            model.Should().NotBeNull();
            model.Email.Should().BeNull();
            model.ConfirmationCode.Should().BeNull();
        }

        [TestMethod]
        public async Task ConfirmEmailGetExecutesCommandHandlerIfEmailAndConfirmationCodeExist()
        {
            string email = "email@email.com";
            string code = "code";

            mockCommandDispatcher.Setup(x => x.DispatchAsync(It.IsAny<ConfirmEmailModel>())).Returns(Task.FromResult(CommandResult.Success()));

            ViewResult result = await TestControllerAsync<AccountController, ViewResult>(async (controller) => await controller.ConfirmEmail(email, code));

            ConfirmEmailModel model = result.Model as ConfirmEmailModel;
            model.Should().NotBeNull();

            model.Email.Should().Be(email);
            model.ConfirmationCode.Should().Be(code);

            mockCommandDispatcher.Verify(x => x.DispatchAsync(It.IsAny<ConfirmEmailModel>()), Times.Once);
        }
    }
}
