using System;
using System.Web.Mvc;
using AR.Website.Utility.FluentHtml.Elements;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AR.Test.FluentHtml
{
    [TestClass]
    public class LinkElementTests : BaseHtmlTest
    {
        [TestMethod]
        public void CreatesValidHtmlForElement()
        {
            new LinkElement(GetHtmlHelper())
                .ToHtmlString()
                .Should()
                .Be("<a></a>");
        }

        [TestMethod]
        public void CorrectlySetsInnerText()
        {
            new LinkElement(GetHtmlHelper())
                .Text("text")
                .ToHtmlString()
                .Should()
                .Be("<a>text</a>");
        }

        [TestMethod]
        public void HtmlEncodesInnerText()
        {
            new LinkElement(GetHtmlHelper())
                .Text("<br/>")
                .ToHtmlString()
                .Should()
                .Be("<a>&lt;br/&gt;</a>");
        }

        [TestMethod]
        public void CorrectlySetsJavascriptLink()
        {
            new LinkElement(GetHtmlHelper())
                .AsJavascriptLink()
                .ToHtmlString()
                .Should()
                .Be("<a href=\"#\"></a>");
        }

        [TestMethod]
        public void CorrectlySetsUrl()
        {
            new LinkElement(GetHtmlHelper())
                .Url("http://www.google.com")
                .ToHtmlString()
                .Should()
                .Be("<a href=\"http://www.google.com\"></a>");
        }

        [TestMethod]
        public void CorrectlySetsUrlFromControllerAction()
        {
            new LinkElement(GetHtmlHelper())
                .ActionLink<TestController>(x => x.TestAction(1))
                .ToHtmlString()
                .Should()
                .Be("<a href=\"/Test/TestAction?actionID=1\"></a>");
        }
    }
}
