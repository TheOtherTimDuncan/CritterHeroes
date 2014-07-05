using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AR.Website.Utility.FluentHtml.Elements;

namespace AR.Test.FluentHtml
{
    [TestClass]
    public class BaseElementTests : BaseHtmlTest
    {
        public class TestElement : Element<TestElement>
        {
            public TestElement(ViewContext viewContext)
                : base("test", viewContext)
            {
            }
        }

        [TestMethod]
        public void CreatesValidHtmlForElement()
        {
            TestElement testElement = new TestElement(GetViewContext());
            Assert.AreEqual(@"<test></test>", testElement.ToHtmlString());
        }

        [TestMethod]
        public void SuccessfullyAddsAttribute()
        {
            TestElement testElement = new TestElement(GetViewContext()).Attribute("test", "value");
            Assert.AreEqual(@"<test test=""value""></test>", testElement.ToHtmlString());
        }

        [TestMethod]
        public void SuccessfullyAddsDataAttribute()
        {
            TestElement testElement = new TestElement(GetViewContext()).Data("test", "value");
            Assert.AreEqual(@"<test data-test=""value""></test>", testElement.ToHtmlString());
        }

        [TestMethod]
        public void SuccessfullyAddsClass()
        {
            TestElement testElement = new TestElement(GetViewContext()).Class("test");
            Assert.AreEqual(@"<test class=""test""></test>", testElement.ToHtmlString());
        }
    }
}
