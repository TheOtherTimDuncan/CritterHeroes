using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.DataProviders.Azure.Services;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage.Blob;
using Moq;

namespace CH.Test.Azure.StorageTests
{
    [TestClass]
    public class CritterPictureServiceTests
    {
        [TestMethod]
        public async Task CanSaveImage()
        {
            int critterID = 99;
            string filename = "3671153_448x336.jpg";
            string contentType = "image/jpeg";
            bool isPrivate = false;
            MemoryStream memStream = new MemoryStream();

            Mock<IAzureService> mockAzureService = new Mock<IAzureService>();
            mockAzureService.Setup(x => x.UploadBlobAsync(It.IsAny<string>(), isPrivate, contentType, memStream)).Returns((string path, bool callbackIsPrivate, string callbackContentType, Stream stream) =>
            {
                path.Should().EndWith($"{critterID}/{filename}");

                CloudBlockBlob blob = new CloudBlockBlob(new Uri("http://localhost/container"));
                return Task.FromResult(blob);
            });

            CritterPictureService service = new CritterPictureService(mockAzureService.Object);
            await service.SavePictureAsync(memStream, critterID, filename, contentType);

            mockAzureService.Verify(x => x.UploadBlobAsync(It.IsAny<string>(), isPrivate, contentType, memStream), Times.Once);
        }
    }
}
