﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Common.Identity;
using CritterHeroes.Web.Common.Queries;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Features.Account.Models;
using CritterHeroes.Web.Features.Account.Queries;
using CritterHeroes.Web.Features.Admin.Critters;
using CritterHeroes.Web.Features.Common;
using CritterHeroes.Web.Features.Common.Models;
using TOTD.Utility.StringHelpers;

namespace CritterHeroes.Web.Features.Account
{
    [Route(AccountController.Route + "/{action=index}")]
    public class AccountController : BaseController
    {
        public const string Route = "Account";

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
            return RedirectToAction(nameof(CrittersController.Index), CrittersController.Route, AreaName.AdminRouteValue);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return PartialView("_ForgotPassword", new ForgotPasswordModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                CommandResult commandResult = await CommandDispatcher.DispatchAsync(model);
                if (commandResult.Succeeded)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                return JsonCamelCase(JsonError.FromCommandResult(commandResult), HttpStatusCode.BadRequest);
            }

            return JsonCamelCase(JsonError.FromModelState(ModelState), HttpStatusCode.BadRequest);
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
                CommandResult commandResult = await CommandDispatcher.DispatchAsync(model);
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
        public async Task<ActionResult> EditProfile()
        {
            UserIDQuery query = new UserIDQuery()
            {
                UserID = User.GetUserID()
            };
            EditProfileModel model = await QueryDispatcher.DispatchAsync<EditProfileModel>(query);

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
                    return RedirectToPrevious();
                }
                else
                {
                    AddCommandResultErrorsToModelState(ModelState, commandResult);
                }
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult EditProfileLogin()
        {
            return PartialView("_EditProfileLogin");
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
                    return StatusCode(HttpStatusCode.NoContent);
                }
                return JsonCamelCase(JsonError.FromCommandResult(commandResult), HttpStatusCode.BadRequest);
            }

            return JsonCamelCase(JsonError.FromModelState(ModelState), HttpStatusCode.BadRequest);
        }

        [HttpGet]
        public ActionResult EditProfileSecure()
        {
            return PartialView("_EditProfileSecure");
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
                    return StatusCode(HttpStatusCode.NoContent);
                }
                return JsonCamelCase(JsonError.FromCommandResult(commandResult), HttpStatusCode.BadRequest);
            }

            return JsonCamelCase(JsonError.FromModelState(ModelState), HttpStatusCode.BadRequest);
        }
    }
}