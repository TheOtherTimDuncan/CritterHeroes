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
        public void AddsDebugJavascriptTagsForLibFilesIfDebugging()
        {
            MockFileSystem mockFileSystem = new MockFileSystem();
            mockFileSystem.Setup(x => x.ReadAllText(It.IsAny<string>())).Returns(LibManifest);
            mockFileSystem.Setup(x => x.MapServerPath(It.IsAny<string>())).Returns((string path) => path);

            Mock<IHttpContext> mockHttpContext = new Mock<IHttpContext>();
            mockHttpContext.Setup(x => x.IsDebuggingEnabled).Returns(true);
            mockHttpContext.Setup(x => x.ConvertToAbsoluteUrl(It.IsAny<string>())).Returns((string virtualPath) => virtualPath);

            VersionedStatics.Configure(mockFileSystem.Object, mockHttpContext.Object);
            string url = VersionedStatics.UrlFor("file1.js");
            url.Should().StartWith("~/src");
        }

        public const string LibManifest = "{\"file1.js\": \"file1-12345.js\", \"file2.js\": \"file2-67890..js\"}";
    }
}
