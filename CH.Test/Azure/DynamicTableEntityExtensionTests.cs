using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CH.Azure.Utility;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage.Table;

namespace CH.Test.Azure
{
    [TestClass]
    public class DynamicTableEntityExtensionTests
    {
        [TestMethod]
        public void SafeGetEntityPropertyStringValueReturnsPropertyValueIfItExists()
        {
            DynamicTableEntity entity = new DynamicTableEntity();
            entity.Properties["Test"] = new EntityProperty("value");
            entity.SafeGetEntityPropertyStringValue("Test").Should().Be("value");
        }

        [TestMethod]
        public void SafeGetEntityPropertyStringValueReturnsNullIfPropertyDoesntExist()
        {
            DynamicTableEntity entity = new DynamicTableEntity();
            entity.Properties.ContainsKey("Test").Should().BeFalse();
            entity.SafeGetEntityPropertyStringValue("Test").Should().BeNull();
        }
    }
}
