using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Domain.Contracts.Configuration;
using CH.Domain.StateManagement;
using CH.Website.Models;
using CH.Website.Services.QueryHandlers;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CH.Test.QueryTests
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
                AzureName="azure",
                LogoFilename="logo.svg"
            };

            HeaderViewModelQueryHandler handler = new HeaderViewModelQueryHandler(mockAppConfiguration.Object);
            HeaderModel model = handler.Retrieve(orgContext).Result;  // Should be synchronous

            model.Should().NotBeNull();
            model.LogoUrl.Should().Be("http://root/azure/logo.svg");

            mockAppConfiguration.Verify(x => x.BlobBaseUrl, Times.Once);
        }
    }
}
