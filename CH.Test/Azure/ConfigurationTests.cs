using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.DataProviders.Azure;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CH.Test.Azure
{
    [TestClass]
    public class ConfigurationTests
    {
        [TestMethod]
        public void GetsConnectionStringFromConfigurationFile()
        {
            new AzureConfiguration().ConnectionString.Should().NotBeNullOrEmpty();
        }
    }
}
