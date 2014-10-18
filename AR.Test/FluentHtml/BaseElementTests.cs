using System;
using System.Web.Mvc;
using AR.Website.Utility.FluentHtml.Elements;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        public void SuccessfullyAddsAttribute()
        {
            new TestElement(GetHtmlHelper())
                .Attribute("test", "value")
                .ToHtmlString()
                .Should()
                .Be("<test test=\"value\"></test>");
        }

        [TestMethod]
        public void SuccessfullyAddsDataAttribute()
        {
            new TestElement(GetHtmlHelper())
                .Data("test", "value")
                .ToHtmlString()
                .Should()
                .Be("<test data-test=\"value\"></test>");
        }

        [TestMethod]
        public void SuccessfullyAddsClass()
        {
            new TestElement(GetHtmlHelper())
                .Class("test")
                .ToHtmlString()
                .Should()
                .Be("<test class=\"test\"></test>");
        }

        [TestMethod]
        public void SuccessfullySetsElementID()
        {
            new TestElement(GetHtmlHelper())
                .ID("test")
                .ToHtmlString()
                .Should()
                .Be("<test id=\"test\"></test>");
        }

        [TestMethod]
        public void SuccessfullySetsElementName()
        {
            new TestElement(GetHtmlHelper())
                .Name("test")
                .ToHtmlString()
                .Should()
                .Be("<test name=\"test\"></test>");
        }
    }
}
