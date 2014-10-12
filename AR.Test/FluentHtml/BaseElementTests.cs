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
            public TestElement(HtmlHelper htmlHelper)
                : base("test", htmlHelper)
            {
            }
        }

        [TestMethod]
        public void CreatesValidHtmlForElement()
        {
            TestElement testElement = new TestElement(GetHtmlHelper());
            Assert.AreEqual(@"<test></test>", testElement.ToHtmlString());
        }

        [TestMethod]
        public void SuccessfullyAddsAttribute()
        {
            TestElement testElement = new TestElement(GetHtmlHelper()).Attribute("test", "value");
            Assert.AreEqual(@"<test test=""value""></test>", testElement.ToHtmlString());
        }

        [TestMethod]
        public void SuccessfullyAddsDataAttribute()
        {
            TestElement testElement = new TestElement(GetHtmlHelper()).Data("test", "value");
            Assert.AreEqual(@"<test data-test=""value""></test>", testElement.ToHtmlString());
        }

        [TestMethod]
        public void SuccessfullyAddsClass()
        {
            TestElement testElement = new TestElement(GetHtmlHelper()).Class("test");
            Assert.AreEqual(@"<test class=""test""></test>", testElement.ToHtmlString());
        }

        [TestMethod]
        public void SuccessfullySetsElementID()
        {
            TestElement testElement = new TestElement(GetHtmlHelper()).ID("test");
            Assert.AreEqual(@"<test id=""test""></test>", testElement.ToHtmlString());
        }

        [TestMethod]
        public void SuccessfullySetsElementName()
        {
            TestElement testElement = new TestElement(GetHtmlHelper()).Name("test");
            Assert.AreEqual(@"<test name=""test""></test>", testElement.ToHtmlString());
        }
    }
}
