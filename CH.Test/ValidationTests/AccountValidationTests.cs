using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Account;
using CritterHeroes.Web.Areas.Account.Models;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CH.Test.ValidationTests
{
    [TestClass]
    public class LoginModelValidatorTests
    {
        [TestClass]
        public class UsernamePropertyTests
        {
            [TestMethod]
            public void ShouldHaveErrorWhenUsernameIsNull()
            {
                new LoginModelValidator().ShouldHaveValidationErrorFor(x => x.Username, (string)null);
            }

            [TestMethod]
            public void ShouldHaveErrorWhenUsernameIsEmpty()
            {
                new LoginModelValidator().ShouldHaveValidationErrorFor(x => x.Username, "");
            }

            [TestMethod]
            public void ShouldHaveErrorWhenUsernameIsWhitespace()
            {
                new LoginModelValidator().ShouldHaveValidationErrorFor(x => x.Username, "  ");
            }

            [TestMethod]
            public void ShouldNotHaveErrorWhenUsernameHasValue()
            {
                new LoginModelValidator().ShouldNotHaveValidationErrorFor(x => x.Username, "user.name");
            }
        }

        [TestClass]
        public class PasswordPropertyTests
        {
            [TestMethod]
            public void ShouldHaveErrorWhenPasswordIsNull()
            {
                new LoginModelValidator().ShouldHaveValidationErrorFor(x => x.Password, (string)null);
            }

            [TestMethod]
            public void ShouldHaveErrorWhenPasswordIsEmpty()
            {
                new LoginModelValidator().ShouldHaveValidationErrorFor(x => x.Password, "");
            }

            [TestMethod]
            public void ShouldHaveErrorWhenPasswordIsWhitespace()
            {
                new LoginModelValidator().ShouldHaveValidationErrorFor(x => x.Password, "  ");
            }

            [TestMethod]
            public void ShouldNotHaveErrorWhenPasswordHasValue()
            {
                new LoginModelValidator().ShouldNotHaveValidationErrorFor(x => x.Password, "password");
            }
        }
    }

    [TestClass]
    public class EditProfileModelValidatorTests
    {
        [TestClass]
        public class UsernamePropertyTests
        {
            [TestMethod]
            public void ShouldHaveErrorWhenUsernameIsNull()
            {
                new EditProfileModelValidator().ShouldHaveValidationErrorFor(x => x.Username, (string)null);
            }

            [TestMethod]
            public void ShouldHaveErrorWhenUsernameIsEmpty()
            {
                new EditProfileModelValidator().ShouldHaveValidationErrorFor(x => x.Username, "");
            }

            [TestMethod]
            public void ShouldHaveErrorWhenUsernameIsWhitespace()
            {
                new EditProfileModelValidator().ShouldHaveValidationErrorFor(x => x.Username, "  ");
            }

            [TestMethod]
            public void ShouldHaveErrorWhenUsernameIsLessThan4Characters()
            {
                new EditProfileModelValidator().ShouldHaveValidationErrorFor(x => x.Username, "123");
            }

            [TestMethod]
            public void ShouldHaveErrorWhenUsernameIsMoreThan255Characters()
            {
                new EditProfileModelValidator().ShouldHaveValidationErrorFor(x => x.Username, new String('x', 256));
            }

            [TestMethod]
            public void ShouldNotHaveErrorWhenUsernameHasValue()
            {
                new LoginModelValidator().ShouldNotHaveValidationErrorFor(x => x.Username, "user.name");
            }
        }

        [TestClass]
        public class FirstNamePropertyTests
        {
            [TestMethod]
            public void ShouldHaveErrorWhenFirstNameIsNull()
            {
                new EditProfileModelValidator().ShouldHaveValidationErrorFor(x => x.FirstName, (string)null);
            }

            [TestMethod]
            public void ShouldHaveErrorWhenFirstNameIsEmpty()
            {
                new EditProfileModelValidator().ShouldHaveValidationErrorFor(x => x.FirstName, "");
            }

            [TestMethod]
            public void ShouldHaveErrorWhenFirstNameIsWhitespace()
            {
                new EditProfileModelValidator().ShouldHaveValidationErrorFor(x => x.FirstName, "  ");
            }

            [TestMethod]
            public void ShouldNotHaveErrorWhenFirstNameHasValue()
            {
                new EditProfileModelValidator().ShouldNotHaveValidationErrorFor(x => x.Username, "FirstName");
            }
        }

        [TestClass]
        public class LastNamePropertyTests
        {
            [TestMethod]
            public void ShouldHaveErrorWhenLastNameIsNull()
            {
                new EditProfileModelValidator().ShouldHaveValidationErrorFor(x => x.LastName, (string)null);
            }

            [TestMethod]
            public void ShouldHaveErrorWhenLastNameIsEmpty()
            {
                new EditProfileModelValidator().ShouldHaveValidationErrorFor(x => x.LastName, "");
            }

            [TestMethod]
            public void ShouldHaveErrorWhenLastNameIsWhitespace()
            {
                new EditProfileModelValidator().ShouldHaveValidationErrorFor(x => x.LastName, "  ");
            }

            [TestMethod]
            public void ShouldNotHaveErrorWhenLastNameHasValue()
            {
                new EditProfileModelValidator().ShouldNotHaveValidationErrorFor(x => x.LastName, "Lastname");
            }
        }

        [TestClass]
        public class EmailPropertyTests
        {
            [TestMethod]
            public void ShouldHaveErrorWhenEmailIsNull()
            {
                new EditProfileModelValidator().ShouldHaveValidationErrorFor(x => x.Email, (string)null);
            }

            [TestMethod]
            public void ShouldHaveErrorWhenEmailIsEmpty()
            {
                new EditProfileModelValidator().ShouldHaveValidationErrorFor(x => x.Email, "");
            }

            [TestMethod]
            public void ShouldHaveErrorWhenEmailIsWhitespace()
            {
                new EditProfileModelValidator().ShouldHaveValidationErrorFor(x => x.Email, "  ");
            }

            [TestMethod]
            public void ShouldHaveErrorWhenEmailIsInvalid()
            {
                new EditProfileModelValidator().ShouldHaveValidationErrorFor(x => x.Email, "foobar");
            }

            [TestMethod]
            public void ShouldNotHaveErrorWhenEmailHasValue()
            {
                new EditProfileModelValidator().ShouldNotHaveValidationErrorFor(x => x.Email, "email@email.com");
            }
        }
    }

    [TestClass]
    public class ForgotPasswordModelValidatorTests
    {
        [TestMethod]
        public void ShouldHaveErrorIfBothEmailAndUsernameAreNull()
        {
            ForgotPasswordModel model = new ForgotPasswordModel();
            ValidationResult validationResult = new ForgotPasswordModelValidator().Validate(model);
            validationResult.IsValid.Should().BeFalse();
        }

        [TestMethod]
        public void ShouldHaveErrorIfBothEmailAndUsernameAreEmpty()
        {
            ForgotPasswordModel model = new ForgotPasswordModel()
            {
                EmailAddress="",
                Username=""
            };
            ValidationResult validationResult = new ForgotPasswordModelValidator().Validate(model);
            validationResult.IsValid.Should().BeFalse();
        }

        [TestMethod]
        public void ShouldHaveErrorIfBothEmailAndUsernameAreWhitespace()
        {
            ForgotPasswordModel model = new ForgotPasswordModel()
            {
                EmailAddress = "  ",
                Username = "  "
            };
            ValidationResult validationResult = new ForgotPasswordModelValidator().Validate(model);
            validationResult.IsValid.Should().BeFalse();
        }

        [TestMethod]
        public void ShouldNotHaveErrorIfEmailAddressHasValue()
        {
            ForgotPasswordModel model = new ForgotPasswordModel()
            {
                EmailAddress = "email@emailc.om",
            };
            ValidationResult validationResult = new ForgotPasswordModelValidator().Validate(model);
            validationResult.IsValid.Should().BeTrue();
        }

        [TestMethod]
        public void ShouldNotHaveErrorIfUsernameHasValue()
        {
            ForgotPasswordModel model = new ForgotPasswordModel()
            {
                Username = "username",
            };
            ValidationResult validationResult = new ForgotPasswordModelValidator().Validate(model);
            validationResult.IsValid.Should().BeTrue();
        }
    }
}
