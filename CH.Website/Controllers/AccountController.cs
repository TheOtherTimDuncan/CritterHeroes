using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CH.Domain.Contracts.Commands;
using CH.Domain.Contracts.Queries;
using CH.Domain.Services.Commands;
using CH.Domain.Services.Queries;
using CH.Website.Models;
using CH.Website.Services.Queries;
using CH.Domain.Identity;

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
                CommandResult commandResult = await CommandDispatcher.Dispatch<LoginModel>(model);
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
            CommandDispatcher.Dispatch<LogoutModel>(new LogoutModel());
            return RedirectToAction("Index", "Home");
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