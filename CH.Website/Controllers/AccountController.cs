using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CH.Domain.Contracts.Identity;
using CH.Domain.Identity;
using CH.Website.Models;
using CH.Website.Utility;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace CH.Website.Controllers
{
    public class AccountController : BaseController
    {
        private IApplicationUserManager _userManager;
        private IAuthenticationManager _authenticationManager;

        public AccountController()
        {
        }

        public AccountController(IApplicationUserManager userManager, IAuthenticationManager authenticationManager)
        {
            _userManager = userManager;
            _authenticationManager = authenticationManager;
        }

        public IApplicationUserManager UserManager
        {
            get
            {
                if (_userManager == null)
                {
                    _userManager = Using<IApplicationUserManager>();
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
                IdentityUser user = await UserManager.FindAsync(model.Username, model.Password);
                if (user != null)
                {
                    ClaimsIdentity userIdentity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

                    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    AuthenticationManager.SignIn(new AuthenticationProperties()
                    {
                        IsPersistent = false
                    }, userIdentity);

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
            model.OriginalUsername = User.Identity.Name;
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
            if (ModelState.IsValid)
            {
                IdentityUser user = await UserManager.FindByNameAsync(User.Identity.Name);
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                IdentityResult identityResult = await UserManager.UpdateAsync(user);
                if (identityResult.Succeeded)
                {
                    return Redirect(model.ReturnUrl);
                }
                AddIdentityErrorsToModelState(ModelState, identityResult);
            }

            return View(model);
        }
    }
}