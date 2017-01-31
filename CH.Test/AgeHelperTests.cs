using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Shared;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CH.Test
{
    [TestClass]
    public class AgeHelperTests
    {
        [TestMethod]
        public void GetAgeReturnsCorrectAgeWhenBirthDayBeforeCurrentDay()
        {
            DateTime birthdate = DateTime.Now.AddDays(-1).AddYears(-2);
            AgeHelper.GetAge(birthdate).Should().Be("3 years old");
        }

        [TestMethod]
        public void GetAgeReturnsCorrectAgeWhenBirthDayAfterurrentDay()
        {
            DateTime birthdate = DateTime.Now.AddDays(1).AddYears(-1);
            AgeHelper.GetAge(birthdate).Should().Be("1 year old");
        }
    }
}
