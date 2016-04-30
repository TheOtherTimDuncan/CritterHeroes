using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CH.Test.Mocks;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Data.Models;
using CritterHeroes.Web.Data.Models.Identity;
using CritterHeroes.Web.Features.Admin.Critters.Models;
using CritterHeroes.Web.Features.Admin.Critters.Queries;
using CritterHeroes.Web.Features.Admin.Critters.QueryHandlers;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TOTD.EntityFramework;

namespace CH.Test.AdminCrittersTests
{
    [TestClass]
    public class CrittersQueryHandlerTests
    {
        [TestMethod]
        public async Task ReturnsViewModel()
        {
            CritterStatus status1 = new CritterStatus("status1", "description1").SetEntityID(x => x.ID);
            CritterStatus status2 = new CritterStatus("status2", "description2").SetEntityID(x => x.ID);

            MockSqlStorageContext<CritterStatus> mockStatusStorage = new MockSqlStorageContext<CritterStatus>(status1, status2);

            Mock<IHttpUser> mockUser = new Mock<IHttpUser>();

            CrittersQuery query = new CrittersQuery()
            {
                StatusID = status2.ID
            };

            CrittersQueryHandler handler = new CrittersQueryHandler(mockStatusStorage.Object, mockUser.Object);
            CrittersModel model = await handler.ExecuteAsync(query);
            model.Should().NotBeNull();

            model.Query.Should().Be(query);
            model.ShowImport.Should().BeFalse();

            model.StatusItems.Should().HaveCount(2);

            SelectListItem resultItem1 = model.StatusItems.SingleOrDefault(x => x.Value == status1.ID.ToString());
            resultItem1.Should().NotBeNull();
            resultItem1.Text.Should().Be(status1.Name);
            resultItem1.Selected.Should().BeFalse();

            SelectListItem resultItem2 = model.StatusItems.SingleOrDefault(x => x.Value == status2.ID.ToString());
            resultItem2.Should().NotBeNull();
            resultItem2.Text.Should().Be(status2.Name);
            resultItem2.Selected.Should().BeTrue();

            mockUser.Verify(x => x.IsInRole(UserRole.MasterAdmin), Times.Once);
        }
    }
}
