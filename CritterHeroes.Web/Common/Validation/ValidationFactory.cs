using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;

namespace CritterHeroes.Web.Common.Validation
{
    public class ValidationFactory : ValidatorFactoryBase
    {
        private IServiceProvider _serviceProvider;

        public ValidationFactory(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public override IValidator CreateInstance(Type validatorType)
        {
            IValidator validator = (IValidator)_serviceProvider.GetService(validatorType);
            return validator;
        }
    }
}