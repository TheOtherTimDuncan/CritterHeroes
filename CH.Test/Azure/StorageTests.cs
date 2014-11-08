using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Azure;
using CH.Domain.Models.Data;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CH.Test.Azure
{
    [TestClass]
    public class StorageTests
    {
        [TestMethod]
        public void UsesConnectionStringFromConfigByDefault()
        {
            AzureStorage storage = new AzureStorage("fflah");
            storage.ConnectionString.Should().NotBeNullOrEmpty();
        }
    }
}
