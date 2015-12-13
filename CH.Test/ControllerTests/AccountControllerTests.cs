using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Account;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Areas.Account.Queries;
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

            mockQueryDispatcher.Setup(x => x.Dispatch(query)).Returns(model);

            ViewResult result = TestController<AccountController, ViewResult>((controller) => controller.Login(query));
            result.Model.Should().Be(model);

            mockQueryDispatcher.Verify(x => x.Dispatch(query), Times.Once);
        }

        [TestMethod]
        public async Task LoginPostReturnsViewWithModelIfModelStateIsInvalid()
        {
            LoginModel model = new LoginModel();

            ViewResult result = await TestController<AccountController, ViewResult>(async (controller) =>
            {
                controller.ModelState.AddModelError("", "test");
                controller.ModelState.IsValid.Should().BeFalse();

                return await controller.Login(model, null);
            });

            result.Model.Should().Be(model);
        }

        [TestMethod]
        public async Task LoginPostReturnsViewWithModelErrorsIfCommandHandlerFails()
        {
            LoginModel model = new LoginModel();

            string error = "error";

            mockCommandDispatcher.Setup(x => x.DispatchAsync(model)).Returns(Task.FromResult(CommandResult.Failed(error)));

            ViewResult result = await TestController<AccountController, ViewResult>(async (controller) =>
             {
                 controller.ModelState.IsValid.Should().BeTrue();

                 ActionResult actionResult = await controller.Login(model, null);

                 ModelState modelState = controller.ModelState[""];
                 modelState.Errors.Should().HaveCount(1);
                 modelState.Errors[0].ErrorMessage.Should().Be(error);

                 return actionResult;
             });

            result.Model.Should().Be(model);

            mockCommandDispatcher.Verify(x => x.DispatchAsync(model), Times.Once);
        }

        [TestMethod]
        public async Task LoginPostRedirectsToHomeIfModelStateIsValidAndCommandHandlerSucceedsAndNoReturnUrl()
        {
            LoginModel model = new LoginModel();

            mockCommandDispatcher.Setup(x => x.DispatchAsync(model)).Returns(Task.FromResult(CommandResult.Success()));

            RedirectToLocalResult result = await TestController<AccountController, RedirectToLocalResult>(async (controller) =>
            {
                controller.ModelState.IsValid.Should().BeTrue();

                return await controller.Login(model, null);
            });

            mockCommandDispatcher.Verify(x => x.DispatchAsync(model), Times.Once);
        }

        [TestMethod]
        public async Task LoginPostRedirectsToReturnUrlfModelStateIsValidAndCommandHandlerSucceedsAndHasReturnUrl()
        {
            LoginModel model = new LoginModel();

            string url = "/Account/EditProfile";

            mockCommandDispatcher.Setup(x => x.DispatchAsync(model)).Returns(Task.FromResult(CommandResult.Success()));

            RedirectToLocalResult result = await TestController<AccountController, RedirectToLocalResult>(async (controller) =>
            {
                controller.ModelState.IsValid.Should().BeTrue();

                return await controller.Login(model, url);
            });

            result.Url.Should().Be(url);

            mockCommandDispatcher.Verify(x => x.DispatchAsync(model), Times.Once);
        }

        [TestMethod]
        public async Task EditProfileGetReturnsViewWithModel()
        {
            EditProfileModel model = new EditProfileModel();

            mockQueryDispatcher.Setup(x => x.DispatchAsync<EditProfileModel>(It.IsAny<UserIDQuery>())).Returns(Task.FromResult(model));

            ViewResult result = await TestController<AccountController, ViewResult>(async (controller) => await controller.EditProfile());
            result.Model.Should().Be(model);

            mockQueryDispatcher.Verify(x => x.DispatchAsync<EditProfileModel>(It.IsAny<UserIDQuery>()), Times.Once);
        }

        [TestMethod]
        public async Task EditProfilePostReturnsViewWithModelIfModelStateIsInvalid()
        {
            EditProfileModel model = new EditProfileModel();

            ViewResult result = await TestController<AccountController, ViewResult>(async (controller) =>
            {
                controller.ModelState.AddModelError("", "test");
                controller.ModelState.IsValid.Should().BeFalse();

                return await controller.EditProfile(model);
            });

            result.Model.Should().Be(model);
        }

        [TestMethod]
        public async Task EditProfilePostReturnsViewWithModelErrorsIfCommandHandlerFails()
        {
            EditProfileModel model = new EditProfileModel();

            string error = "error";

            mockCommandDispatcher.Setup(x => x.DispatchAsync(model)).Returns(Task.FromResult(CommandResult.Failed(error)));

            ViewResult result = await TestController<AccountController, ViewResult>(async (controller) =>
            {
                controller.ModelState.IsValid.Should().BeTrue();

                ActionResult actionResult = await controller.EditProfile(model);

                ModelState modelState = controller.ModelState[""];
                modelState.Errors.Should().HaveCount(1);
                modelState.Errors[0].ErrorMessage.Should().Be(error);

                return actionResult;
            });

            result.Model.Should().Be(model);

            mockCommandDispatcher.Verify(x => x.DispatchAsync(model), Times.Once);
        }

        [TestMethod]
        public async Task EditProfilePostRedirectsIfModelStateIsValidAndCommandHandlerSucceeds()
        {
            EditProfileModel model = new EditProfileModel();

            mockCommandDispatcher.Setup(x => x.DispatchAsync(model)).Returns(Task.FromResult(CommandResult.Success()));

            RedirectToPreviousResult result = await TestController<AccountController, RedirectToPreviousResult>(async (controller) =>
            {
                controller.ModelState.IsValid.Should().BeTrue();

                return await controller.EditProfile(model);
            });

            mockCommandDispatcher.Verify(x => x.DispatchAsync(model), Times.Once);
        }

        [TestMethod]
        public async Task ForgotPasswordPostReturnsJsonWithErrorIfModelStateIsInvalid()
        {
            ForgotPasswordModel model = new ForgotPasswordModel();

            string error = "error";

            JsonResult result = await TestController<AccountController, JsonResult>(async (controller) =>
            {
                controller.ModelState.AddModelError("", error);
                controller.ModelState.IsValid.Should().BeFalse();

                return await controller.ForgotPassword(model);
            });

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

            mockCommandDispatcher.Setup(x => x.DispatchAsync(model)).Returns(Task.FromResult(CommandResult.Failed(error)));

            JsonResult result = await TestController<AccountController, JsonResult>(async (controller) =>
            {
                controller.ModelState.IsValid.Should().BeTrue();

                return await controller.ForgotPassword(model);
            });

            JsonCommandResult resultModel = result.Data as JsonCommandResult;
            resultModel.Should().NotBeNull();

            resultModel.Succeeded.Should().BeFalse();
            resultModel.Message.Should().Be(error);

            mockCommandDispatcher.Verify(x => x.DispatchAsync(model), Times.Once);
        }

        [TestMethod]
        public async Task ForgotPasswordPostReturnJsonIfModelStateIsValidAndCommandHandlerSucceeds()
        {
            ForgotPasswordModel model = new ForgotPasswordModel();

            mockCommandDispatcher.Setup(x => x.DispatchAsync(model)).Returns(Task.FromResult(CommandResult.Success()));

            JsonResult result = await TestController<AccountController, JsonResult>(async (controller) =>
            {
                controller.ModelState.IsValid.Should().BeTrue();

                return await controller.ForgotPassword(model);
            });

            JsonCommandResult resultModel = result.Data as JsonCommandResult;
            resultModel.Should().NotBeNull();

            resultModel.Succeeded.Should().BeTrue();

            mockCommandDispatcher.Verify(x => x.DispatchAsync(model), Times.Once);
        }

        [TestMethod]
        public async Task ResetPostReturnsViewWithModelIfModelStateIsInvalid()
        {
            ResetPasswordModel model = new ResetPasswordModel();

            ViewResult result = await TestController<AccountController, ViewResult>(async (controller) =>
            {
                controller.ModelState.AddModelError("", "test");
                controller.ModelState.IsValid.Should().BeFalse();

                return await controller.ResetPassword(model);
            });

            result.Model.Should().Be(model);
        }

        [TestMethod]
        public async Task ResetPasswordPostReturnsViewWithModelErrorsIfCommandHandlerFails()
        {
            ResetPasswordModel model = new ResetPasswordModel();

            string error = "error";

            mockCommandDispatcher.Setup(x => x.DispatchAsync(model)).Returns(Task.FromResult(CommandResult.Failed(error)));

            ViewResult result = await TestController<AccountController, ViewResult>(async (controller) =>
            {
                controller.ModelState.IsValid.Should().BeTrue();

                ActionResult actionResult = await controller.ResetPassword(model);

                ModelState modelState = controller.ModelState[""];
                modelState.Errors.Should().HaveCount(1);
                modelState.Errors[0].ErrorMessage.Should().Be(error);

                return actionResult;
            });

            result.Model.Should().Be(model);

            mockCommandDispatcher.Verify(x => x.DispatchAsync(model), Times.Once);
        }
    }
}
