using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CritterHeroes.Web.Areas.Admin.Critters.CommandHandlers;
using CritterHeroes.Web.Areas.Admin.Critters.Commands;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Models;
using EntityFramework.Testing.Moq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TOTD.Utility.Misc;

namespace CH.Test.AdminCrittersTests
{
    [TestClass]
    public class UploadXmlFileCommandHandlerTests
    {
        [TestMethod]
        public async Task CanAddOrUpdateCrittersFromFileData()
        {
            OrganizationContext orgContext = new OrganizationContext()
            {
                OrganizationID = Guid.NewGuid()
            };

            MockDbSet<Breed> mockBreedSet = new MockDbSet<Breed>().SetupLinq().SetupAddAndRemove();
            MockDbSet<CritterStatus> mockStatusSet = new MockDbSet<CritterStatus>().SetupLinq().SetupAddAndRemove();
            MockDbSet<Species> mockSpeciesSet = new MockDbSet<Species>().SetupLinq().SetupAddAndRemove();
            MockDbSet<Critter> mockCritterSet = new MockDbSet<Critter>().SetupLinq().SetupAddAndRemove();

            Mock<ICritterBatchSqlStorageContext> mockCritterStorage = new Mock<ICritterBatchSqlStorageContext>();
            mockCritterStorage.Setup(x => x.Breeds).Returns(mockBreedSet.Object);
            mockCritterStorage.Setup(x => x.CritterStatus).Returns(mockStatusSet.Object);
            mockCritterStorage.Setup(x => x.Species).Returns(mockSpeciesSet.Object);
            mockCritterStorage.Setup(x => x.Critters).Returns(mockCritterSet.Object);
            mockCritterStorage.Setup(x => x.AddCritter(It.IsAny<Critter>())).Callback((Critter critter) => mockCritterSet.Object.Add(critter));

            Mock<IStateManager<OrganizationContext>> mockStateManager = new Mock<IStateManager<OrganizationContext>>();
            mockStateManager.Setup(x => x.GetContext()).Returns(orgContext);

            string filename = Path.Combine(UnitTestHelper.GetSolutionRoot(), ".vs", "Sample Data", "animals.xml");
            using (FileStream stream = new FileStream(filename, FileMode.Open))
            {
                Mock<HttpPostedFileBase> mockFile = new Mock<HttpPostedFileBase>();
                mockFile.Setup(x => x.InputStream).Returns(stream);

                UploadXmlFileCommand command = new UploadXmlFileCommand()
                {
                    File = mockFile.Object
                };

                UploadXmlFileCommandHandler handler = new UploadXmlFileCommandHandler(mockCritterStorage.Object, mockStateManager.Object);
                CommandResult commandResult = await handler.ExecuteAsync(command);
                commandResult.Succeeded.Should().BeTrue();

                mockCritterSet.Object.ToList().Should().NotBeNullOrEmpty();
            }

            mockCritterStorage.Verify(x => x.SaveChangesAsync(), Times.AtLeastOnce());
        }
    }
}
