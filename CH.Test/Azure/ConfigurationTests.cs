using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Azure;
using CH.Domain.Proxies.Configuration;
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
