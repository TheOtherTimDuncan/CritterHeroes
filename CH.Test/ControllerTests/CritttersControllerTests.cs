using System;
using System.Collections.Generic;
using System.Linq;
using CH.Test.ControllerTests.TestHelpers;
using CritterHeroes.Web.Features.Admin.Critters;
using CritterHeroes.Web.Features.Admin.Critters.Models;
using CritterHeroes.Web.Features.Admin.Critters.Queries;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CH.Test.ControllerTests
{
    [TestClass]
    public class CritttersControllerTests : BaseTest
    {
        [TestMethod]
        public void IndexGetReturnsViewWithModel()
        {
            CrittersModel model = new CrittersModel();
            CrittersQuery query = new CrittersQuery();

            ControllerTester.UsingController<CrittersController>()
                .SetupQueryDispatcherAsync(model, query)
                .WithCallTo(x => x.Index(query))
                .VerifyQueryDispatcher()
                .ShouldReturnViewResult()
                .HavingModel(model);
        }

        [TestMethod]
        public void ListGetReturnsJson()
        {
            CrittersListQuery query = new CrittersListQuery();
            CrittersListModel model = new CrittersListModel();

            ControllerTester.UsingController<CrittersController>()
                .SetupQueryDispatcherAsync(model, query)
                .WithCallTo(x => x.List(query))
                .VerifyQueryDispatcher()
                .ShouldReturnJsonCamelCase()
                .HavingModel(model);
        }

        [TestMethod]
        public void SummaryGetReturnsViewWithModel()
        {
            CritterSummaryModel model = new CritterSummaryModel();

            ControllerTester.UsingController<CrittersController>()
                .SetupQueryDispatcherAsync(model)
                .WithCallTo(x => x.Summary())
                .VerifyQueryDispatcher()
                .ShouldReturnPartialViewResult()
                .HavingModel(model);
        }
    }
}
