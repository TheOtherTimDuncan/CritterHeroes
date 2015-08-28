using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CH.Test.Mocks;
using CritterHeroes.Web.Areas.Admin.Critters.CommandHandlers;
using CritterHeroes.Web.Areas.Admin.Critters.Commands;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Data.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TOTD.Utility.Misc;

namespace CH.Test.AdminCrittersTests
{
    [TestClass]
    public class UploadFileCommandHandlerTests : BaseTest
    {
        public Breed MockSqlStorage
        {
            get;
            private set;
        }

        [TestMethod]
        public async Task CanAddOrUpdateCrittersFromFileData()
        {
            MockSqlStorageContext<Breed> breedStorage = new MockSqlStorageContext<Breed>();

            MockSqlStorageContext<CritterStatus> statusStorage = new MockSqlStorageContext<CritterStatus>();

            MockSqlStorageContext<Species> speciesStorage = new MockSqlStorageContext<Species>();

            MockSqlStorageContext<Critter> critterStorage = new MockSqlStorageContext<Critter>();

            string filename = Path.Combine(UnitTestHelper.GetSolutionRoot(), ".vs", "Sample Data", "FD5ObDXh_pets_1.json");
            using (FileStream stream = new FileStream(filename, FileMode.Open))
            {
                Mock<HttpPostedFileBase> mockFile = new Mock<HttpPostedFileBase>();
                mockFile.Setup(x => x.InputStream).Returns(stream);

                UploadFileCommand command = new UploadFileCommand()
                {
                    File = mockFile.Object
                };

                UploadFileCommandHandler handler = new UploadFileCommandHandler(critterStorage.Object, statusStorage.Object, breedStorage.Object, speciesStorage.Object);
                CommandResult commandResult = await handler.ExecuteAsync(command);
                commandResult.Succeeded.Should().BeTrue();

                breedStorage.Object.Entities.Should().NotBeNullOrEmpty();
                statusStorage.Object.Entities.Should().NotBeNullOrEmpty();
                speciesStorage.Object.Entities.Should().NotBeNullOrEmpty();
                critterStorage.Object.Entities.Should().NotBeNullOrEmpty();
            }
        }
    }
}
