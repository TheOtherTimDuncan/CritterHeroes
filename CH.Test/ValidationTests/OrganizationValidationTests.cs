using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Features.Admin.Organizations;
using FluentAssertions;
using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CH.Test.ValidationTests
{
    [TestClass]
    public class EditProfileValidationTests
    {
        public EditProfileModelValidator GetValidator()
        {
            return new EditProfileModelValidator();
        }

        [TestClass]
        public class NamePropertyTests : EditProfileValidationTests
        {
            [TestMethod]
            public void ShouldHaveErrorWhenUsernameIsNull()
            {
                GetValidator().ShouldHaveValidationErrorFor(x => x.Name, (string)null);
            }

            [TestMethod]
            public void ShouldHaveErrorWhenUsernameIsEmpty()
            {
                GetValidator().ShouldHaveValidationErrorFor(x => x.Name, "");
            }

            [TestMethod]
            public void ShouldHaveErrorWhenUsernameIsWhitespace()
            {
                GetValidator().ShouldHaveValidationErrorFor(x => x.Name, "  ");
            }

            [TestMethod]
            public void ShouldHaveErrorWhenUsernameIsOver100Characters()
            {
                GetValidator().ShouldHaveValidationErrorFor(x => x.Name, new string('z', 101));
            }

            [TestMethod]
            public void ShouldNotHaveErrorWhenUsernameHasValue()
            {
                GetValidator().ShouldNotHaveValidationErrorFor(x => x.Name, "Name");
            }
        }

        [TestClass]
        public class ShortNamePropertyTests : EditProfileValidationTests
        {
            [TestMethod]
            public void ShouldHaveErrorWhenUsernameIsNull()
            {
                GetValidator().ShouldHaveValidationErrorFor(x => x.ShortName, (string)null);
            }

            [TestMethod]
            public void ShouldHaveErrorWhenUsernameIsEmpty()
            {
                GetValidator().ShouldHaveValidationErrorFor(x => x.ShortName, "");
            }

            [TestMethod]
            public void ShouldHaveErrorWhenUsernameIsWhitespace()
            {
                GetValidator().ShouldHaveValidationErrorFor(x => x.ShortName, "  ");
            }

            [TestMethod]
            public void ShouldHaveErrorWhenUsernameIsOver100Characters()
            {
                GetValidator().ShouldHaveValidationErrorFor(x => x.ShortName, new string('z', 101));
            }

            [TestMethod]
            public void ShouldNotHaveErrorWhenUsernameHasValue()
            {
                GetValidator().ShouldNotHaveValidationErrorFor(x => x.ShortName, "ShortName");
            }
        }

        [TestClass]
        public class EmailPropertyTests : EditProfileValidationTests
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
                GetValidator().ShouldNotHaveValidationErrorFor(x => x.Email, "email@email.com");
            }
        }
    }
}
