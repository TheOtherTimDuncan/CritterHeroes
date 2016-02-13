﻿using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.DataProviders.RescueGroups.Configuration;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;
using CritterHeroes.Web.DataProviders.RescueGroups.Storage;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace CH.Test.RescueGroups
{
    [TestClass]
    public class CritterStatusSourceTests : BaseTest
    {
        [TestMethod]
        public void ObjectTypeIsCorrect()
        {
            new CritterStatusSourceStorage(new RescueGroupsConfiguration(), null, null).ObjectType.Should().Be("animalStatuses");
        }

        [TestMethod]
        public void ConvertsJsonResultToModel()
        {
            CritterStatusSource critterStatus1 = new CritterStatusSource("1", "Name 1", "Description 1");
            CritterStatusSource critterStatus2 = new CritterStatusSource("2", "Name 2", "Description 2");

            JProperty element1 = new JProperty("1", new JObject(new JProperty("name", critterStatus1.Name), new JProperty("description", critterStatus1.Description)));
            JProperty element2 = new JProperty("2", new JObject(new JProperty("name", critterStatus2.Name), new JProperty("description", critterStatus2.Description)));

            JObject data = new JObject();
            data.Add(element1);
            data.Add(element2);

            IEnumerable<CritterStatusSource> critterStatuses = new CritterStatusSourceStorage(new RescueGroupsConfiguration(), null, null).FromStorage(data.Properties());
            critterStatuses.Should().HaveCount(2);

            CritterStatusSource result1 = critterStatuses.FirstOrDefault(x => x.ID == critterStatus1.ID);
            result1.Should().NotBeNull();
            result1.Name.Should().Be(critterStatus1.Name);
            result1.Description.Should().Be(critterStatus1.Description);

            CritterStatusSource result2 = critterStatuses.FirstOrDefault(x => x.ID == critterStatus2.ID);
            result2.Should().NotBeNull();
            result2.Name.Should().Be(critterStatus2.Name);
            result2.Description.Should().Be(critterStatus2.Description);
        }
    }
}
