using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using CH.Test.ControllerTests.TestHelpers;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Features.Account;
using CritterHeroes.Web.Features.Account.Models;
using CritterHeroes.Web.Features.Account.Queries;
using CritterHeroes.Web.Features.Admin.Critters;
using CritterHeroes.Web.Features.Common;
using CritterHeroes.Web.Features.Common.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CH.Test.ControllerTests
{
    [TestClass]
    public class AccountControllerTests : BaseTest
    {
        [TestMethod]
        public void LoginGetReturnsViewWithModel()
        {
            LoginModel model = new LoginModel();
            LoginQuery query = new LoginQuery();

            ControllerTester.UsingController<AccountController>()
                .SetupQueryDispatcher(model, query)
                .WithCallTo(x => x.Login(query))
                .VerifyQueryDispatcher()
                .ShouldReturnViewResult()
                .HavingModel(model);
        }

        [TestMethod]
        public void LoginPostReturnsViewWithModelIfModelStateIsInvalid()
        {
            LoginModel model = new LoginModel();

            ControllerTester.UsingController<AccountController>()
                .WithInvalidModelState("error")
                .WithCallTo(x => x.Login(model, null))
                .ShouldReturnViewResult()
                .HavingModel(model);
        }

        [TestMethod]
        public void LoginPostReturnsViewWithModelErrorsIfCommandHandlerFails()
        {
            LoginModel model = new LoginModel();
            string error = "error";

            ControllerTester.UsingController<AccountController>()
                .SetupCommandDispatcherForFailureAsync(model, error)
                .ShouldHaveValidModelState()
                .WithCallTo(x => x.Login(model, null))
                .VerifyCommandDispatcher()
                .ShouldHaveSingleModelError(error)
                .ShouldReturnViewResult()
                .HavingModel(model);
        }

        [TestMethod]
        public void LoginPostRedirectsToHomeIfModelStateIsValidAndCommandHandlerSucceedsAndNoReturnUrl()
        {
            LoginModel model = new LoginModel();

            ControllerTester.UsingController<AccountController>()
                .SetupCommandDispatcherForSuccessAsync(model)
                .ShouldHaveValidModelState()
                .WithCallTo(x => x.Login(model, null))
                .VerifyCommandDispatcher()
                .ShouldRedirectToLocal();
        }

        [TestMethod]
        public void LoginPostRedirectsToReturnUrlfModelStateIsValidAndCommandHandlerSucceedsAndHasReturnUrl()
        {
            LoginModel model = new LoginModel();
            string url = "/Account/EditProfile";

            ControllerTester.UsingController<AccountController>()
                .SetupCommandDispatcherForSuccessAsync(model)
                .ShouldHaveValidModelState()
                .WithCallTo(x => x.Login(model, url))
                .VerifyCommandDispatcher()
                .ShouldRedirectToLocal()
                .HavingUrl(url);
        }

        [TestMethod]
        public void LogoutRedirectsToHome()
        {
            ControllerTester.UsingController<AccountController>()
                .SetupCommandDispatcherForSuccess<LogoutModel>()
                .ShouldHaveValidModelState()
                .WithCallTo(x => x.LogOut())
                .VerifyCommandDispatcher()
                .ShouldRedirectToRoute()
                .HavingControllerRoute(CrittersController.Route)
                .HavingActionRoute(nameof(CrittersController.Index))
                .HavingRouteValues(AreaName.AdminRouteValue);
        }

        [TestMethod]
        public void EditProfileGetReturnsViewWithModel()
        {
            EditProfileModel model = new EditProfileModel();

            ControllerTester.UsingController<AccountController>()
                .SetupQueryDispatcherAsync(model)
                .WithCallTo(x => x.EditProfile())
                .VerifyQueryDispatcher()
                .ShouldReturnViewResult()
                .HavingModel(model);
        }

        [TestMethod]
        public void EditProfilePostReturnsViewWithModelIfModelStateIsInvalid()
        {
            EditProfileModel model = new EditProfileModel();

            ControllerTester.UsingController<AccountController>()
                .WithInvalidModelState("error")
                .WithCallTo(x => x.EditProfile(model))
                .ShouldReturnViewResult()
                .HavingModel(model);
        }

        [TestMethod]
        public void EditProfilePostReturnsViewWithModelErrorsIfCommandHandlerFails()
        {
            EditProfileModel model = new EditProfileModel();
            string error = "error";

            ControllerTester.UsingController<AccountController>()
                .SetupCommandDispatcherForFailureAsync(model, error)
                .WithCallTo(x => x.EditProfile(model))
                .VerifyCommandDispatcher()
                .ShouldHaveSingleModelError(error)
                .ShouldReturnViewResult()
                .HavingModel(model);
        }

        [TestMethod]
        public void EditProfilePostRedirectsIfModelStateIsValidAndCommandHandlerSucceeds()
        {
            EditProfileModel model = new EditProfileModel();

            ControllerTester.UsingController<AccountController>()
                .SetupCommandDispatcherForSuccessAsync(model)
                .ShouldHaveValidModelState()
                .WithCallTo(x => x.EditProfile(model))
                .VerifyCommandDispatcher()
                .ShouldRedirectToPrevious();
        }

        [TestMethod]
        public void ForgotPasswordGetReturnsPartialView()
        {
            ControllerTester.UsingController<AccountController>()
                .WithCallTo(x => x.ForgotPassword())
                .ShouldReturnPartialViewResult();
        }

        [TestMethod]
        public void ForgotPasswordPostReturnsJsonWithErrorIfModelStateIsInvalid()
        {
            ForgotPasswordModel model = new ForgotPasswordModel();
            string error = "error";

            ControllerTester.UsingController<AccountController>()
                .WithInvalidModelState(error)
                .WithCallTo(x => x.ForgotPassword(model))
                .ShouldReturnJsonCamelCase()
                .HavingStatusCode(HttpStatusCode.BadRequest)
                .HavingModel<JsonError>((errorModel) =>
                {
                    errorModel.Errors.Should().HaveCount(1);
                    errorModel.Errors.Should().Contain(error);
                });
        }

        [TestMethod]
        public void ForgotPasswordPostReturnJsonWithErrorIfModelStateIsValidAndCommandHandlerFails()
        {
            ForgotPasswordModel model = new ForgotPasswordModel();
            string error = "error";

            ControllerTester.UsingController<AccountController>()
                .SetupCommandDispatcherForFailureAsync(model, error)
                .ShouldHaveValidModelState()
                .WithCallTo(x => x.ForgotPassword(model))
                .VerifyCommandDispatcher()
                .ShouldReturnJsonCamelCase()
                .HavingStatusCode(HttpStatusCode.BadRequest)
                .HavingModel<JsonError>((errorModel) =>
                {
                    errorModel.Errors.Should().HaveCount(1);
                    errorModel.Errors.Should().Contain(error);
                });
        }

        [TestMethod]
        public void ForgotPasswordPostReturnsStatusCodeIfModelStateIsValidAndCommandHandlerSucceeds()
        {
            ForgotPasswordModel model = new ForgotPasswordModel();

            ControllerTester.UsingController<AccountController>()
                .SetupCommandDispatcherForSuccessAsync(model)
                .ShouldHaveValidModelState()
                .WithCallTo(x => x.ForgotPassword(model))
                .VerifyCommandDispatcher()
                .ShouldReturnStatusCode(HttpStatusCode.NoContent);
        }

        [TestMethod]
        public void ResetPasswordGetReturnsViewWithModel()
        {
            string code = "code";

            ControllerTester.UsingController<AccountController>()
                .WithCallTo(x => x.ResetPassword(code))
                .ShouldReturnViewResult()
                .HavingModel<ResetPasswordModel>((model) =>
                {
                    model.Code.Should().Be(code);
                });
        }

        [TestMethod]
        public void ResetPostReturnsViewWithModelIfModelStateIsInvalid()
        {
            ResetPasswordModel model = new ResetPasswordModel();

            ControllerTester.UsingController<AccountController>()
                .WithInvalidModelState("error")
                .WithCallTo(x => x.ResetPassword(model))
                .ShouldReturnViewResult()
                .HavingModel(model);
        }

        [TestMethod]
        public void ResetPasswordPostReturnsViewWithModelErrorsIfCommandHandlerFails()
        {
            ResetPasswordModel model = new ResetPasswordModel();
            string error = "error";

            ControllerTester.UsingController<AccountController>()
                .SetupCommandDispatcherForFailureAsync(model, error)
                .ShouldHaveValidModelState()
                .WithCallTo(x => x.ResetPassword(model))
                .VerifyCommandDispatcher()
                .ShouldHaveSingleModelError(error)
                .ShouldReturnViewResult()
                .HavingModel(model);
        }

        [TestMethod]
        public void ResetPasswordPostReturnsViewWithModelIfModelStateIsInvalidAndCommandHandlerSucceeds()
        {
            ResetPasswordModel model = new ResetPasswordModel();

            ControllerTester.UsingController<AccountController>()
                .SetupCommandDispatcherForSuccessAsync(model)
                .ShouldHaveValidModelState()
                .WithCallTo(x => x.ResetPassword(model))
                .VerifyCommandDispatcher()
                .ShouldReturnViewResult()
                .HavingModel(model);
        }

        [TestMethod]
        public void ConfirmEmailGetReturnsViewWithEmptyModelIfEmailIsMissing()
        {
            ControllerTester.UsingController<AccountController>()
                .WithCallTo(x => x.ConfirmEmail(null, "code"))
                .ShouldReturnViewResult()
                .HavingModel<ConfirmEmailModel>((model) =>
                {
                    model.Email.Should().BeNull();
                    model.ConfirmationCode.Should().BeNull();
                });
        }

        [TestMethod]
        public void ConfirmEmailGetReturnsViewWithEmptyModelIfCodeIsMissing()
        {
            ControllerTester.UsingController<AccountController>()
                .WithCallTo(x => x.ConfirmEmail("email@email.com", null))
                .ShouldReturnViewResult()
                .HavingModel<ConfirmEmailModel>((model) =>
                {
                    model.Email.Should().BeNull();
                    model.ConfirmationCode.Should().BeNull();
                });
        }

        [TestMethod]
        public void ConfirmEmailGetExecutesCommandHandlerIfEmailAndConfirmationCodeExist()
        {
            string email = "email@email.com";
            string code = "code";

            ControllerTester.UsingController<AccountController>()
                .SetupCommandDispatcherForSuccessAsync<ConfirmEmailModel>()
                .WithCallTo(x => x.ConfirmEmail(email, code))
                .VerifyCommandDispatcher()
                .ShouldReturnViewResult()
                .HavingModel<ConfirmEmailModel>((model) =>
                {
                    model.Email.Should().Be(email);
                    model.ConfirmationCode.Should().Be(code);
                });
        }

        [TestMethod]
        public void ConfirmEmailPostReturnsViewWithModelIfModelStateIsInvalid()
        {
            ConfirmEmailModel model = new ConfirmEmailModel();

            ControllerTester.UsingController<AccountController>()
                .WithInvalidModelState("error")
                .WithCallTo(x => x.ConfirmEmail(model))
                .ShouldReturnViewResult()
                .HavingModel(model);
        }

        [TestMethod]
        public void ConfirmEmailPostReturnsViewWithModelAndErrorsIfModelStateIsValidAndCommandHandlerFails()
        {
            ConfirmEmailModel model = new ConfirmEmailModel();
            string error = "error";

            ControllerTester.UsingController<AccountController>()
                .SetupCommandDispatcherForFailureAsync(model, error)
                .ShouldHaveValidModelState()
                .WithCallTo(x => x.ConfirmEmail(model))
                .VerifyCommandDispatcher()
                .ShouldHaveSingleModelError(error)
                .ShouldReturnViewResult()
                .HavingModel(model);
        }

        [TestMethod]
        public void ConfirmEmailPostReturnsViewWithModelWithNoErrorsIfModelStateIsValidAndCommandHandlerSucceeds()
        {
            ConfirmEmailModel model = new ConfirmEmailModel();

            ControllerTester.UsingController<AccountController>()
                .SetupCommandDispatcherForSuccessAsync(model)
                .ShouldHaveValidModelState()
                .WithCallTo(x => x.ConfirmEmail(model))
                .VerifyCommandDispatcher()
                .ShouldHaveValidModelState()
                .ShouldReturnViewResult()
                .HavingModel(model);
        }

        [TestMethod]
        public void EditProfileLoginGetReturnsPartialView()
        {
            ControllerTester.UsingController<AccountController>()
                .WithCallTo(x => x.EditProfileLogin())
                .ShouldReturnPartialViewResult();
        }

        [TestMethod]
        public void EditProfileLoginPostReturnsJsonAndBadRequestStatusIfModelStateIsInvalid()
        {
            EditProfileLoginModel model = new EditProfileLoginModel();
            string error = "error";

            ControllerTester.UsingController<AccountController>()
                .WithInvalidModelState("error")
                .WithCallTo(x => x.EditProfileLogin(model))
                .ShouldReturnJsonCamelCase()
                .HavingStatusCode(HttpStatusCode.BadRequest)
                .HavingModel<JsonError>((jsonError) =>
                {
                    jsonError.Errors.Should().HaveCount(1);
                    jsonError.Errors.Should().Contain(error);
                });
        }

        [TestMethod]
        public void EditProfileLoginPostReturnsJsonAndBadRequestIfModelStateIsValidAndCommandHandlerFails()
        {
            EditProfileLoginModel model = new EditProfileLoginModel();
            string error = "error";

            ControllerTester.UsingController<AccountController>()
                .ShouldHaveValidModelState()
                .SetupCommandDispatcherForFailureAsync(model, error)
                .WithCallTo(x => x.EditProfileLogin(model))
                .ShouldReturnJsonCamelCase()
                .HavingStatusCode(HttpStatusCode.BadRequest)
                .HavingModel<JsonError>((jsonError) =>
                {
                    jsonError.Errors.Should().HaveCount(1);
                    jsonError.Errors.Should().Contain(error);
                });
        }

        [TestMethod]
        public void EditProfileLoginPostReturnsStatusCodeIfModelStateIsValidAndCommandHandlerSucceeds()
        {
            EditProfileLoginModel model = new EditProfileLoginModel();

            ControllerTester.UsingController<AccountController>()
                .ShouldHaveValidModelState()
                .SetupCommandDispatcherForSuccessAsync(model)
                .WithCallTo(x => x.EditProfileLogin(model))
                .ShouldReturnStatusCode(HttpStatusCode.NoContent);
        }

        [TestMethod]
        public void EditProfileSecureGetReturnsPartialView()
        {
            ControllerTester.UsingController<AccountController>()
                .WithCallTo(x => x.EditProfileSecure())
                .ShouldReturnPartialViewResult();
        }

        [TestMethod]
        public void EditProfileLSecurePostReturnsJsonAndBadRequestStatusIfModelStateIsInvalid()
        {
            EditProfileSecureModel model = new EditProfileSecureModel();
            string error = "error";

            ControllerTester.UsingController<AccountController>()
                .WithInvalidModelState("error")
                .WithCallTo(x => x.EditProfileSecure(model))
                .ShouldReturnJsonCamelCase()
                .HavingStatusCode(HttpStatusCode.BadRequest)
                .HavingModel<JsonError>((jsonError) =>
                {
                    jsonError.Errors.Should().HaveCount(1);
                    jsonError.Errors.Should().Contain(error);
                });
        }

        [TestMethod]
        public void EditProfileSecurePostReturnsJsonAndBadRequestIfModelStateIsValidAndCommandHandlerFails()
        {
            EditProfileSecureModel model = new EditProfileSecureModel();
            string error = "error";

            ControllerTester.UsingController<AccountController>()
                .ShouldHaveValidModelState()
                .SetupCommandDispatcherForFailureAsync(model, error)
                .WithCallTo(x => x.EditProfileSecure(model))
                .ShouldReturnJsonCamelCase()
                .HavingStatusCode(HttpStatusCode.BadRequest)
                .HavingModel<JsonError>((jsonError) =>
                {
                    jsonError.Errors.Should().HaveCount(1);
                    jsonError.Errors.Should().Contain(error);
                });
        }

        [TestMethod]
        public void EditProfileSecurePostReturnsStatusCodeIfModelStateIsValidAndCommandHandlerSucceeds()
        {
            EditProfileSecureModel model = new EditProfileSecureModel();

            ControllerTester.UsingController<AccountController>()
                .ShouldHaveValidModelState()
                .SetupCommandDispatcherForSuccessAsync(model)
                .WithCallTo(x => x.EditProfileSecure(model))
                .ShouldReturnStatusCode(HttpStatusCode.NoContent);
        }
    }
}
