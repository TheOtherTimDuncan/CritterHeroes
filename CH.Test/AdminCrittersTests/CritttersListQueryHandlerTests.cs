using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Test.Mocks;
using CritterHeroes.Web.Data.Models;
using CritterHeroes.Web.Features.Admin.Critters.Models;
using CritterHeroes.Web.Features.Admin.Critters.Queries;
using CritterHeroes.Web.Features.Shared.ActionExtensions;
using CritterHeroes.Web.Shared;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TOTD.Utility.UnitTestHelpers;

namespace CH.Test.AdminCrittersTests
{
    [TestClass]
    public class CritttersListQueryHandlerTests
    {
        [TestMethod]
        public async Task ReturnsViewModel()
        {
            CritterStatus status = new CritterStatus("status", "description").SetEntityID(x => x.ID);
            Species species = new Species("species", "species", "species");
            Breed breed = new Breed(species, "breed");

            Critter critter = new Critter("critter", status, breed, Guid.NewGuid(), rescueGroupsID: 99)
            {
                Sex = "M"
            }.SetEntityID(x => x.ID);

            critter.ChangeFoster(new Person()
            {
                FirstName = "First",
                LastName = "Last"
            });

            CritterPicture critterPicture = critter.AddPicture(new Picture("picture.jpg", 0, 0, 0, "contenttype")
            {
                DisplayOrder = 1
            });

            CrittersListQuery query = new CrittersListQuery();

            MockSqlQueryStorageContext<Critter> mockCritterStorage = new MockSqlQueryStorageContext<Critter>(critter);

            MockUrlGenerator mockUrlGenerator = new MockUrlGenerator();

            CrittersListQueryHandler handler = new CrittersListQueryHandler(mockCritterStorage.Object, mockUrlGenerator.Object);
            CrittersListModel model = await handler.ExecuteAsync(query);
            model.Should().NotBeNull();

            model.Paging.Should().NotBeNull();

            model.Critters.Should().HaveCount(1);
            CritterModel critterModel = model.Critters.Single();

            critterModel.ID.Should().Be(critter.ID);
            critterModel.Name.Should().Be(critter.Name);
            critterModel.Status.Should().Be(critter.Status.Name);
            critterModel.Breed.Should().Be(critter.Breed.BreedName);
            critterModel.Sex.Should().Be(critter.Sex);
            critterModel.SexName.Should().Be(SexHelper.GetSexName(critter.Sex));
            critterModel.SiteID.Should().Be(critter.RescueGroupsID.ToString());
            critterModel.FosterName.Should().Be(critter.Foster.FirstName + " " + critter.Foster.LastName);
            critterModel.PictureFilename.Should().Be(critterPicture.Picture.Filename);
            critterModel.EditUrl.Should().Be(mockUrlGenerator.Object.GenerateAdminCrittersEditAction(critterModel.ID));
        }

        [TestMethod]
        public async Task ReturnsViewModelForSpecifiedStatus()
        {
            CritterStatus status1 = new CritterStatus("status", "description").SetEntityID(x => x.ID);
            CritterStatus status2 = new CritterStatus("status", "description").SetEntityID(x => x.ID);

            Species species = new Species("species", "species", "species");
            Breed breed = new Breed(species, "breed");

            Critter critter1 = new Critter("critter1", status1, breed, Guid.NewGuid()).SetEntityID(x => x.ID);
            Critter critter21 = new Critter("critter2", status2, breed, Guid.NewGuid()).SetEntityID(x => x.ID);

            CritterPicture critterPicture = critter1.AddPicture(new Picture("picture.jpg", 0, 0, 0, "contenttype")
            {
                DisplayOrder = 1
            });

            critter1.ChangeFoster(new Person()
            {
                FirstName = "First",
                LastName = "Last"
            });

            CrittersListQuery query = new CrittersListQuery()
            {
                StatusID = status1.ID
            };

            MockSqlQueryStorageContext<Critter> mockCritterStorage = new MockSqlQueryStorageContext<Critter>(critter1);

            MockUrlGenerator mockUrlGenerator = new MockUrlGenerator();

            CrittersListQueryHandler handler = new CrittersListQueryHandler(mockCritterStorage.Object, mockUrlGenerator.Object);
            CrittersListModel model = await handler.ExecuteAsync(query);
            model.Should().NotBeNull();

            model.Paging.Should().NotBeNull();

            model.Critters.Should().HaveCount(1);
            CritterModel critterModel = model.Critters.Single();

            critterModel.ID.Should().Be(critter1.ID);
        }
    }
}
