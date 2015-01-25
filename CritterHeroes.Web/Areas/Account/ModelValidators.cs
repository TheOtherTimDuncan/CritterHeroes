using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Areas.Account.Models;
using FluentValidation;

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
        public EditProfileModelValidator()
        {
            RuleFor(x => x.Username)
                .Length(min: 4, max: 255).WithMessage("Please enter a username of at least 4 characters.")
                .NotEmpty().WithMessage("Please enter a username.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Please enter your email address.")
                .EmailAddress().WithMessage("Please enter a valid email address.");

            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Please enter your first name.");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Please enter your last name.");
        }
    }
}