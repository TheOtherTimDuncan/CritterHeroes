using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AR.Domain.Identity;
using AR.Website.Identity;
using AR.Website.Models;
using AR.Website.Utility;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace AR.Website.Controllers
{
    public class AccountController : BaseController
    {
        private ApplicationUserManager _userManager;
        private IAuthenticationManager _authenticationManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
        {
            _userManager = userManager;
            _authenticationManager = authenticationManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                if (_userManager == null)
                {
                    _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
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
    }
}