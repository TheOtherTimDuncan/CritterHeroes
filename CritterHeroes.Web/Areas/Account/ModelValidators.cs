﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Common.Identity;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Contracts.Storage;
using FluentValidation;
using FluentValidation.Results;
using TOTD.Utility.StringHelpers;

namespace CritterHeroes.Web.Areas.Account
{
    public class LoginModelValidator : AbstractValidator<LoginModel>
    {
        public LoginModelValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Please enter a username.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Please enter a password.");
        }
    }

    public class EditProfileModelValidator : AbstractValidator<EditProfileModel>
    {
        private IApplicationUserManager _userManager;
        private IHttpUser _httpUser;

        public EditProfileModelValidator(IApplicationUserManager storageContext, IHttpUser httpUser)
        {
            this._userManager = storageContext;
            this._httpUser = httpUser;

            RuleFor(x => x.Username)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Please enter a username.")
                .Length(min: 4, max: 255).WithMessage("Please enter a username of at least 4 characters.")
                .MustAsync(HaveUniqueUsername).WithMessage("The username you entered is not available. Please enter a different username.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Please enter your email address.")
                .EmailAddress().WithMessage("Please enter a valid email address.");

            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Please enter your first name.");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Please enter your last name.");
        }

        public async Task<bool> HaveUniqueUsername(string userName)
        {
            if (!_httpUser.Username.Equals(userName, StringComparison.InvariantCultureIgnoreCase))
            {
                IdentityUser dupeUser = await _userManager.FindByNameAsync(userName);
                if (dupeUser != null)
                {
                    return false;
                }
            }

            return true;
        }
    }

    public class ForgotPasswordModelValidator : AbstractValidator<ForgotPasswordModel>
    {
        public ForgotPasswordModelValidator()
        {
            Custom(m =>
            {
                if (m.EmailAddress.IsNullOrWhiteSpace() && m.Username.IsNullOrWhiteSpace())
                {
                    return new ValidationFailure("", "Please enter your email address or your username.");
                }

                return null;
            });
        }
    }

    public class ResetPasswordModelValidator : AbstractValidator<ResetPasswordModel>
    {
        public ResetPasswordModelValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Please enter a username.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Please enter a password.");
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("The password and confirmation password do not match.");
        }
    }
}