using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Admin.DataMaintenance;
using CritterHeroes.Web.Areas.Admin.DataMaintenance.Models;
using CritterHeroes.Web.Areas.Admin.DataMaintenance.Sources;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TOTD.Utility.StringHelpers;

namespace CH.Test.ControllerTests
{
    [TestClass]
    public class DataMaintenanceControllerTests
    {
        [TestMethod]
        public void GetDashboardReturnsView()
        {
            DataMaintenanceController controller = new DataMaintenanceController(null,null);

            ViewResult viewResult = controller.Dashboard();
            viewResult.Should().NotBeNull();

            DashboardModel model = viewResult.Model as DashboardModel;
            model.Should().NotBeNull();

            model.TargetStorageItem.StorageSourceID.Should().Be(new AzureStorageSource().ID);
            model.TargetStorageItem.Title.Should().NotBeNullOrEmpty();

            model.SourceStorageItem.StorageSourceID.Should().Be(new RescueGroupsStorageSource().ID);
            model.SourceStorageItem.Title.Should().NotBeNullOrEmpty();

            model.Items.Should().NotBeNullOrEmpty();
            model.Items.Any(x => x.Title.IsNullOrEmpty()).Should().BeFalse();
        }
    }
}
