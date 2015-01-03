using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Areas.Account.Queries;
using CritterHeroes.Web.Areas.Admin.DataMaintenance.Handlers;
using CritterHeroes.Web.Areas.Common;
using CritterHeroes.Web.Common.Identity;
using CritterHeroes.Web.Common.Services.Commands;
using CritterHeroes.Web.Common.Services.Queries;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Queries;

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
            LoginModel model =  QueryDispatcher.Dispatch<LoginQuery, LoginModel>(query);
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
            CommandResult commandResult = await CommandDispatcher.DispatchAsync<ForgotPasswordModel>(model);
            if (commandResult.Succeeded)
            {
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
            EditProfileModel model = await QueryDispatcher.DispatchAsync<UserIDQuery, EditProfileModel>(query);
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

                CommandResult commandResult = await CommandDispatcher.DispatchAsync<EditProfileModel>(model);
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
            CheckUsernameResult queryResult = await QueryDispatcher.DispatchAsync<UsernameQuery, CheckUsernameResult>(new UsernameQuery()
            {
                Username = userName
            });
            return Json(queryResult.UserExists);
        }
    }
}