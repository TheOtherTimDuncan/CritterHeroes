using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Areas.Account.Queries;
using CritterHeroes.Web.Areas.Common;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Common.Identity;
using CritterHeroes.Web.Common.Queries;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Queries;
using TOTD.Utility.StringHelpers;

namespace CritterHeroes.Web.Areas.Account
{
    public class AccountController : BaseController
    {
        public AccountController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
            : base(queryDispatcher, commandDispatcher)
        {
        }

        [AllowAnonymous]
        public ActionResult Login(LoginQuery query)
        {
            LoginModel model = QueryDispatcher.Dispatch(query);
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                CommandResult commandResult = await CommandDispatcher.DispatchAsync(model);
                if (commandResult.Succeeded)
                {
                    return RedirectToLocal(returnUrl);
                }
                AddCommandResultErrorsToModelState(ModelState, commandResult);
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
            if (ModelState.IsValid)
            {
                CommandResult commandResult = await CommandDispatcher.DispatchAsync(model);
                AddCommandResultErrorsToModelState(ModelState, commandResult);
            }

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
                CommandResult commandResult = await CommandDispatcher.DispatchAsync<ResetPasswordModel>(model);
                if (!commandResult.Succeeded)
                {
                    AddCommandResultErrorsToModelState(ModelState, commandResult);
                }
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string email, string confirmationCode)
        {
            if (email.IsNullOrWhiteSpace() || confirmationCode.IsNullOrWhiteSpace())
            {
                return View(new ConfirmEmailModel());
            }

            ConfirmEmailModel model = new ConfirmEmailModel()
            {
                Email = email,
                ConfirmationCode = confirmationCode
            };

            CommandResult commandResult = await CommandDispatcher.DispatchAsync(model);
            AddCommandResultErrorsToModelState(ModelState, commandResult);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(ConfirmEmailModel model)
        {
            if (ModelState.IsValid)
            {
                CommandResult commandResult = await CommandDispatcher.DispatchAsync(model);
                AddCommandResultErrorsToModelState(ModelState, commandResult);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> EditProfile(string returnUrl)
        {
            UserIDQuery query = new UserIDQuery()
            {
                UserID = User.GetUserID()
            };
            EditProfileModel model = await QueryDispatcher.DispatchAsync<EditProfileModel>(query);

            if (!returnUrl.IsNullOrEmpty())
            {
                model.ReturnUrl = returnUrl;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditProfile(EditProfileModel model)
        {
            if (ModelState.IsValid)
            {
                CommandResult commandResult = await CommandDispatcher.DispatchAsync(model);
                if (commandResult.Succeeded)
                {
                    return RedirectToLocal(model.ReturnUrl);
                }
                else
                {
                    AddCommandResultErrorsToModelState(ModelState, commandResult);
                }
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult EditProfileLogin(string returnUrl)
        {
            return View(new EditProfileLoginModel()
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditProfileLogin(EditProfileLoginModel model)
        {
            if (ModelState.IsValid)
            {
                CommandResult commandResult = await CommandDispatcher.DispatchAsync(model);
                if (commandResult.Succeeded)
                {
                    return RedirectToAction("EditProfileSecure", new
                    {
                        returnUrl = model.ReturnUrl
                    });
                }
                AddCommandResultErrorsToModelState(ModelState, commandResult);
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult EditProfileSecure(string returnUrl)
        {
            return View(new EditProfileSecureModel()
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditProfileSecure(EditProfileSecureModel model)
        {
            if (ModelState.IsValid)
            {
                CommandResult commandResult = await CommandDispatcher.DispatchAsync(model);
                if (commandResult.Succeeded)
                {
                    return RedirectToAction("EditProfile", new
                    {
                        returnUrl = model.ReturnUrl
                    });
                }
                AddCommandResultErrorsToModelState(ModelState, commandResult);
            }

            return View(model);
        }
    }
}