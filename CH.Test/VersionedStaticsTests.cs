using System;
using System.Collections.Generic;
using System.Linq;
using CH.Test.Mocks;
using CritterHeroes.Web.Common.VersionedStatics;
using CritterHeroes.Web.Contracts;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CH.Test
{
    [TestClass]
    public class VersionedStaticsTests
    {
        [TestMethod]
        public void ReturnsUrlForDebugFileIfDebuging()
        {
            MockFileSystem mockFileSystem = new MockFileSystem();
            mockFileSystem.Setup(x => x.ReadAllText("/versioned-js.json")).Returns(LibManifest);
            mockFileSystem.Setup(x => x.ReadAllText("/versioned-lib.json")).Returns("{}");
            mockFileSystem.Setup(x => x.ReadAllText("/versioned-css.json")).Returns("{}");
            mockFileSystem.Setup(x => x.ReadAllText("/versioned-templates.json")).Returns("{}");
            mockFileSystem.Setup(x => x.MapServerPath(It.IsAny<string>())).Returns((string path) => path);

            Mock<IHttpContext> mockHttpContext = new Mock<IHttpContext>();
            mockHttpContext.Setup(x => x.IsDebuggingEnabled).Returns(true);
            mockHttpContext.Setup(x => x.ConvertToAbsoluteUrl(It.IsAny<string>())).Returns((string virtualPath) => virtualPath);

            MockUrlHelper mockUrlHelper = new MockUrlHelper(new MockHttpContext());

            VersionedStatics.Configure(mockFileSystem.Object, mockHttpContext.Object);
            VersionedStatics.IsDebug = true;
            mockUrlHelper.For("file1.js").Should().Be("~/dist/js/file1.js");
        }

        [TestMethod]
        public void ReturnsUrlForProductionFileIfNotDebuging()
        {
            MockFileSystem mockFileSystem = new MockFileSystem();
            mockFileSystem.Setup(x => x.ReadAllText("/versioned-js.json")).Returns(LibManifest);
            mockFileSystem.Setup(x => x.ReadAllText("/versioned-lib.json")).Returns("{}");
            mockFileSystem.Setup(x => x.ReadAllText("/versioned-css.json")).Returns("{}");
            mockFileSystem.Setup(x => x.ReadAllText("/versioned-templates.json")).Returns("{}");
            mockFileSystem.Setup(x => x.MapServerPath(It.IsAny<string>())).Returns((string path) => path);

            Mock<IHttpContext> mockHttpContext = new Mock<IHttpContext>();
            mockHttpContext.Setup(x => x.IsDebuggingEnabled).Returns(true);
            mockHttpContext.Setup(x => x.ConvertToAbsoluteUrl(It.IsAny<string>())).Returns((string virtualPath) => virtualPath);

            MockUrlHelper mockUrlHelper = new MockUrlHelper(new MockHttpContext());

            VersionedStatics.Configure(mockFileSystem.Object, mockHttpContext.Object);
            VersionedStatics.IsDebug = false;
            mockUrlHelper.For("file1.js").Should().Be("~/dist/js/file1-12345.min.js");
        }

        public const string LibManifest = "{\"js/file1.min.js\": \"js/file1-12345.min.js\"}";
    }
}
