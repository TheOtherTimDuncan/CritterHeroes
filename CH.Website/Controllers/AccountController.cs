using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CH.Domain.Commands;
using CH.Domain.Contracts.Commands;
using CH.Domain.Contracts.Queries;
using CH.Website.Models;
using CH.Website.Services.Queries;
using Microsoft.Owin.Security;

namespace CH.Website.Controllers
{
    public class AccountController : BaseController
    {
        private IAuthenticationManager _authenticationManager;

        private IQueryDispatcher _queryDispatcher;
        private ICommandDispatcher _commandDispatcher;

        public AccountController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
            this._commandDispatcher = commandDispatcher;
        }

        public IAuthenticationManager AuthenticationManager
        {
            get
            {
                if (_authenticationManager == null)
                {
                    _authenticationManager = HttpContext.GetOwinContext().Authentication;
                }
                return _authenticationManager;
            }
        }

        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginQuery query)
        {
            LoginModel model = await _queryDispatcher.Dispatch<LoginQuery, LoginModel>(query);
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                CommandResult commandResult = await _commandDispatcher.Dispatch<LoginModel>(model);
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
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<ActionResult> EditProfile()
        {
            EditProfileQuery query = new EditProfileQuery()
            {
                Username = User.Identity.Name
            };
            EditProfileModel model = await _queryDispatcher.Dispatch<EditProfileQuery, EditProfileModel>(query);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditProfile(EditProfileModel model)
        {
            if (ModelState.IsValid)
            {
                model.OriginalUsername = User.Identity.Name;
                CommandResult commandResult = await _commandDispatcher.Dispatch<EditProfileModel>(model);
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
            CheckUsernameResult queryResult = await _queryDispatcher.Dispatch<CheckUsernameQuery, CheckUsernameResult>(new CheckUsernameQuery()
            {
                Username = userName
            });
            return Json(queryResult.UserExists);
        }
    }
}