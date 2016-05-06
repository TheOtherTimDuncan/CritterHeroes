using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Features.Admin.Organizations.Models;
using FluentValidation;

namespace CritterHeroes.Web.Features.Admin.Organizations
{
    public class EditProfileModelValidator : AbstractValidator<EditProfileModel>
    {
        public EditProfileModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Please enter a name.")
                .Length(min: 1, max: 100).WithMessage("Please do not enter a name with more than 100 characters.");

            RuleFor(x => x.ShortName)
                .NotEmpty().WithMessage("Please enter an abbreviated name.")
                .Length(min: 1, max: 100).WithMessage("Please do not enter a name with more than 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Please enter an email address.")
                .EmailAddress().WithMessage("Please enter a valid email address.");

            RuleFor(x => x.TimeZoneID)
                .NotEmpty().WithMessage("Please select a time zone.");
        }
    }
}
