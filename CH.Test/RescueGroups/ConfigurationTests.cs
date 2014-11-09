using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.RescueGroups.Configuration;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CH.Test.RescueGroups
{
    [TestClass]
    public class ConfigurationTests
    {
        [TestMethod]
        public void ReadsConfigurationFromConfigurationFile()
        {
            RescueGroupsConfiguration configuration = new RescueGroupsConfiguration();
            configuration.Url.Should().NotBeNullOrEmpty();
            configuration.APIKey.Should().NotBeNullOrEmpty();
            configuration.AccountNumber.Should().NotBeNullOrEmpty();
            configuration.Username.Should().NotBeNullOrEmpty();
            configuration.Password.Should().NotBeNullOrEmpty();
        }
    }
}
