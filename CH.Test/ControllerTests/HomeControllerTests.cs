using System;
using System.Collections.Generic;
using System.Linq;
using CH.Test.ControllerTests.TestHelpers;
using CritterHeroes.Web.Areas.Home;
using CritterHeroes.Web.Areas.Home.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CH.Test.ControllerTests
{
    [TestClass]
    public class HomeControllerTests : BaseTest
    {
        [TestMethod]
        public void MenuGetReturnsPartialViewWithModel()
        {
            MenuModel model = new MenuModel();

            ControllerTester.UsingController<HomeController>()
                .SetupQueryDispatcher(model)
                .WithCallTo(x => x.Menu())
                .VerifyQueryDispatcher()
                .ShouldReturnPartialViewResult()
                .HavingModel(model);
        }

        [TestMethod]
        public void HeaderGetReturnsPartialViewWithModel()
        {
            HeaderModel model = new HeaderModel();

            ControllerTester.UsingController<HomeController>()
                .SetupQueryDispatcher(model)
                .WithCallTo(x => x.Header())
                .VerifyQueryDispatcher()
                .ShouldReturnPartialViewResult()
                .HavingModel(model);
        }
    }
}
