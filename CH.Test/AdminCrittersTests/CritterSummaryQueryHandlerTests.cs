using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Test.Mocks;
using CritterHeroes.Web.Data.Models;
using CritterHeroes.Web.Features.Admin.Critters.Models;
using CritterHeroes.Web.Features.Admin.Critters.Queries;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TOTD.Utility.UnitTestHelpers;

namespace CH.Test.AdminCrittersTests
{
    [TestClass]
    public class CritterSummaryQueryHandlerTests
    {
        [TestMethod]
        public async Task ReturnsViewModel()
        {
            CritterStatus status1 = new CritterStatus("status1", "description1").SetEntityID(x => x.ID);
            CritterStatus status2 = new CritterStatus("status2", "description2").SetEntityID(x => x.ID);

            Species species = new Species("species", "species", "species");
            Breed breed = new Breed(species, "breed");

            Guid organizationID = Guid.NewGuid();

            List<Critter> critters = new List<Critter>();

            int status1Count = 3;
            for (int c = 1; c <= status1Count; c++)
            {
                critters.Add(new Critter($"critter1-{c}", status1, breed, organizationID));
            }


            int status2Count = 5;
            for (int c = 1; c <= status2Count; c++)
            {
                critters.Add(new Critter($"critter2-{c}", status2, breed, organizationID));
            }

            MockSqlStorageContext<Critter> mockCritterStorage = new MockSqlStorageContext<Critter>(critters);

            CritterSummaryQueryHandler handler = new CritterSummaryQueryHandler(mockCritterStorage.Object);
            CritterSummaryModel model = await handler.ExecuteAsync(new CritterSummaryQuery());
            model.Should().NotBeNull();

            model.StatusSummary.Should().HaveCount(2);

            StatusModel statusModel1 = model.StatusSummary.SingleOrDefault(x => x.StatusID == status1.ID);
            statusModel1.Should().NotBeNull();
            statusModel1.Status.Should().Be(status1.Name);
            statusModel1.Count.Should().Be(status1Count);

            StatusModel statusModel2 = model.StatusSummary.SingleOrDefault(x => x.StatusID == status2.ID);
            statusModel2.Should().NotBeNull();
            statusModel2.Status.Should().Be(status2.Name);
            statusModel2.Count.Should().Be(status2Count);
        }
    }
}
