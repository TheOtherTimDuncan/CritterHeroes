using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CH.Domain.Contracts.Identity;
using CH.Domain.Contracts.Logging;
using CH.Domain.Identity;
using CH.Domain.Models.Logging;
using CH.Website.Models;
using CH.Website.Utility;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace CH.Website.Controllers
{
    public class AccountController : BaseController
    {
        private ApplicationUserManager _userManager;
        private IAuthenticationManager _authenticationManager;
        private IUserLogger _userLogger;

        public AccountController()
        {
            _userLogger = Using<IUserLogger>();
        }

        public AccountController(ApplicationUserManager userManager, IAuthenticationManager authenticationManager, IUserLogger userLogger)
        {
            _userManager = userManager;
            _authenticationManager = authenticationManager;
            _userLogger = userLogger;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                if (_userManager == null)
                {
                    _userManager = Using<ApplicationUserManager>();
                }
                return _userManager;
            }
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
        public ActionResult Login(string returnUrl)
        {
            LoginModel model = new LoginModel()
            {
                ReturnUrl = returnUrl
            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                ApplicationSignInManager signinManager = new ApplicationSignInManager(UserManager, AuthenticationManager);
                SignInStatus result = await signinManager.PasswordSignInAsync(model.Username, model.Password, isPersistent: false, shouldLockout: false);

                if (result == SignInStatus.Success)
                {
                    await _userLogger.LogAction(UserActions.PasswordLoginSuccess, model.Username);

                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }

                else
                {
                    ModelState.AddModelError("", "The username or password that you entered was incorrect. Please try again.");
                }
            }

            await _userLogger.LogAction(UserActions.PasswordLoginFailure, model.Username);

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
            EditProfileModel model = new EditProfileModel();
            model.ReturnUrl = Request.UrlReferrer.AbsoluteUri;

            IdentityUser user = await UserManager.FindByNameAsync(User.Identity.Name);
            model.Username = User.Identity.Name;
            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
            model.Email = user.Email;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditProfile(EditProfileModel model)
        {
            bool isUserNameChanged = false;
            if (!User.Identity.Name.Equals(model.Username, StringComparison.InvariantCultureIgnoreCase))
            {
                IdentityUser dupeUser = await UserManager.FindByNameAsync(model.Username);
                if (dupeUser != null)
                {
                    ModelState.AddModelError("", "The username you entered is not available. Please enter a different username.");
                }
                else
                {
                    isUserNameChanged = true;
                }
            }

            if (ModelState.IsValid)
            {
                IdentityUser user = await UserManager.FindByNameAsync(User.Identity.Name);
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;

                if (isUserNameChanged)
                {
                    user.UserName = model.Username;
                }

                IdentityResult identityResult = await UserManager.UpdateAsync(user);
                if (identityResult.Succeeded)
                {
                    if (isUserNameChanged)
                    {
                        await _userLogger.LogAction(UserActions.UsernameChanged, user.UserName, "Original username: " + User.Identity.Name);
                        AuthenticationManager.SignOut();
                        AuthenticationManager.SignIn(await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie));
                    }
                    return Redirect(model.ReturnUrl);
                }
                AddIdentityErrorsToModelState(ModelState, identityResult);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> IsDuplicateUsername(string userName)
        {
            await _userLogger.LogAction(UserActions.DuplicateUsernameCheck, User.Identity.Name, Request.UrlReferrer.AbsoluteUri);
            IdentityUser user = await UserManager.FindByNameAsync(userName);
            bool result = (user != null);
            return Json(result);
        }
    }
}