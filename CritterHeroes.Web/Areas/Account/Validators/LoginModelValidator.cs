using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Areas.Account.Models;
using FluentValidation;

namespace CritterHeroes.Web.Areas.Account.Validators
{
    public class LoginModelValidator:AbstractValidator<LoginModel>
    {
        public LoginModelValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Please enter a username.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Please enter a password.");
        }
    }
}