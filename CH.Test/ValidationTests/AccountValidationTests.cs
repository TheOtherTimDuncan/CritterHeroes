using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Account;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Common.Identity;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Contracts.Storage;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CH.Test.ValidationTests
{
    [TestClass]
    public class LoginModelValidatorTests
    {
        public LoginModelValidator GetValidator()
        {
            return new LoginModelValidator();
        }

        [TestClass]
        public class UsernamePropertyTests : LoginModelValidatorTests
        {
            [TestMethod]
            public void ShouldHaveErrorWhenUsernameIsNull()
            {
                GetValidator().ShouldHaveValidationErrorFor(x => x.Email, (string)null);
            }

            [TestMethod]
            public void ShouldHaveErrorWhenUsernameIsEmpty()
            {
                GetValidator().ShouldHaveValidationErrorFor(x => x.Email, "");
            }

            [TestMethod]
            public void ShouldHaveErrorWhenUsernameIsWhitespace()
            {
                GetValidator().ShouldHaveValidationErrorFor(x => x.Email, "  ");
            }

            [TestMethod]
            public void ShouldNotHaveErrorWhenUsernameHasValue()
            {
                GetValidator().ShouldNotHaveValidationErrorFor(x => x.Email, "user.name");
            }
        }

        [TestClass]
        public class PasswordPropertyTests : LoginModelValidatorTests
        {
            [TestMethod]
            public void ShouldHaveErrorWhenPasswordIsNull()
            {
                GetValidator().ShouldHaveValidationErrorFor(x => x.Password, (string)null);
            }

            [TestMethod]
            public void ShouldHaveErrorWhenPasswordIsEmpty()
            {
                GetValidator().ShouldHaveValidationErrorFor(x => x.Password, "");
            }

            [TestMethod]
            public void ShouldHaveErrorWhenPasswordIsWhitespace()
            {
                GetValidator().ShouldHaveValidationErrorFor(x => x.Password, "  ");
            }

            [TestMethod]
            public void ShouldNotHaveErrorWhenPasswordHasValue()
            {
                GetValidator().ShouldNotHaveValidationErrorFor(x => x.Password, "password");
            }
        }
    }

    [TestClass]
    public class EditProfileModelValidatorTests
    {
        public EditProfileModelValidator GetValidator()
        {
            return new EditProfileModelValidator();
        }

        [TestClass]
        public class FirstNamePropertyTests : EditProfileModelValidatorTests
        {
            [TestMethod]
            public void ShouldHaveErrorWhenFirstNameIsNull()
            {
                GetValidator().ShouldHaveValidationErrorFor(x => x.FirstName, (string)null);
            }

            [TestMethod]
            public void ShouldHaveErrorWhenFirstNameIsEmpty()
            {
                GetValidator().ShouldHaveValidationErrorFor(x => x.FirstName, "");
            }

            [TestMethod]
            public void ShouldHaveErrorWhenFirstNameIsWhitespace()
            {
                GetValidator().ShouldHaveValidationErrorFor(x => x.FirstName, "  ");
            }

            [TestMethod]
            public void ShouldNotHaveErrorWhenFirstNameHasValue()
            {
                GetValidator().ShouldNotHaveValidationErrorFor(x => x.FirstName, "FirstName");
            }
        }

        [TestClass]
        public class LastNamePropertyTests : EditProfileModelValidatorTests
        {
            [TestMethod]
            public void ShouldHaveErrorWhenLastNameIsNull()
            {
                GetValidator().ShouldHaveValidationErrorFor(x => x.LastName, (string)null);
            }

            [TestMethod]
            public void ShouldHaveErrorWhenLastNameIsEmpty()
            {
                GetValidator().ShouldHaveValidationErrorFor(x => x.LastName, "");
            }

            [TestMethod]
            public void ShouldHaveErrorWhenLastNameIsWhitespace()
            {
                GetValidator().ShouldHaveValidationErrorFor(x => x.LastName, "  ");
            }

            [TestMethod]
            public void ShouldNotHaveErrorWhenLastNameHasValue()
            {
                GetValidator().ShouldNotHaveValidationErrorFor(x => x.LastName, "Lastname");
            }
        }
    }

    [TestClass]
    public class ForgotPasswordModelValidatorTests
    {
        public ForgotPasswordModelValidator GetValidator()
        {
            return new ForgotPasswordModelValidator();
        }

        [TestMethod]
        public void ShouldHaveErrorIfBothEmailAndUsernameAreNull()
        {
            ForgotPasswordModel model = new ForgotPasswordModel();
            ValidationResult validationResult = GetValidator().Validate(model);
            validationResult.IsValid.Should().BeFalse();
        }

        [TestMethod]
        public void ShouldHaveErrorIfBothEmailAndUsernameAreEmpty()
        {
            ForgotPasswordModel model = new ForgotPasswordModel()
            {
                Email = "",
                Username = ""
            };
            ValidationResult validationResult = GetValidator().Validate(model);
            validationResult.IsValid.Should().BeFalse();
        }

        [TestMethod]
        public void ShouldHaveErrorIfBothEmailAndUsernameAreWhitespace()
        {
            ForgotPasswordModel model = new ForgotPasswordModel()
            {
                Email = "  ",
                Username = "  "
            };
            ValidationResult validationResult = GetValidator().Validate(model);
            validationResult.IsValid.Should().BeFalse();
        }

        [TestMethod]
        public void ShouldNotHaveErrorIfEmailAddressHasValue()
        {
            ForgotPasswordModel model = new ForgotPasswordModel()
            {
                Email = "email@emailc.om",
            };
            ValidationResult validationResult = GetValidator().Validate(model);
            validationResult.IsValid.Should().BeTrue();
        }

        [TestMethod]
        public void ShouldNotHaveErrorIfUsernameHasValue()
        {
            ForgotPasswordModel model = new ForgotPasswordModel()
            {
                Username = "username",
            };
            ValidationResult validationResult = GetValidator().Validate(model);
            validationResult.IsValid.Should().BeTrue();
        }
    }

    [TestClass]
    public class ResetPasswordModelValidatorTests
    {
        public ResetPasswordModelValidator GetValidator()
        {
            return new ResetPasswordModelValidator();
        }

        [TestClass]
        public class ConfirmPasswordPropertyTests : ResetPasswordModelValidatorTests
        {
            [TestMethod]
            public void ShouldHaveErrorIfDoesNotMatchPassword()
            {
                ResetPasswordModel model = new ResetPasswordModel()
                {
                    Password = "password",
                    ConfirmPassword = "foobar"
                };
                GetValidator().ShouldHaveValidationErrorFor(x => x.ConfirmPassword, model);
            }

            [TestMethod]
            public void ShouldNotHaveErrorWhenConfirmPasswordMatchesPassword()
            {
                ResetPasswordModel model = new ResetPasswordModel()
                {
                    Password = "password",
                    ConfirmPassword = "password"
                };
                GetValidator().ShouldNotHaveValidationErrorFor(x => x.ConfirmPassword, model);
            }

        }

        [TestClass]
        public class UsernamePropertyTests : ResetPasswordModelValidatorTests
        {
            [TestMethod]
            public void ShouldHaveErrorWhenUsernameIsNull()
            {
                GetValidator().ShouldHaveValidationErrorFor(x => x.Email, (string)null);
            }

            [TestMethod]
            public void ShouldHaveErrorWhenUsernameIsEmpty()
            {
                GetValidator().ShouldHaveValidationErrorFor(x => x.Email, "");
            }

            [TestMethod]
            public void ShouldHaveErrorWhenUsernameIsWhitespace()
            {
                GetValidator().ShouldHaveValidationErrorFor(x => x.Email, "  ");
            }

            [TestMethod]
            public void ShouldNotHaveErrorWhenUsernameHasValue()
            {
                GetValidator().ShouldNotHaveValidationErrorFor(x => x.Email, "user.name");
            }
        }

        [TestClass]
        public class PasswordPropertyTests : ResetPasswordModelValidatorTests
        {
            [TestMethod]
            public void ShouldHaveErrorWhenPasswordIsNull()
            {
                GetValidator().ShouldHaveValidationErrorFor(x => x.Password, (string)null);
            }

            [TestMethod]
            public void ShouldHaveErrorWhenPasswordIsEmpty()
            {
                GetValidator().ShouldHaveValidationErrorFor(x => x.Password, "");
            }

            [TestMethod]
            public void ShouldHaveErrorWhenPasswordIsWhitespace()
            {
                GetValidator().ShouldHaveValidationErrorFor(x => x.Password, "  ");
            }

            [TestMethod]
            public void ShouldNotHaveErrorWhenPasswordHasValue()
            {
                GetValidator().ShouldNotHaveValidationErrorFor(x => x.Password, "password");
            }
        }
    }
    [TestClass]
    public class EditProfileLoginModelValidatorTests
    {
        public EditProfileLoginModelValidator GetValidator()
        {
            return new EditProfileLoginModelValidator();
        }

        [TestClass]
        public class PasswordPropertyTests : EditProfileLoginModelValidatorTests
        {
            [TestMethod]
            public void ShouldHaveErrorWhenPasswordIsNull()
            {
                GetValidator().ShouldHaveValidationErrorFor(x => x.Password, (string)null);
            }

            [TestMethod]
            public void ShouldHaveErrorWhenPasswordIsEmpty()
            {
                GetValidator().ShouldHaveValidationErrorFor(x => x.Password, "");
            }

            [TestMethod]
            public void ShouldHaveErrorWhenPasswordIsWhitespace()
            {
                GetValidator().ShouldHaveValidationErrorFor(x => x.Password, "  ");
            }

            [TestMethod]
            public void ShouldNotHaveErrorWhenPasswordHasValue()
            {
                GetValidator().ShouldNotHaveValidationErrorFor(x => x.Password, "password");
            }
        }
    }

    [TestClass]
    public class EditProfileSecureModelValidatorTests
    {
        public Mock<IApplicationUserManager> mockUserManager;
        public Mock<IHttpUser> mockHttpUser;

        public EditProfileSecureModelValidator GetValidator()
        {
            mockUserManager = new Mock<IApplicationUserManager>();

            mockHttpUser = new Mock<IHttpUser>();
            mockHttpUser.Setup(x => x.Username).Returns("user.name");

            return new EditProfileSecureModelValidator(mockUserManager.Object, mockHttpUser.Object);
        }

        [TestClass]
        public class EmailPropertyTests : EditProfileSecureModelValidatorTests
        {
            [TestMethod]
            public void ShouldHaveErrorWhenEmailIsNull()
            {
                GetValidator().ShouldHaveValidationErrorFor(x => x.Email, (string)null);
            }

            [TestMethod]
            public void ShouldHaveErrorWhenEmailIsEmpty()
            {
                GetValidator().ShouldHaveValidationErrorFor(x => x.Email, "");
            }

            [TestMethod]
            public void ShouldHaveErrorWhenEmailIsWhitespace()
            {
                GetValidator().ShouldHaveValidationErrorFor(x => x.Email, "  ");
            }

            [TestMethod]
            public void ShouldHaveErrorWhenEmailIsInvalid()
            {
                GetValidator().ShouldHaveValidationErrorFor(x => x.Email, "foobar");
            }

            [TestMethod]
            public void ShouldHaveErrorWhenUsernameMatchesExistingUser()
            {
                EditProfileSecureModelValidator validator = GetValidator();

                string email = "new@new.com";

                mockUserManager.Setup(x => x.FindByEmailAsync(email)).Returns(Task.FromResult(new IdentityUser(email)));

                validator.ShouldHaveValidationErrorFor(x => x.Email, email);

                mockUserManager.Verify(x => x.FindByEmailAsync(email), Times.Once);
                mockHttpUser.Verify(x => x.Username, Times.Once);
            }

            [TestMethod]
            public void ShouldNotHaveErrorWhenEmailHasValue()
            {
                GetValidator().ShouldNotHaveValidationErrorFor(x => x.Email, "email@email.com");
            }
        }
    }
}
