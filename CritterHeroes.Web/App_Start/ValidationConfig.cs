using System;
using CritterHeroes.Web.Common.Validation;
using FluentValidation.Mvc;

namespace CritterHeroes.Web
{
    public static class ValidationConfig
    {
        public static void ConfigureValidation(IServiceProvider serviceProvider)
        {
            FluentValidationModelValidatorProvider.Configure(provider =>
            {
                provider.ValidatorFactory = new ValidationFactory(serviceProvider);
            });
        }
    }
}