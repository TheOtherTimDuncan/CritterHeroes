using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CH.Test.Mocks;
using CritterHeroes.Web.Data.Models;
using CritterHeroes.Web.Domain.Contracts.Storage;
using CritterHeroes.Web.Middleware;
using FluentAssertions;
using Microsoft.Owin;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TOTD.Utility.UnitTestHelpers;

namespace CH.Test.MiddlewareTests
{
    [TestClass]
    public class ImageMiddlewareTests
    {
        [TestMethod]
        public async Task ReturnsNotFoundImageIfCritterIDNotInCorrectSegment()
        {
            string notFound = "notfound";

            Mock<ICritterPictureService> mockPictureService = new Mock<ICritterPictureService>();
            mockPictureService.Setup(x => x.GetNotFoundUrl()).Returns(notFound);

            // Only the setup methods for mockResolver should be called
            Mock<IDependencyResolver> mockResolver = new Mock<IDependencyResolver>(MockBehavior.Strict);
            mockResolver.Setup(x => x.GetService(typeof(ICritterPictureService))).Returns(mockPictureService.Object);

            Mock<IOwinContext> mockOwinContext = new Mock<IOwinContext>();
            mockOwinContext.Setup(x => x.Request.Path).Returns(new PathString($"{ImageMiddleware.Route}/zz/test.jpg"));
            mockOwinContext.Setup(x => x.Request.Uri).Returns(new Uri($"http://localhost{ImageMiddleware.Route}"));

            Mock<IOwinResponse> mockResponse = new Mock<IOwinResponse>();
            mockOwinContext.Setup(x => x.Response).Returns(mockResponse.Object);

            TestEndMiddleware testMiddleware = new TestEndMiddleware();
            testMiddleware.isInvoked.Should().BeFalse();

            ImageMiddleware middleware = new ImageMiddleware(testMiddleware, mockResolver.Object);
            await middleware.Invoke(mockOwinContext.Object);

            mockResponse.Verify(x => x.Redirect(notFound), Times.Once);

            testMiddleware.isInvoked.Should().BeFalse();
        }

        [TestMethod]
        public async Task ReturnsNotFoundImageIfCritterIDDoesNotMatchExistingCritter()
        {
            string notFound = "notfound";

            Dictionary<string, string[]> queryValues = new Dictionary<string, string[]>();
            queryValues["width"] = new string[] { "100" };

            ReadableStringCollection requestQuery = new ReadableStringCollection(queryValues);

            MockSqlCommandStorageContext<Critter> mockStorageContext = new MockSqlCommandStorageContext<Critter>();

            Mock<ICritterPictureService> mockPictureService = new Mock<ICritterPictureService>();
            mockPictureService.Setup(x => x.GetNotFoundUrl()).Returns(notFound);

            // Only the setup methods for mockResolver should be called
            Mock<IDependencyResolver> mockResolver = new Mock<IDependencyResolver>(MockBehavior.Strict);
            mockResolver.Setup(x => x.GetService(typeof(ISqlCommandStorageContext<Critter>))).Returns(mockStorageContext.Object);
            mockResolver.Setup(x => x.GetService(typeof(ICritterPictureService))).Returns(mockPictureService.Object);

            Mock<IOwinRequest> mockRequest = new Mock<IOwinRequest>();
            mockRequest.Setup(x => x.Query).Returns(requestQuery);
            mockRequest.Setup(x => x.Path).Returns(new PathString($"{ImageMiddleware.Route}/1/test.jpg"));
            mockRequest.Setup(x => x.Uri).Returns(new Uri($"http://localhost{ImageMiddleware.Route}/1/test.jpg"));

            Mock<IOwinResponse> mockResponse = new Mock<IOwinResponse>();

            Mock<IOwinContext> mockOwinContext = new Mock<IOwinContext>();
            mockOwinContext.Setup(x => x.Request).Returns(mockRequest.Object);
            mockOwinContext.Setup(x => x.Response).Returns(mockResponse.Object);

            TestEndMiddleware testMiddleware = new TestEndMiddleware();
            testMiddleware.isInvoked.Should().BeFalse();

            ImageMiddleware middleware = new ImageMiddleware(testMiddleware, mockResolver.Object);
            await middleware.Invoke(mockOwinContext.Object);

            mockResponse.Verify(x => x.Redirect(notFound), Times.Once);

            testMiddleware.isInvoked.Should().BeFalse();
        }

        [TestMethod]
        public async Task RedirectsToOriginalPictureUrlIfNoWidthOrHeightRequested()
        {
            int critterID = 1;
            string filename = "test.jpg";
            string url = "redirect";

            MockSqlCommandStorageContext<Critter> mockStorageContext = new MockSqlCommandStorageContext<Critter>();

            Mock<ICritterPictureService> mockPictureService = new Mock<ICritterPictureService>();
            mockPictureService.Setup(x => x.GetPictureUrl(critterID, filename)).Returns(url);

            // Only the setup methods for mockResolver should be called
            Mock<IDependencyResolver> mockResolver = new Mock<IDependencyResolver>(MockBehavior.Strict);
            mockResolver.Setup(x => x.GetService(typeof(ISqlCommandStorageContext<Critter>))).Returns(mockStorageContext.Object);
            mockResolver.Setup(x => x.GetService(typeof(ICritterPictureService))).Returns(mockPictureService.Object);

            Mock<IOwinRequest> mockRequest = new Mock<IOwinRequest>();
            mockRequest.Setup(x => x.Path).Returns(new PathString($"{ImageMiddleware.Route}/{critterID}/{filename}"));
            mockRequest.Setup(x => x.Uri).Returns(new Uri($"http://localhost{ImageMiddleware.Route}/{critterID}/{filename}"));

            Mock<IOwinResponse> mockResponse = new Mock<IOwinResponse>();

            Mock<IOwinContext> mockOwinContext = new Mock<IOwinContext>();
            mockOwinContext.Setup(x => x.Request).Returns(mockRequest.Object);
            mockOwinContext.Setup(x => x.Response).Returns(mockResponse.Object);

            TestEndMiddleware testMiddleware = new TestEndMiddleware();
            testMiddleware.isInvoked.Should().BeFalse();

            ImageMiddleware middleware = new ImageMiddleware(testMiddleware, mockResolver.Object);
            await middleware.Invoke(mockOwinContext.Object);

            mockResponse.Verify(x => x.Redirect(url), Times.Once);

            testMiddleware.isInvoked.Should().BeFalse();
        }

        [TestMethod]
        public async Task ReturnsNotFoundImageIfCritterDoesNotHavePictureWithMatchingFilename()
        {
            string filename = "test.jpg";
            string notFound = "notfound";

            Critter critter = new Critter("critter", new CritterStatus("status", "status"), new Breed(new Species("species", "singular", "plural"), "breed"), Guid.NewGuid(), rescueGroupsID: 99).SetEntityID(x => x.ID);

            Dictionary<string, string[]> queryValues = new Dictionary<string, string[]>();
            queryValues["width"] = new string[] { "100" };

            ReadableStringCollection requestQuery = new ReadableStringCollection(queryValues);

            MockSqlCommandStorageContext<Critter> mockStorageContext = new MockSqlCommandStorageContext<Critter>(critter);

            Mock<ICritterPictureService> mockPictureService = new Mock<ICritterPictureService>();
            mockPictureService.Setup(x => x.GetNotFoundUrl()).Returns(notFound);

            // Only the setup methods for mockResolver should be called
            Mock<IDependencyResolver> mockResolver = new Mock<IDependencyResolver>(MockBehavior.Strict);
            mockResolver.Setup(x => x.GetService(typeof(ISqlCommandStorageContext<Critter>))).Returns(mockStorageContext.Object);
            mockResolver.Setup(x => x.GetService(typeof(ICritterPictureService))).Returns(mockPictureService.Object);

            Mock<IOwinRequest> mockRequest = new Mock<IOwinRequest>();
            mockRequest.Setup(x => x.Query).Returns(requestQuery);
            mockRequest.Setup(x => x.Path).Returns(new PathString($"{ImageMiddleware.Route}/{critter.ID}/{filename}"));
            mockRequest.Setup(x => x.Uri).Returns(new Uri($"http://localhost{ImageMiddleware.Route}/{critter.ID}/{filename}"));

            Mock<IOwinResponse> mockResponse = new Mock<IOwinResponse>();

            Mock<IOwinContext> mockOwinContext = new Mock<IOwinContext>();
            mockOwinContext.Setup(x => x.Request).Returns(mockRequest.Object);
            mockOwinContext.Setup(x => x.Response).Returns(mockResponse.Object);

            TestEndMiddleware testMiddleware = new TestEndMiddleware();
            testMiddleware.isInvoked.Should().BeFalse();

            ImageMiddleware middleware = new ImageMiddleware(testMiddleware, mockResolver.Object);
            await middleware.Invoke(mockOwinContext.Object);

            mockResponse.Verify(x => x.Redirect(notFound), Times.Once);

            testMiddleware.isInvoked.Should().BeFalse();
        }

        [TestMethod]
        public async Task RedirectsToOriginalPictureUrlIfWidthIsInvalid()
        {
            string filename = "test.jpg";
            string url = "redirect";

            Critter critter = new Critter("critter", new CritterStatus("status", "status"), new Breed(new Species("species", "singular", "plural"), "breed"), Guid.NewGuid(), rescueGroupsID: 99).SetEntityID(x => x.ID);
            CritterPicture critterPicture = critter.AddPicture(new Picture(filename, 100, 100, 500, "jpeg"));

            Dictionary<string, string[]> queryValues = new Dictionary<string, string[]>();
            queryValues["width"] = new string[] { "foobar" };

            ReadableStringCollection requestQuery = new ReadableStringCollection(queryValues);

            MockSqlCommandStorageContext<Critter> mockStorageContext = new MockSqlCommandStorageContext<Critter>(critter);

            Mock<ICritterPictureService> mockPictureService = new Mock<ICritterPictureService>();
            mockPictureService.Setup(x => x.GetPictureUrl(critter.ID, filename)).Returns(url);

            // Only the setup methods for mockResolver should be called
            Mock<IDependencyResolver> mockResolver = new Mock<IDependencyResolver>(MockBehavior.Strict);
            mockResolver.Setup(x => x.GetService(typeof(ISqlCommandStorageContext<Critter>))).Returns(mockStorageContext.Object);
            mockResolver.Setup(x => x.GetService(typeof(ICritterPictureService))).Returns(mockPictureService.Object);

            Mock<IOwinRequest> mockRequest = new Mock<IOwinRequest>();
            mockRequest.Setup(x => x.Query).Returns(requestQuery);
            mockRequest.Setup(x => x.Path).Returns(new PathString($"{ImageMiddleware.Route}/{critter.ID}/{filename}"));
            mockRequest.Setup(x => x.Uri).Returns(new Uri($"http://localhost{ImageMiddleware.Route}/{critter.ID}/{filename}"));

            Mock<IOwinResponse> mockResponse = new Mock<IOwinResponse>();

            Mock<IOwinContext> mockOwinContext = new Mock<IOwinContext>();
            mockOwinContext.Setup(x => x.Request).Returns(mockRequest.Object);
            mockOwinContext.Setup(x => x.Response).Returns(mockResponse.Object);

            TestEndMiddleware testMiddleware = new TestEndMiddleware();
            testMiddleware.isInvoked.Should().BeFalse();

            ImageMiddleware middleware = new ImageMiddleware(testMiddleware, mockResolver.Object);
            await middleware.Invoke(mockOwinContext.Object);

            mockResponse.Verify(x => x.Redirect(url), Times.Once);

            testMiddleware.isInvoked.Should().BeFalse();
        }

        [TestMethod]
        public async Task RedirectsToOriginalPictureUrlIfHeightIsInvalid()
        {
            string filename = "test.jpg";
            string url = "redirect";

            Critter critter = new Critter("critter", new CritterStatus("status", "status"), new Breed(new Species("species", "singular", "plural", null, null), "breed"), Guid.NewGuid(), rescueGroupsID: 99).SetEntityID(x => x.ID);
            CritterPicture critterPicture = critter.AddPicture(new Picture(filename, 100, 100, 500, "jpeg"));

            Dictionary<string, string[]> queryValues = new Dictionary<string, string[]>();
            queryValues["height"] = new string[] { "foobar" };

            ReadableStringCollection requestQuery = new ReadableStringCollection(queryValues);

            MockSqlCommandStorageContext<Critter> mockStorageContext = new MockSqlCommandStorageContext<Critter>(critter);

            Mock<ICritterPictureService> mockPictureService = new Mock<ICritterPictureService>();
            mockPictureService.Setup(x => x.GetPictureUrl(critter.ID, filename)).Returns(url);

            // Only the setup methods for mockResolver should be called
            Mock<IDependencyResolver> mockResolver = new Mock<IDependencyResolver>(MockBehavior.Strict);
            mockResolver.Setup(x => x.GetService(typeof(ISqlCommandStorageContext<Critter>))).Returns(mockStorageContext.Object);
            mockResolver.Setup(x => x.GetService(typeof(ICritterPictureService))).Returns(mockPictureService.Object);

            Mock<IOwinRequest> mockRequest = new Mock<IOwinRequest>();
            mockRequest.Setup(x => x.Query).Returns(requestQuery);
            mockRequest.Setup(x => x.Path).Returns(new PathString($"{ImageMiddleware.Route}/{critter.ID}/{filename}"));
            mockRequest.Setup(x => x.Uri).Returns(new Uri($"http://localhost{ImageMiddleware.Route}/{critter.ID}/{filename}"));

            Mock<IOwinResponse> mockResponse = new Mock<IOwinResponse>();

            Mock<IOwinContext> mockOwinContext = new Mock<IOwinContext>();
            mockOwinContext.Setup(x => x.Request).Returns(mockRequest.Object);
            mockOwinContext.Setup(x => x.Response).Returns(mockResponse.Object);

            TestEndMiddleware testMiddleware = new TestEndMiddleware();
            testMiddleware.isInvoked.Should().BeFalse();

            ImageMiddleware middleware = new ImageMiddleware(testMiddleware, mockResolver.Object);
            await middleware.Invoke(mockOwinContext.Object);

            mockResponse.Verify(x => x.Redirect(url), Times.Once);

            testMiddleware.isInvoked.Should().BeFalse();
        }

        [TestMethod]
        public async Task RedirectsToPictureUrlWithRequestedWidthIfAlreadyExists()
        {
            int width = 200;
            int height = 500;
            int requestWidth = width / 2;
            int requestHeight = height / 2;

            Critter critter = new Critter("critter", new CritterStatus("status", "status"), new Breed(new Species("species", "singular", "plural", null, null), "breed"), Guid.NewGuid(), rescueGroupsID: 99).SetEntityID(x => x.ID);
            CritterPicture critterPicture = critter.AddPicture(new Picture("test.jpg", width, height, 500, "jpeg"));
            PictureChild childPicture = critterPicture.Picture.AddChildPicture(requestWidth, requestHeight, 100);

            Dictionary<string, string[]> queryValues = new Dictionary<string, string[]>();
            queryValues["width"] = new string[] { $"{requestWidth}" };

            ReadableStringCollection requestQuery = new ReadableStringCollection(queryValues);

            MockSqlCommandStorageContext<Critter> mockStorageContext = new MockSqlCommandStorageContext<Critter>(critter);

            Mock<ICritterPictureService> mockPictureService = new Mock<ICritterPictureService>();
            mockPictureService.Setup(x => x.GetPictureUrl(critter.ID, childPicture.Filename)).Returns(childPicture.Filename);

            // Only the setup methods for mockResolver should be called
            Mock<IDependencyResolver> mockResolver = new Mock<IDependencyResolver>(MockBehavior.Strict);
            mockResolver.Setup(x => x.GetService(typeof(ISqlCommandStorageContext<Critter>))).Returns(mockStorageContext.Object);
            mockResolver.Setup(x => x.GetService(typeof(ICritterPictureService))).Returns(mockPictureService.Object);

            Mock<IOwinRequest> mockRequest = new Mock<IOwinRequest>();
            mockRequest.Setup(x => x.Query).Returns(requestQuery);
            mockRequest.Setup(x => x.Path).Returns(new PathString($"{ImageMiddleware.Route}/{critter.ID}/{critterPicture.Picture.Filename}"));
            mockRequest.Setup(x => x.Uri).Returns(new Uri($"http://localhost{ImageMiddleware.Route}/{critter.ID}/{critterPicture.Picture.Filename}"));

            Mock<IOwinResponse> mockResponse = new Mock<IOwinResponse>();

            Mock<IOwinContext> mockOwinContext = new Mock<IOwinContext>();
            mockOwinContext.Setup(x => x.Request).Returns(mockRequest.Object);
            mockOwinContext.Setup(x => x.Response).Returns(mockResponse.Object);

            TestEndMiddleware testMiddleware = new TestEndMiddleware();
            testMiddleware.isInvoked.Should().BeFalse();

            ImageMiddleware middleware = new ImageMiddleware(testMiddleware, mockResolver.Object);
            await middleware.Invoke(mockOwinContext.Object);

            mockResponse.Verify(x => x.Redirect(childPicture.Filename), Times.Once);

            testMiddleware.isInvoked.Should().BeFalse();
        }

        [TestMethod]
        public async Task RedirectsToPictureUrlWithRequestedHeightIfAlreadyExists()
        {
            int width = 200;
            int height = 500;
            int requestWidth = width / 2;
            int requestHeight = height / 2;

            Critter critter = new Critter("critter", new CritterStatus("status", "status"), new Breed(new Species("species", "singular", "plural", null, null), "breed"), Guid.NewGuid(), rescueGroupsID: 99).SetEntityID(x => x.ID);
            CritterPicture critterPicture = critter.AddPicture(new Picture("test.jpg", width, height, 500, "jpeg"));
            PictureChild childPicture = critterPicture.Picture.AddChildPicture(requestWidth, requestHeight, 100);

            Dictionary<string, string[]> queryValues = new Dictionary<string, string[]>();
            queryValues["height"] = new string[] { $"{requestHeight}" };

            ReadableStringCollection requestQuery = new ReadableStringCollection(queryValues);

            MockSqlCommandStorageContext<Critter> mockStorageContext = new MockSqlCommandStorageContext<Critter>(critter);

            Mock<ICritterPictureService> mockPictureService = new Mock<ICritterPictureService>();
            mockPictureService.Setup(x => x.GetPictureUrl(critter.ID, childPicture.Filename)).Returns(childPicture.Filename);

            // Only the setup methods for mockResolver should be called
            Mock<IDependencyResolver> mockResolver = new Mock<IDependencyResolver>(MockBehavior.Strict);
            mockResolver.Setup(x => x.GetService(typeof(ISqlCommandStorageContext<Critter>))).Returns(mockStorageContext.Object);
            mockResolver.Setup(x => x.GetService(typeof(ICritterPictureService))).Returns(mockPictureService.Object);

            Mock<IOwinRequest> mockRequest = new Mock<IOwinRequest>();
            mockRequest.Setup(x => x.Query).Returns(requestQuery);
            mockRequest.Setup(x => x.Path).Returns(new PathString($"{ImageMiddleware.Route}/{critter.ID}/{critterPicture.Picture.Filename}"));
            mockRequest.Setup(x => x.Uri).Returns(new Uri($"http://localhost{ImageMiddleware.Route}/{critter.ID}/{critterPicture.Picture.Filename}"));

            Mock<IOwinResponse> mockResponse = new Mock<IOwinResponse>();

            Mock<IOwinContext> mockOwinContext = new Mock<IOwinContext>();
            mockOwinContext.Setup(x => x.Request).Returns(mockRequest.Object);
            mockOwinContext.Setup(x => x.Response).Returns(mockResponse.Object);

            TestEndMiddleware testMiddleware = new TestEndMiddleware();
            testMiddleware.isInvoked.Should().BeFalse();

            ImageMiddleware middleware = new ImageMiddleware(testMiddleware, mockResolver.Object);
            await middleware.Invoke(mockOwinContext.Object);

            mockResponse.Verify(x => x.Redirect(childPicture.Filename), Times.Once);

            testMiddleware.isInvoked.Should().BeFalse();
        }
    }
}
