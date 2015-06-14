using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Areas.Home.Models;
using CritterHeroes.Web.Areas.Home.Queries;
using CritterHeroes.Web.Areas.Home.QueryHandlers;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.Configuration;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CH.Test.HomeTests
{
    [TestClass]
    public class HeaderViewModelQueryHandlerTests
    {
        [TestMethod]
        public void ReturnsViewModel()
        {
            Mock<IAppConfiguration> mockAppConfiguration = new Mock<IAppConfiguration>();
            mockAppConfiguration.Setup(x => x.BlobBaseUrl).Returns("http://root");

            OrganizationContext orgContext = new OrganizationContext()
            {
                AzureName = "azure",
                LogoFilename = "logo.svg"
            };

            HeaderViewModelQueryHandler handler = new HeaderViewModelQueryHandler(mockAppConfiguration.Object);
            HeaderModel model = handler.RetrieveAsync(new HeaderQuery(orgContext)).Result;  // Should be synchronous

            model.Should().NotBeNull();
            model.LogoUrl.Should().Be("http://root/azure/logo.svg");

            mockAppConfiguration.Verify(x => x.BlobBaseUrl, Times.Once);
        }
    }
}
