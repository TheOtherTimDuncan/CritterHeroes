using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AR.Website.Utility.FluentHtml.Elements;

namespace AR.Test.FluentHtml
{
    [TestClass]
    public class BaseElementTests
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
            ViewContext viewContext = new ViewContext();
            TestElement testElement = new TestElement(viewContext);
            Assert.AreEqual(@"<test></test>", testElement.ToHtmlString());
        }

        [TestMethod]
        public void SuccessfullyAddsAttribute()
        {
            ViewContext viewContext = new ViewContext();
            TestElement testElement = new TestElement(viewContext).Attribute("test", "value");
            Assert.AreEqual(@"<test test=""value""></test>", testElement.ToHtmlString());
        }

        [TestMethod]
        public void SuccessfullyAddsDataAttribute()
        {
            ViewContext viewContext = new ViewContext();
            TestElement testElement = new TestElement(viewContext).Data("test", "value");
            Assert.AreEqual(@"<test data-test=""value""></test>", testElement.ToHtmlString());
        }

        [TestMethod]
        public void SuccessfullyAddsClass()
        {
            ViewContext viewContext = new ViewContext();
            TestElement testElement = new TestElement(viewContext).Class("test");
            Assert.AreEqual(@"<test class=""test""></test>", testElement.ToHtmlString());
        }
    }
}
