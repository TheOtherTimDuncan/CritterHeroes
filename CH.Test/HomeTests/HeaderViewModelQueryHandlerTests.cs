using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Features.Home.Models;
using CritterHeroes.Web.Features.Home.Queries;
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
            Mock<IOrganizationLogoService> mockLogoService = new Mock<IOrganizationLogoService>();
            mockLogoService.Setup(x => x.GetLogoUrl()).Returns("http://root/azure/logo.svg");

            HeaderQueryHandler handler = new HeaderQueryHandler(mockLogoService.Object);
            HeaderModel model = handler.Execute(new HeaderQuery()); 

            model.Should().NotBeNull();
            model.LogoUrl.Should().Be("http://root/azure/logo.svg");

            mockLogoService.Verify(x => x.GetLogoUrl(), Times.Once);
        }
    }
}
