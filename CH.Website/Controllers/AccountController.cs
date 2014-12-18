using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CH.Domain.Contracts.Commands;
using CH.Domain.Contracts.Queries;
using CH.Domain.Identity;
using CH.Domain.Services.Commands;
using CH.Domain.Services.Queries;
using CH.Website.Models.Account;
using CH.Website.Services.CommandHandlers;
using CH.Website.Services.Commands;
using CH.Website.Services.Queries;
using CH.Website.Utility;

namespace CH.Website.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
            : base(queryDispatcher, commandDispatcher)
        {
        }

        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginQuery query)
        {
            LoginModel model = await QueryDispatcher.Dispatch<LoginQuery, LoginModel>(query);
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                CommandResult commandResult = await CommandDispatcher.Dispatch(model);
                if (commandResult.Succeeded)
                {
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    AddCommandResultErrorsToModelState(ModelState, commandResult);
                }
            }

            return View(model);
        }

        public ActionResult LogOut()
        {
            CommandDispatcher.Dispatch(new LogoutModel());
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View(new ForgotPasswordModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            ForgotPasswordCommand command = new ForgotPasswordCommand()
            {
                EmailAddress = model.EmailAddress,
                Username = model.Username,
                UrlGenerator = new UrlGenerator(Url),
                OrganizationFullName = OrganizationContext.FullName,
                OrganizationEmailAddress = OrganizationContext.EmailAddress
            };

            ModalDialogCommandResult commandResult = await CommandDispatcher.Dispatch<ForgotPasswordCommand, ModalDialogCommandResult>(command);

            if (commandResult.Succeeded)
            {
                model.ModalDialog = commandResult.ModalDialog;
                return View(model);
            }

            AddCommandResultErrorsToModelState(ModelState, commandResult);
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            ResetPasswordModel model = new ResetPasswordModel()
            {
                Code = code
            };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                CommandResult commandResult = await CommandDispatcher.Dispatch(model);
                if (commandResult.Succeeded)
                {
                    model.ShowMessage = true;
                }
                else
                {
                    AddCommandResultErrorsToModelState(ModelState, commandResult);
                }
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotUsername()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ResendConfirmationCode()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ConfirmEmail()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> EditProfile()
        {
            UserIDQuery query = new UserIDQuery()
            {
                UserID = User.GetUserID()
            };
            EditProfileModel model = await QueryDispatcher.Dispatch<UserIDQuery, EditProfileModel>(query);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditProfile(EditProfileModel model)
        {
            if (ModelState.IsValid)
            {
                model.OriginalUsername = User.Identity.Name;
                model.UserID = User.GetUserID();

                CommandResult commandResult = await CommandDispatcher.Dispatch<EditProfileModel>(model);
                if (commandResult.Succeeded)
                {
                    return Redirect(model.ReturnUrl);
                }
                else
                {
                    AddCommandResultErrorsToModelState(ModelState, commandResult);
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> IsDuplicateUsername(string userName)
        {
            CheckUsernameResult queryResult = await QueryDispatcher.Dispatch<UsernameQuery, CheckUsernameResult>(new UsernameQuery()
            {
                Username = userName
            });
            return Json(queryResult.UserExists);
        }
    }
}