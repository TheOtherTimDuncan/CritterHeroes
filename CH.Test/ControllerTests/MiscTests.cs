using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Features.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TOTD.Mvc;

namespace CH.Test.ControllerTests
{
    [TestClass]
    public class MiscTests : BaseTest
    {
        [TestMethod]
        public void AllHttpPostControllerActionsShouldHaveValidateAntiForgeryTokenAttribute()
        {
            AssertMethodsListIsNullOrEmpty(UnitTestHelper.GetControllerActionsMissingValidateAntiForgeryTokenAttribute<BaseController>(), "all post actions need anti-forgery token");
        }
    }
}
