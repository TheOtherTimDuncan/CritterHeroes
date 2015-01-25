using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Areas.Account.Validators;
using FluentAssertions;
using FluentValidation;
using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CH.Test.ValidationTests
{
    [TestClass]
    public class AccountValidationTests
    {
        [TestClass]
        public class LoginModelValidatorTests
        {
            [TestMethod]
            public void ShouldHaveErrorWhenUsernameIsNull()
            {
                LoginModel model = new LoginModel();
                LoginModelValidator validator = new LoginModelValidator();
                validator.ShouldHaveValidationErrorFor(x => x.Username, model);
            }

            [TestMethod]
            public void ShouldHaveErrorWhenUsernameIsEmpty()
            {
                LoginModel model = new LoginModel()
                {
                    Username = ""
                };
                LoginModelValidator validator = new LoginModelValidator();
                validator.ShouldHaveValidationErrorFor(x => x.Username, model);
            }

            [TestMethod]
            public void ShouldHaveErrorWhenUsernameIsWhitespace()
            {
                LoginModel model = new LoginModel()
                {
                    Username = "  "
                };
                LoginModelValidator validator = new LoginModelValidator();
                validator.ShouldHaveValidationErrorFor(x => x.Username, model);
            }

            [TestMethod]
            public void ShouldNotHaveErrorWhenUsernameHasValue()
            {
                LoginModel model = new LoginModel()
                {
                    Username = "user.name"
                };
                LoginModelValidator validator = new LoginModelValidator();
                validator.ShouldNotHaveValidationErrorFor(x => x.Username, model);
            }

            [TestMethod]
            public void ShouldHaveErrorWhenPasswordIsNull()
            {
                LoginModel model = new LoginModel();
                LoginModelValidator validator = new LoginModelValidator();
                validator.ShouldHaveValidationErrorFor(x => x.Password, model);
            }

            [TestMethod]
            public void ShouldHaveErrorWhenPasswordIsEmpty()
            {
                LoginModel model = new LoginModel()
                {
                    Password = ""
                };
                LoginModelValidator validator = new LoginModelValidator();
                validator.ShouldHaveValidationErrorFor(x => x.Password, model);
            }

            [TestMethod]
            public void ShouldHaveErrorWhenPasswordIsWhitespace()
            {
                LoginModel model = new LoginModel()
                {
                    Password = "  "
                };
                LoginModelValidator validator = new LoginModelValidator();
                validator.ShouldHaveValidationErrorFor(x => x.Password, model);
            }

            [TestMethod]
            public void ShouldNotHaveErrorWhenPasswordHasValue()
            {
                LoginModel model = new LoginModel()
                {
                    Password = "user.name"
                };
                LoginModelValidator validator = new LoginModelValidator();
                validator.ShouldNotHaveValidationErrorFor(x => x.Password, model);
            }
        }
    }
}
