using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CH.Azure;
using CH.Domain.Proxies;
using CH.RescueGroups;
using CH.Website;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TOTD.Utility.Misc;

namespace CH.Test
{
    [TestClass]
    public class MiscTests : BaseTest
    {
        [TestMethod]
        public void NoAsyncMethodsShouldReturnVoid()
        {
            AssertMethodsListIsNullOrEmpty(UnitTestHelper.GetAsyncVoidMethods(GetType().Assembly), "no async methods should be returning null");
            AssertMethodsListIsNullOrEmpty(UnitTestHelper.GetAsyncVoidMethods(typeof(AzureStorage).Assembly), "no async methods should be returning null");
            AssertMethodsListIsNullOrEmpty(UnitTestHelper.GetAsyncVoidMethods(typeof(MvcApplication).Assembly), "no async methods should be returning null");
        }
    }
}
