using System;
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
            RuleFor(x => x.Email).NotEmpty().WithMessage("Please enter a username.");
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

            RuleFor(x => x.Email)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Please enter your email address.")
                .EmailAddress().WithMessage("Please enter a valid email address.")
                .MustAsync(HaveUniqueEmail).WithMessage("The username you entered is not available. Please enter a different username.");

            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Please enter your first name.");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Please enter your last name.");
        }

        public async Task<bool> HaveUniqueEmail(string email)
        {
            if (!_httpUser.Username.Equals(email, StringComparison.InvariantCultureIgnoreCase))
            {
                IdentityUser dupeUser = await _userManager.FindByEmailAsync(email);
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
                if (m.Email.IsNullOrWhiteSpace() && m.Username.IsNullOrWhiteSpace())
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
            RuleFor(x => x.Email).NotEmpty().WithMessage("Please enter a username.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Please enter a password.");
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("The password and confirmation password do not match.");
        }
    }

    public class ConfirmEmailModelValidator : AbstractValidator<ConfirmEmailModel>
    {
        public ConfirmEmailModelValidator()
        {
            RuleFor(x => x.EmailAddress).NotEmpty().WithMessage("Please enter your email address.");
            RuleFor(x => x.ConfirmationCode).NotEmpty().WithMessage("Please enter the confirmation code from your email.");
        }
    }
}